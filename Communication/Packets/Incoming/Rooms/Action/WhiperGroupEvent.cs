using Butterfly.HabboHotel.GameClients;
using Butterfly.HabboHotel.Rooms;

namespace Butterfly.Communication.Packets.Incoming.Structure
{
    internal class WhiperGroupEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (room == null)
            {
                return;
            }

            RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabboId(Session.GetHabbo().Id);
            if (roomUserByHabbo == null)
            {
                return;
            }

            string name = Packet.PopString();

            RoomUser roomOtherUserByHabbo = room.GetRoomUserManager().GetRoomUserByName(name);
            if (roomOtherUserByHabbo == null)
            {
                return;
            }

            if (!roomUserByHabbo.WhiperGroupUsers.Contains(roomOtherUserByHabbo.GetUsername()))
            {
                if (roomUserByHabbo.WhiperGroupUsers.Count >= 5)
                {
                    return;
                }

                roomUserByHabbo.WhiperGroupUsers.Add(roomOtherUserByHabbo.GetUsername());
            }
            else
            {
                roomUserByHabbo.WhiperGroupUsers.Remove(roomOtherUserByHabbo.GetUsername());
            }
        }
    }
}