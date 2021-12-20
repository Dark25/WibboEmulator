namespace Butterfly.Communication.Packets.Outgoing.Help
{
    internal class OnGuideSessionInvitedToGuideRoomComposer : ServerPacket
    {
        public OnGuideSessionInvitedToGuideRoomComposer(int roomId, string name)
            : base(ServerPacketHeader.GUIDE_SESSION_INVITED_TO_GUIDE_ROOM)
        {
            WriteInteger(roomId);
            WriteString(name);
        }
    }
}