﻿using System.Data;
using System.Drawing;
using WibboEmulator.Database.Interfaces;
using WibboEmulator.Games.Items.Wired.Interfaces;
using WibboEmulator.Games.Rooms;

namespace WibboEmulator.Games.Items.Wired.Conditions
{
    public class FurniHasNoUser : WiredConditionBase, IWiredCondition, IWired
    {
        public FurniHasNoUser(Item item, Room room) : base(item, room, (int)WiredConditionType.FURNI_NOT_HAVE_HABBO)
        {
        }

        public bool AllowsExecution(RoomUser user, Item TriggerItem)
        {
            foreach (Item item in this.Items.ToList())
            {
                foreach (Point coord in item.GetCoords)
                {
                    if (this.RoomInstance.GetGameMap().GetRoomUsers(coord).Count != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void SaveToDatabase(IQueryAdapter dbClient) => WiredUtillity.SaveTriggerItem(dbClient, this.Id, string.Empty, string.Empty, false, this.Items);

        public void LoadFromDatabase(DataRow row)
        {
            string triggerItems = row["triggers_item"].ToString();

            if (triggerItems == null || triggerItems == "")
            {
                return;
            }

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
