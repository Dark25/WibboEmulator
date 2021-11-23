﻿using Butterfly.Communication.Packets.Outgoing;
using Butterfly.Database.Interfaces;
using Butterfly.Game.Clients;
using Butterfly.Game.Items;
using Butterfly.Game.Rooms.Wired.WiredHandlers.Interfaces;
using System;
using System.Data;

namespace Butterfly.Game.Rooms.Wired.WiredHandlers.Triggers
{
    public class UserCommand : WiredTriggerBase, IWired
    {
        private readonly RoomEventDelegate delegateFunction;

        public UserCommand(Item item, Room room) : base(item, room, (int)WiredTriggerType.COLLISION)
        {
            this.delegateFunction = new RoomEventDelegate(this.roomUserManager_OnUserSays);
            room.OnTrigger += this.delegateFunction;
        }

        private void roomUserManager_OnUserSays(object sender, EventArgs e)
        {
            RoomUser user = (RoomUser)sender;
            if (user == null || user.IsBot)
            {
                return;
            }

            this.RoomInstance.GetWiredHandler().ExecutePile(this.ItemInstance.Coordinate, user, null);
        }

        public override void Dispose()
        {
            base.Dispose();

            this.RoomInstance.GetWiredHandler().GetRoom().OnTrigger -= this.delegateFunction;
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {

        }

        public void LoadFromDatabase(DataRow row)
        {
        }
    }
}
