namespace WibboEmulator.WebSocketSharp;

#region License
/*
 * HttpResponse.cs
 *
 * The MIT License
 *
 * Copyright (c) 2012-2022 sta.blockhead
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using System;
using System.Collections.Specialized;
using System.IO;
using WibboEmulator.WebSocketSharp.Net;

internal sealed class HttpResponse : HttpBase
{
    #region Private Fields


    #endregion

    #region Private Constructors

    private HttpResponse(
      int code, string reason, Version version, NameValueCollection headers
    )
      : base(version, headers)
    {
        this.StatusCode = code;
        this.Reason = reason;
    }

    #endregion

    #region Internal Constructors

    internal HttpResponse(int code)
      : this(code, code.GetStatusDescription())
    {
    }

    internal HttpResponse(HttpStatusCode code)
      : this((int)code)
    {
    }

    internal HttpResponse(int code, string reason)
      : this(
          code,
          reason,
          HttpVersion.Version11,
          new NameValueCollection()
        ) => this.Headers["Server"] = "websocket-sharp/1.0";

    internal HttpResponse(HttpStatusCode code, string reason)
      : this((int)code, reason)
    {
    }

    #endregion

    #region Internal Properties

    internal string StatusLine => this.Reason != null
                   ? string.Format(
                       "HTTP/{0} {1} {2}{3}", this.ProtocolVersion, this.StatusCode, this.Reason, CrLf
                     )
                   : string.Format(
                       "HTTP/{0} {1}{2}", this.ProtocolVersion, this.StatusCode, CrLf
                     );

    #endregion

    #region Public Properties

    public bool CloseConnection
    {
        get
        {
            var compType = StringComparison.OrdinalIgnoreCase;

            return this.Headers.Contains("Connection", "close", compType);
        }
    }

    public CookieCollection Cookies => this.Headers.GetCookies(true);

    public bool IsProxyAuthenticationRequired => this.StatusCode == 407;

    public bool IsRedirect => this.StatusCode is 301 or 302;

    public bool IsSuccess => this.StatusCode is >= 200 and <= 299;

    public bool IsUnauthorized => this.StatusCode == 401;

    public bool IsWebSocketResponse => this.ProtocolVersion > HttpVersion.Version10
                   && this.StatusCode == 101
                   && this.Headers.Upgrades("websocket");

    public override string MessageHeader => this.StatusLine + this.HeaderSection;

    public string Reason { get; }

    public int StatusCode { get; }

    #endregion

    #region Internal Methods

    internal static HttpResponse CreateCloseResponse(HttpStatusCode code)
    {
        var ret = new HttpResponse(code);

        ret.Headers["Connection"] = "close";

        return ret;
    }

    internal static HttpResponse CreateUnauthorizedResponse(string challenge)
    {
        var ret = new HttpResponse(HttpStatusCode.Unauthorized);

        ret.Headers["WWW-Authenticate"] = challenge;

        return ret;
    }

    internal static HttpResponse CreateWebSocketHandshakeResponse()
    {
        var ret = new HttpResponse(HttpStatusCode.SwitchingProtocols);

        var headers = ret.Headers;

        headers["Upgrade"] = "websocket";
        headers["Connection"] = "Upgrade";

        return ret;
    }

    internal static HttpResponse Parse(string[] messageHeader)
    {
        var len = messageHeader.Length;

        if (len == 0)
        {
            var msg = "An empty response header.";

            throw new ArgumentException(msg);
        }

        var slParts = messageHeader[0].Split(new[] { ' ' }, 3);
        var plen = slParts.Length;

        if (plen < 2)
        {
            var msg = "It includes an invalid status line.";

            throw new ArgumentException(msg);
        }

        var code = slParts[1].ToInt32();
        var reason = plen == 3 ? slParts[2] : null;
        var ver = slParts[0][5..].ToVersion();

        var headers = new WebHeaderCollection();

        for (var i = 1; i < len; i++)
        {
            headers.InternalSet(messageHeader[i], true);
        }

        return new HttpResponse(code, reason, ver, headers);
    }

    internal static HttpResponse ReadResponse(
      Stream stream, int millisecondsTimeout
    ) => Read(stream, Parse, millisecondsTimeout);

    #endregion

    #region Public Methods

    public void SetCookies(CookieCollection cookies)
    {
        if (cookies == null || cookies.Count == 0)
        {
            return;
        }

        var headers = this.Headers;

        foreach (var cookie in cookies.Sorted)
        {
            var val = cookie.ToResponseString();

            headers.Add("Set-Cookie", val);
        }
    }

    #endregion
}
