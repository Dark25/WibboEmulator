﻿using Butterfly.Database.Interfaces;
using Butterfly.Game.Items;
using Butterfly.Game.Rooms.Wired.WiredHandlers.Interfaces;
using System.Data;

namespace Butterfly.Game.Rooms.Wired.WiredHandlers.Actions
{
    public class BotTeleport : WiredActionBase, IWired, IWiredEffect
    {
        public BotTeleport(Item item, Room room) : base(item, room, (int)WiredActionType.BOT_TELEPORT)
        {
        }

        public void Handle(RoomUser user, Item TriggerItem)
        {
            if (string.IsNullOrWhiteSpace(this.StringParam) || this.Items.Count == 0)
            {
                return;
            }

            RoomUser Bot = this.RoomInstance.GetRoomUserManager().GetBotOrPetByName(this.StringParam);
            if (Bot == null)
            {
                return;
            }

            Item roomItem = this.Items[ButterflyEnvironment.GetRandomNumber(0, this.Items.Count - 1)];
            if (roomItem == null)
            {
                return;
            }

            if (roomItem.Coordinate != Bot.Coordinate)
            {
                this.RoomInstance.GetGameMap().TeleportToItem(Bot, roomItem);
            }
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, this.Id, string.Empty, this.StringParam, false, this.Items);
        }

        public void LoadFromDatabase(DataRow row)
        {
            this.StringParam = row["trigger_data"].ToString();

            string triggerItems = row["triggers_item"].ToString();

            if (triggerItems == "")
                return;

            foreach (string itemId in triggerItems.Split(';'))
            {
                if (!int.TryParse(itemId, out int id))
                    continue;

                if (!this.StuffIds.Contains(id))
                    this.StuffIds.Add(id);
            }
        }
    }
}
