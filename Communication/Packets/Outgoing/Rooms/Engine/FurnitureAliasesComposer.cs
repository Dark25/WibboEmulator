namespace Butterfly.Communication.Packets.Outgoing.Rooms.Engine
{
    internal class FurnitureAliasesComposer : ServerPacket
    {
        public FurnitureAliasesComposer()
            : base(ServerPacketHeader.FURNITURE_ALIASES)
        {

        }
    }
}