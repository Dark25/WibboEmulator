namespace Butterfly.Communication.Packets.Outgoing.Rooms.Session
{
    internal class CantConnectComposer : ServerPacket
    {
        public CantConnectComposer(int Error)
            : base(ServerPacketHeader.ROOM_ENTER_ERROR)
        {
            this.WriteInteger(Error);
        }
    }
}