namespace Butterfly.Communication.Packets.Outgoing.Help
{
    internal class OnGuideSessionMsgComposer : ServerPacket
    {
        public OnGuideSessionMsgComposer(string message, int userId)
            : base(ServerPacketHeader.GUIDE_SESSION_MESSAGE)
        {
            WriteString(message);
            WriteInteger(userId);
        }
    }
}