using Butterfly.Communication.Packets.Outgoing.Users;
using Butterfly.Game.Clients;

namespace Butterfly.Communication.Packets.Incoming.Structure
{
    internal class CheckValidNameEvent : IPacketEvent
    {
        public void Parse(Client Session, ClientPacket Packet)
        {
            if (Session.GetHabbo() == null || Session == null)
            {
                return;
            }

            string Name = Packet.PopString();

            Session.SendPacket(new NameChangeUpdateComposer(Name));
        }
    }
}