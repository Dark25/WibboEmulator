using WibboEmulator.Game.Items;
using WibboEmulator.Game.Rooms;

namespace WibboEmulator.Communication.Packets.Outgoing.Rooms.Engine
{
    internal class ItemWallComposer : ServerPacket
    {
        public ItemWallComposer(Item[] item, Room room)
            : base(ServerPacketHeader.ITEM_WALL)
        {
            this.WriteInteger(1); // total Owners
            this.WriteInteger(room.RoomData.OwnerId);
            this.WriteString(room.RoomData.OwnerName);

            this.WriteInteger(item.Length);

            foreach (Item Item in item)
            {
                this.WriteWallItem(Item, room.RoomData.OwnerId);
            }
        }

        private void WriteWallItem(Item item, int userId)
        {
            this.WriteString(item.Id.ToString());
            this.WriteInteger(item.GetBaseItem().SpriteId);
            this.WriteString(item.WallCoord ?? string.Empty);

            ItemBehaviourUtility.GenerateWallExtradata(item, this);

            this.WriteInteger(-1);
            this.WriteInteger((item.GetBaseItem().Modes > 1) ? 1 : 0);
            this.WriteInteger(userId);
        }
    }
}