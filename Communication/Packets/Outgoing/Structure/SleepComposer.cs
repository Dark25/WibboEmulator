using Butterfly.HabboHotel.Rooms;

namespace Butterfly.Communication.Packets.Outgoing.Structure
{
    internal class SleepComposer : ServerPacket
    {
        public SleepComposer(RoomUser User, bool IsSleeping)
            : base(ServerPacketHeader.UNIT_IDLE)
        {
            this.WriteInteger(User.VirtualId);
            this.WriteBoolean(IsSleeping);
        }
    }
}
