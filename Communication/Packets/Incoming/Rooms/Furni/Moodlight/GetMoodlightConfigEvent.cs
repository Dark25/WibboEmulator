using Butterfly.Communication.Packets.Outgoing;
using Butterfly.Communication.Packets.Outgoing.Rooms.Furni.Moodlight;
using Butterfly.Game.Clients;
using Butterfly.Game.Rooms;
using Butterfly.Game.Rooms.Moodlight;

namespace Butterfly.Communication.Packets.Incoming.Structure
{
    internal class GetMoodlightConfigEvent : IPacketEvent
    {
        public void Parse(Client Session, ClientPacket Packet)
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (room == null || !room.CheckRights(Session, true))
            {
                return;
            }

            if (room.MoodlightData == null || room.MoodlightData.Presets == null)
            {
                return;
            }

            Session.SendPacket(new MoodlightConfigComposer(room.MoodlightData));
        }
    }
}