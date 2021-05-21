namespace Butterfly.Communication.Packets.Outgoing.Structure
{
    internal class FlatCreatedComposer : ServerPacket
    {
        public FlatCreatedComposer(int roomID, string roomName)
            : base(ServerPacketHeader.ROOM_CREATED)
        {
            this.WriteInteger(roomID);
            this.WriteString(roomName);
        }
    }
}
