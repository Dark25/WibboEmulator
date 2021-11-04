﻿using Butterfly.Communication.Packets.Outgoing.MarketPlace;
using Butterfly.Database.Daos;
using Butterfly.Database.Interfaces;
using Butterfly.HabboHotel.Catalog.Utilities;
using Butterfly.HabboHotel.Items;

namespace Butterfly.Communication.Packets.Incoming.Marketplace
{
    internal class MakeOfferEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int SellingPrice = Packet.PopInt();
            int ComissionPrice = Packet.PopInt();
            int ItemId = Packet.PopInt();

            Item Item = Session.GetHabbo().GetInventoryComponent().GetItem(ItemId);
            if (Item == null)
            {
                Session.SendPacket(new MarketplaceMakeOfferResultComposer(0));
                return;
            }

            if (!ItemUtility.IsRare(Item))
            {
                return;
            }

            if (SellingPrice > 999999 || SellingPrice <= 0)
            {
                Session.SendPacket(new MarketplaceMakeOfferResultComposer(0));
                return;
            }

            int Comission = ButterflyEnvironment.GetGame().GetCatalog().GetMarketplace().CalculateComissionPrice(SellingPrice);
            int TotalPrice = SellingPrice + Comission;
            int ItemType = 1;
            if (Item.GetBaseItem().Type == 'i')
            {
                ItemType++;
            }

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("DELETE items, items_limited FROM items LEFT JOIN items_limited ON(items_limited.item_id = items.id) WHERE id = '" + ItemId + "'");

                CatalogMarketplaceOfferDao.Insert(dbClient, Item.GetBaseItem().ItemName, Item.ExtraData, ItemId, Item.BaseItem, Session.GetHabbo().Id ,SellingPrice, TotalPrice, Item.GetBaseItem().SpriteId, ItemType, Item.Limited, Item.LimitedStack);

            }

            Session.GetHabbo().GetInventoryComponent().RemoveItem(ItemId);
            Session.SendPacket(new MarketplaceMakeOfferResultComposer(1));
        }
    }
}