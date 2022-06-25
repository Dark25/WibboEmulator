﻿using WibboEmulator.Database.Interfaces;
using WibboEmulator.Game.Rooms;
using WibboEmulator.Game.Items.Wired.Interfaces;
using System.Data;

namespace WibboEmulator.Game.Items.Wired.Triggers
{
    public class EntersRoom : WiredTriggerBase, IWired
    {
        private readonly RoomEventDelegate delegateFunction;

        public EntersRoom(Item item, Room room) : base(item, room, (int)WiredTriggerType.AVATAR_ENTERS_ROOM)
        {
            this.delegateFunction = new RoomEventDelegate(this.OnUserEnter);
            this.RoomInstance.GetRoomUserManager().OnUserEnter += this.delegateFunction;
        }

        private void OnUserEnter(object sender, EventArgs e)
        {
            RoomUser user = (RoomUser)sender;
            if (user == null)
            {
                return;
            }

            if ((user.IsBot || string.IsNullOrEmpty(this.StringParam) || (string.IsNullOrEmpty(this.StringParam) || !(user.GetUsername() == this.StringParam))) && !string.IsNullOrEmpty(this.StringParam))
            {
                return;
            }

            if (this.RoomInstance.GetWiredHandler() != null)
            {
                this.RoomInstance.GetWiredHandler().ExecutePile(this.ItemInstance.Coordinate, user, null);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            this.RoomInstance.GetRoomUserManager().OnUserEnter -= this.delegateFunction;
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, this.Id, string.Empty, this.StringParam, false, null);
        }

        public void LoadFromDatabase(DataRow row)
        {
            this.StringParam = row["trigger_data"].ToString();
        }
    }
}
