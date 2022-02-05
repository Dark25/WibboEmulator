using Butterfly.Communication.Packets.Outgoing.Rooms.Action;
using Butterfly.Game.Clients;
using Butterfly.Game.Users;

namespace Butterfly.Communication.Packets.Incoming.Structure
{
    internal class IgnoreUserEvent : IPacketEvent
    {
        public void Parse(Client Session, ClientPacket Packet)
        {
            if (Session.GetHabbo() == null)
            {
                return;
            }

            if (Session.GetHabbo().CurrentRoom == null)
            {
                return;
            }

            string UserName = Packet.PopString();

            Client gameclient = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(UserName);
            if (gameclient == null)
            {
                return;
            }

            User habbo = gameclient.GetHabbo();
            if (habbo == null || Session.GetHabbo().MutedUsers.Contains(habbo.Id))
            {
                return;
            }

            Session.GetHabbo().MutedUsers.Add(habbo.Id);

            Session.SendPacket(new IgnoreStatusComposer(1, UserName));
        }
    }
}