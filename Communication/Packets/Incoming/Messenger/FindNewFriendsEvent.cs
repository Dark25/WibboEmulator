using WibboEmulator.Communication.Packets.Outgoing.Rooms.Session;
using WibboEmulator.Game.Clients;

namespace WibboEmulator.Communication.Packets.Incoming.Structure
{
    internal class FindNewFriendsEvent : IPacketEvent
    {
        public double Delay => 0;

        public void Parse(Client Session, ClientPacket Packet)
        {
            Session.SendPacket(new RoomForwardComposer(447654));
        }
    }
}
