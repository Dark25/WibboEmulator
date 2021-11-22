﻿using System;
using System.Collections;
using System.IO;
using System.Text;

namespace ConsoleWriter
{
    public class Writer
    {
        private static bool mDisabled = false;

        public static bool DisabledState
        {
            get => mDisabled;
            set => mDisabled = value;
        }

        public static void WriteLine(string Line)
        {
            if (mDisabled)
            {
                return;
            }

            Console.WriteLine(Line);
        }

        public static void LogException(string logText)
        {
            WriteToFile("logs/exceptions.txt", logText + "\r\n\r\n");
            WriteLine("Exception has been saved");
        }

        public static void LogDDOS(string logText)
        {
            WriteToFile("logs/ddos.txt", logText + "\r\n\r\n");
            WriteLine("Exception has been saved");
        }

        public static void LogCriticalException(string logText)
        {
            WriteToFile("logs/criticalexceptions.txt", logText + "\r\n\r\n");
            WriteLine("CRITICAL ERROR LOGGED");
        }

        public static void LogCacheError(string logText)
        {
            WriteToFile("logs/cacheerror.txt", logText + "\r\n\r\n");
            WriteLine("Critical error saved");
        }

        public static void LogMessage(string logText)
        {
            Console.WriteLine(logText);
        }

        public static void LogDDOSS(string logText)
        {
            WriteToFile("logs/ddos.txt", logText + "\r\n\r\n");
            WriteLine(logText);
        }

        public static void LogThreadException(string Exception, string Threadname)
        {
            WriteToFile("logs/threaderror.txt", "Error in thread " + Threadname + ": \r\n" + Exception + "\r\n\r\n");
            WriteLine("Error in " + Threadname + " caught");
        }

        public static void LogQueryError(Exception Exception, string query)
        {
            WriteToFile("logs/MySQLerrors.txt", "Error in query: \r\n" + (object)query + "\r\n" + Exception.ToString() + "\r\n\r\n");
            WriteLine("Error in query caught");
        }

        public static void LogPacketException(string Packet, string Exception)
        {
            WriteToFile("logs/packeterror.txt", "Error in packet " + Packet + ": \r\n" + Exception + "\r\n\r\n");
            WriteLine("User disconnection logged: " + Exception);
        }

        public static void HandleException(Exception pException, string pLocation)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Exception logged " + DateTime.Now.ToString() + " in " + pLocation + ":");
            stringBuilder.AppendLine(pException.ToString());
            if (pException.InnerException != null)
            {
                stringBuilder.AppendLine("Inner exception:");
                stringBuilder.AppendLine(pException.InnerException.ToString());
            }
            if (pException.HelpLink != null)
            {
                stringBuilder.AppendLine("Help link:");
                stringBuilder.AppendLine(pException.HelpLink);
            }
            if (pException.Source != null)
            {
                stringBuilder.AppendLine("Source:");
                stringBuilder.AppendLine(pException.Source);
            }
            if (pException.Data != null)
            {
                stringBuilder.AppendLine("Data:");
                foreach (DictionaryEntry dictionaryEntry in pException.Data)
                {
                    stringBuilder.AppendLine("  Key: " + dictionaryEntry.Key + "Value: " + dictionaryEntry.Value);
                }
            }
            if (pException.Message != null)
            {
                stringBuilder.AppendLine("Message:");
                stringBuilder.AppendLine(pException.Message);
            }
            if (pException.StackTrace != null)
            {
                stringBuilder.AppendLine("Stack trace:");
                stringBuilder.AppendLine(pException.StackTrace);
            }
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();
            LogException(stringBuilder.ToString());
        }

        public static void DisablePrimaryWriting(bool ClearConsole)
        {
            mDisabled = true;
            if (!ClearConsole)
            {
                return;
            }

            Console.Clear();
        }

        public static void LogShutdown(StringBuilder builder)
        {
            WriteToFile("logs/shutdownlog.txt", builder.ToString());
        }

        private static void WriteToFile(string path, string content)
        {
            try
            {
                FileStream fileStream = new FileStream(Butterfly.ButterflyEnvironment.PatchDir + path, FileMode.Append, FileAccess.Write);
                byte[] bytes = Encoding.ASCII.GetBytes(Environment.NewLine + content);
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Dispose();
            }
            catch (Exception ex)
            {
                WriteLine("Could not write to file: " + ex.ToString() + ":" + content);
            }
        }
    }
}