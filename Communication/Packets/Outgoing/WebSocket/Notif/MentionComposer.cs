﻿namespace Butterfly.Communication.Packets.Outgoing.WebSocket
{
    internal class MentionComposer : ServerPacket
    {
        public MentionComposer(int UserId, string Username, string Look, string Msg)
         : base(24)
        {
            this.WriteInteger(UserId);
            this.WriteString(Username);
            this.WriteString(Look);
            this.WriteString(Msg);
        }
    }
}
