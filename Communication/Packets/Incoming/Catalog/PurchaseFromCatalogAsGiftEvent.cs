using Butterfly.Communication.Packets.Outgoing.Catalog;
using Butterfly.Communication.Packets.Outgoing.Inventory.Badges;
using Butterfly.Communication.Packets.Outgoing.Inventory.Furni;
using Butterfly.Communication.Packets.Outgoing.Inventory.Purse;
using Butterfly.Database.Daos;
using Butterfly.Database.Interfaces;
using Butterfly.Game.Catalog;
using Butterfly.Game.Catalog.Utilities;
using Butterfly.Game.Clients;
using Butterfly.Game.Guilds;
using Butterfly.Game.Items;
using Butterfly.Game.Users;
using Butterfly.Utilities;
using System;
using System.Linq;

namespace Butterfly.Communication.Packets.Incoming.Structure
{
    internal class PurchaseFromCatalogAsGiftEvent : IPacketEvent
    {
        public double Delay => 1000;

        public void Parse(Client Session, ClientPacket Packet)
        {
            int PageId = Packet.PopInt();
            int ItemId = Packet.PopInt();
            string Data = Packet.PopString();
            string GiftUser = StringCharFilter.Escape(Packet.PopString());
            string GiftMessage = StringCharFilter.Escape(Packet.PopString().Replace(Convert.ToChar(5), ' '));
            int SpriteId = Packet.PopInt();
            int Ribbon = Packet.PopInt();
            int Colour = Packet.PopInt();
            bool dnow = Packet.PopBoolean();

            if (!ButterflyEnvironment.GetGame().GetCatalog().TryGetPage(PageId, out CatalogPage Page))
            {
                return;
            }

            if (!Page.Enabled || Page.MinimumRank > Session.GetHabbo().Rank)
            {
                return;
            }

            if (!Page.Items.TryGetValue(ItemId, out CatalogItem Item))
            {
                return;
            }

            if (!ItemUtility.CanGiftItem(Item))
            {
                return;
            }

            if (!ButterflyEnvironment.GetGame().GetItemManager().GetGift(SpriteId, out ItemData PresentData) || PresentData.InteractionType != InteractionType.GIFT)
            {
                return;
            }

            int TotalCreditsCost = Item.CostCredits;
            int TotalPixelCost = Item.CostDuckets;
            int TotalDiamondCost = Item.CostWibboPoints;

            if (Session.GetHabbo().Credits < TotalCreditsCost || Session.GetHabbo().Duckets < TotalPixelCost || Session.GetHabbo().WibboPoints < TotalDiamondCost)
            {
                return;
            }

            User Habbo = ButterflyEnvironment.GetHabboByUsername(GiftUser);
            if (Habbo == null)
            {
                //Session.SendPacket(new GiftWrappingErrorComposer());
                return;
            }

            if ((DateTime.Now - Session.GetHabbo().LastGiftPurchaseTime).TotalSeconds <= 15.0)
            {
                Session.SendNotification(ButterflyEnvironment.GetLanguageManager().TryGetValue("notif.buygift.flood", Session.Langue));

                Session.GetHabbo().GiftPurchasingWarnings += 1;
                if (Session.GetHabbo().GiftPurchasingWarnings >= 25)
                {
                    Session.GetHabbo().SessionGiftBlocked = true;
                }

                return;
            }

            if (Session.GetHabbo().SessionGiftBlocked)
            {
                return;
            }

            string ED = Session.GetHabbo().Id + ";" + GiftMessage + Convert.ToChar(5) + Ribbon + Convert.ToChar(5) + Colour;

            int NewItemId = 0;
            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                NewItemId = ItemDao.Insert(dbClient, PresentData.Id, Habbo.Id, ED);

                string ItemExtraData = null;
                switch (Item.Data.InteractionType)
                {
                    case InteractionType.NONE:
                        ItemExtraData = "";
                        break;

                    case InteractionType.GUILD_ITEM:
                    case InteractionType.GUILD_GATE:
                        int Groupid = 0;
                        if (!int.TryParse(Data, out Groupid))
                        {
                            return;
                        }

                        if (Groupid == 0)
                        {
                            return;
                        }

                        Group groupItem;
                        if (ButterflyEnvironment.GetGame().GetGroupManager().TryGetGroup(Groupid, out groupItem))
                        {
                            ItemExtraData = "0;" + groupItem.Id;
                        }

                        break;

                    #region Pet handling

                    case InteractionType.PET:

                        try
                        {
                            string[] Bits = Data.Split('\n');
                            string PetName = Bits[0];
                            string Race = Bits[1];
                            string Color = Bits[2];

                            int.Parse(Race); // to trigger any possible errors

                            if (PetUtility.CheckPetName(PetName))
                            {
                                return;
                            }

                            if (Race.Length > 2)
                            {
                                return;
                            }

                            if (Color.Length != 6)
                            {
                                return;
                            }

                            ButterflyEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_PetLover", 1);
                        }
                        catch
                        {
                            return;
                        }

                        break;

                    #endregion

                    case InteractionType.FLOOR:
                    case InteractionType.WALLPAPER:
                    case InteractionType.LANDSCAPE:

                        double Number = 0;
                        try
                        {
                            if (string.IsNullOrEmpty(Data))
                            {
                                Number = 0;
                            }
                            else
                            {
                                Number = double.Parse(Data);
                            }
                        }
                        catch
                        {

                        }

                        ItemExtraData = Number.ToString().Replace(',', '.');
                        break; // maintain extra data // todo: validate

                    case InteractionType.POSTIT:
                        ItemExtraData = "FFFF33";
                        break;

                    case InteractionType.MOODLIGHT:
                        ItemExtraData = "1,1,1,#000000,255";
                        break;

                    case InteractionType.TROPHY:
                        ItemExtraData = Session.GetHabbo().Username + Convert.ToChar(9) + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + Convert.ToChar(9) + Data;
                        break;

                    case InteractionType.MANNEQUIN:
                        ItemExtraData = "m" + Convert.ToChar(5) + ".ch-210-1321.lg-285-92" + Convert.ToChar(5) + "Default Mannequin";
                        break;

                    case InteractionType.BADGE_TROC:
                        {
                            if (ButterflyEnvironment.GetGame().GetBadgeManager().HaveNotAllowed(Data) || !ButterflyEnvironment.GetGame().GetCatalog().HasBadge(Data))
                            {
                                Session.SendNotification(ButterflyEnvironment.GetLanguageManager().TryGetValue("notif.buybadgedisplay.error", Session.Langue));
                                return;
                            }

                            if (!Data.StartsWith("perso_"))
                            {
                                Session.GetHabbo().GetBadgeComponent().RemoveBadge(Data);
                            }

                            Session.SendPacket(new BadgesComposer(Session.GetHabbo().GetBadgeComponent().BadgeList));

                            ItemExtraData = Data;
                            break;
                        }

                    case InteractionType.BADGE_DISPLAY:
                        if (ButterflyEnvironment.GetGame().GetBadgeManager().HaveNotAllowed(Data) || !Session.GetHabbo().GetBadgeComponent().HasBadge(Data))
                        {
                            Session.SendNotification(ButterflyEnvironment.GetLanguageManager().TryGetValue("notif.buybadgedisplay.error", Session.Langue));
                            Session.SendPacket(new PurchaseOKComposer());
                            return;
                        }

                        ItemExtraData = Data + Convert.ToChar(9) + Session.GetHabbo().Username + Convert.ToChar(9) + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
                        break;

                    default:
                        ItemExtraData = Data;
                        break;
                }

                UserPresentDao.Insert(dbClient, NewItemId, Item.Data.Id, ItemExtraData);

                ItemDao.Delete(dbClient, NewItemId);
            }


