﻿using Butterfly.Communication.Packets.Outgoing;
using Butterfly.Database.Interfaces;
using Butterfly.Game.Clients;
using Butterfly.Game.Items;
using Butterfly.Game.Rooms.Wired.WiredHandlers.Interfaces;
using System.Data;

namespace Butterfly.Game.Rooms.Wired.WiredHandlers.Actions
{
    public class BotGiveHanditem : WiredActionBase, IWired, IWiredEffect
    {
        public BotGiveHanditem(Item item, Room room) : base(item, room, (int)WiredActionType.BOT_GIVE_HAND_ITEM)
        {
        }

        public void Handle(RoomUser user, Item TriggerItem)
        {
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            int handItemId = ((this.IntParams.Count > 0) ? this.IntParams[0] : 0);

            WiredUtillity.SaveTriggerItem(dbClient, this.Id, handItemId.ToString(), this.StringParam, false, null);
        }

        public void LoadFromDatabase(DataRow row)
        {
            this.StringParam = row["trigger_data"].ToString();

            if (int.TryParse(row["trigger_data2"].ToString(), out int handItemId))
                this.IntParams.Add(handItemId);
        }
    }
}