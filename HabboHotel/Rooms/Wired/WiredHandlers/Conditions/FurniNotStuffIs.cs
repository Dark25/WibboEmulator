﻿using Butterfly.Communication.Packets.Outgoing;
using Butterfly.Database.Interfaces;
using Butterfly.HabboHotel.GameClients;
using Butterfly.HabboHotel.Items;
using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Conditions
{
    public class FurniNotStuffIs : IWiredCondition, IWired
    {
        private readonly int itemID;
        private List<Item> items;
        private bool isDisposed;

        public FurniNotStuffIs(Item item, List<Item> items)
        {
            this.itemID = item.Id;
            this.isDisposed = false;

            this.items = items;
        }

        public bool AllowsExecution(RoomUser user, Item TriggerItem)
        {
            if (TriggerItem == null)
            {
                return false;
            }

            foreach (Item roomItem in this.items)
            {
                if (roomItem.BaseItem == TriggerItem.BaseItem && roomItem.ExtraData == TriggerItem.ExtraData)
                {
                    return false;
                }
            }
            return true;
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, this.itemID, string.Empty, string.Empty, false, this.items);
        }

        public void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom)
        {
            dbClient.SetQuery("SELECT triggers_item FROM wired_items WHERE trigger_id = " + this.itemID);
            DataRow row = dbClient.GetRow();

            if (row == null)
            {
                return;
            }

            string TriggerItemId = row["triggers_item"].ToString();

            if (TriggerItemId == "")
            {
                return;
            }

            foreach (string ItemId in TriggerItemId.Split(';'))
            {
                Item roomItem = insideRoom.GetRoomItemHandler().GetItem(Convert.ToInt32(ItemId));
                if (roomItem != null && !this.items.Contains(roomItem) && roomItem.Id != this.itemID)
                {
                    this.items.Add(roomItem);
                }
            }
        }

        public void OnTrigger(GameClient Session, int SpriteId)
        {
            ServerPacket Message18 = new ServerPacket(ServerPacketHeader.WIRED_CONDITION);
            Message18.WriteBoolean(false);
            Message18.WriteInteger(10);
            Message18.WriteInteger(this.items.Count);
            foreach (Item roomItem in this.items)
            {
                Message18.WriteInteger(roomItem.Id);
            }

            Message18.WriteInteger(SpriteId);
            Message18.WriteInteger(this.itemID);
            Message18.WriteInteger(0);
            Message18.WriteInteger(0);
            Message18.WriteInteger(0);
            Message18.WriteBoolean(false);
            Message18.WriteBoolean(true);
            Session.SendPacket(Message18);
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
            dbClient.RunQuery("DELETE FROM wired_items WHERE trigger_id = '" + this.itemID + "'");
        }

        public void Dispose()
        {
            this.isDisposed = true;
            if (this.items != null)
            {
                this.items.Clear();
            }

            this.items = null;
        }

        public bool Disposed()
        {
            return this.isDisposed;
        }
    }
}