            Item GiveItem = ItemFactory.CreateSingleItem(PresentData, Habbo, ED, NewItemId);
            if (GiveItem != null)
            {
                Client Receiver = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(Habbo.Id);
                if (Receiver != null)
                {
                    Receiver.GetHabbo().GetInventoryComponent().TryAddItem(GiveItem);
                    Receiver.SendPacket(new FurniListNotificationComposer(GiveItem.Id, 1));
                    Receiver.SendPacket(new PurchaseOKComposer());
                    //Receiver.SendPacket(new FurniListUpdateComposer());
                }

                if (Habbo.Id != Session.GetHabbo().Id && !string.IsNullOrWhiteSpace(GiftMessage))
                {
                    ButterflyEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_GiftGiver", 1);
                    if (Receiver != null)
                    {
                        ButterflyEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Receiver, "ACH_GiftReceiver", 1);
                    }
                }
            }

            Session.SendPacket(new PurchaseOKComposer(Item, PresentData));

            if (Item.CostCredits > 0)
            {
                Session.GetHabbo().Credits -= TotalCreditsCost;
                Session.SendPacket(new CreditBalanceComposer(Session.GetHabbo().Credits));
            }

            if (Item.CostDuckets > 0)
            {
                Session.GetHabbo().Duckets -= TotalPixelCost;
                Session.SendPacket(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Duckets));
            }

            if (Item.CostWibboPoints > 0)
            {
                Session.GetHabbo().WibboPoints -= TotalDiamondCost;
                Session.SendPacket(new HabboActivityPointNotificationComposer(Session.GetHabbo().WibboPoints, 0, 105));

                using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    UserDao.UpdateRemovePoints(dbClient, Session.GetHabbo().Id, TotalDiamondCost);
                }
            }

            Session.GetHabbo().LastGiftPurchaseTime = DateTime.Now;
        }
    }
}
