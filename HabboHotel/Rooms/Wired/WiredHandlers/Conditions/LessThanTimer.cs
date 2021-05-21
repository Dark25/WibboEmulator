﻿using Butterfly.Communication.Packets.Outgoing;
using Butterfly.Database.Interfaces;
using Butterfly.HabboHotel.GameClients;
using Butterfly.HabboHotel.Items;
using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
using System;
using System.Data;

namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Conditions
{
    public class LessThanTimer : IWiredCondition, IWired
    {
        private int timeout;
        private Room room;
        private Item item;
        private bool isDisposed;

        public LessThanTimer(int timeout, Room room, Item item)
        {
            this.timeout = timeout;
            this.room = room;
            this.isDisposed = false;
            this.item = item;
        }

        public bool AllowsExecution(RoomUser user, Item TriggerItem)
        {
            DateTime dateTime = this.room.lastTimerReset;
            return (DateTime.Now - dateTime).TotalSeconds < this.timeout / 2;
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, this.item.Id, string.Empty, this.timeout.ToString(), false, null);
        }

        public void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom)
        {
            dbClient.SetQuery("SELECT trigger_data FROM wired_items WHERE trigger_id = @id ");
            dbClient.AddParameter("id", this.item.Id);
            DataRow row = dbClient.GetRow();
            if (row != null)
            {
                this.timeout = Convert.ToInt32(row[0].ToString());
            }
            else
            {
                this.timeout = 20;
            }
        }

        public void OnTrigger(GameClient Session, int SpriteId)
        {
            ServerPacket Message20 = new ServerPacket(ServerPacketHeader.WIRED_CONDITION);
            Message20.WriteBoolean(false);
            Message20.WriteInteger(5);
            Message20.WriteInteger(0);
            Message20.WriteInteger(SpriteId);
            Message20.WriteInteger(this.item.Id);
            Message20.WriteString("");
            Message20.WriteInteger(1);
            Message20.WriteInteger(this.timeout);
            Message20.WriteInteger(1);
            Message20.WriteInteger(3);
            Message20.WriteInteger(0);
            Message20.WriteInteger(0);
            Session.SendPacket(Message20);
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
            dbClient.RunQuery("DELETE FROM wired_items WHERE trigger_id = '" + this.item.Id + "'");
        }

        public void Dispose()
        {
            this.isDisposed = true;
            this.room = null;
            this.item = null;
        }

        public bool Disposed()
        {
            return this.isDisposed;
        }
    }
}
