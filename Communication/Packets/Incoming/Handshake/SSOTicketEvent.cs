namespace WibboEmulator.Communication.Packets.Incoming.Handshake;
using WibboEmulator.Communication.Packets.Outgoing.BuildersClub;
using WibboEmulator.Communication.Packets.Outgoing.Handshake;
using WibboEmulator.Communication.Packets.Outgoing.Help;
using WibboEmulator.Communication.Packets.Outgoing.Inventory.Achievements;
using WibboEmulator.Communication.Packets.Outgoing.Inventory.AvatarEffects;
using WibboEmulator.Communication.Packets.Outgoing.Inventory.Purse;
using WibboEmulator.Communication.Packets.Outgoing.Misc;
using WibboEmulator.Communication.Packets.Outgoing.Moderation;
using WibboEmulator.Communication.Packets.Outgoing.Navigator;
using WibboEmulator.Communication.Packets.Outgoing.Notifications;
using WibboEmulator.Communication.Packets.Outgoing.Rooms.Notifications;
using WibboEmulator.Communication.Packets.Outgoing.Settings;
using WibboEmulator.Core;
using WibboEmulator.Core.Language;
using WibboEmulator.Database.Daos.Item;
using WibboEmulator.Database.Daos.Room;
using WibboEmulator.Database.Daos.User;
using WibboEmulator.Database.Interfaces;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Items;
using WibboEmulator.Games.Users.Authentificator;
using WibboEmulator.Utilities;

internal sealed class SSOTicketEvent : IPacketEvent
{
    public double Delay => 5000;

    public void Parse(GameClient session, ClientPacket packet)
    {
        if (session == null || session.User != null)
        {
            return;
        }

        var ssoTicket = packet.PopString();
        _ = packet.PopInt();

        if (string.IsNullOrEmpty(ssoTicket))
        {
            return;
        }

        if (WibboEnvironment.GetGame().GetGameClientManager().TryReconnection(ref session, ssoTicket))
        {
            session.SendPacket(new AuthenticationOKComposer());
            return;
        }

        if (session.IsDisconnected)
        {
            session.Disconnect();
            return;
        }

        try
        {
            using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();

            var ip = session.Connection.GetIp();
            var user = UserFactory.GetUserData(dbClient, ssoTicket, ip, session.MachineId);

            if (user == null)
            {
                session.Disconnect();
                return;
            }

            var packetList = new ServerPacketList();

            WibboEnvironment.GetGame().GetGameClientManager().LogClonesOut(user.Id);
            session.User = user;
            session.Langue = user.Langue;
            session.SSOTicket = ssoTicket;

            WibboEnvironment.GetGame().GetGameClientManager().RegisterClient(session, user.Id, user.Username, ssoTicket);

            if (session.Langue == Language.French)
            {
                WibboEnvironment.GetGame().GetGameClientManager().OnlineUsersFr++;
            }
            else if (session.Langue == Language.English)
            {
                WibboEnvironment.GetGame().GetGameClientManager().OnlineUsersEn++;
            }
            else if (session.Langue == Language.Portuguese)
            {
                WibboEnvironment.GetGame().GetGameClientManager().OnlineUsersBr++;
            }

            if (session.User.MachineId != session.MachineId && session.MachineId != null)
            {
                UserDao.UpdateMachineId(dbClient, session.User.Id, session.MachineId);
            }

            session.User.Init(dbClient, session);

            IsFirstConnexionToday(session, dbClient, packetList);

            session.SendPacket(new AuthenticationOKComposer());

            packetList.Add(new NavigatorHomeRoomComposer(session.User.HomeRoom, session.User.HomeRoom));
            //packetList.Add(new FavouritesComposer(session.User.FavoriteRooms));
            packetList.Add(new FigureSetIdsComposer());
            packetList.Add(new UserRightsComposer(session.User.Rank < 2 ? 2 : session.User.Rank, session.User.Rank > 1));
            packetList.Add(new AvailabilityStatusComposer());
            packetList.Add(new AchievementScoreComposer(session.User.AchievementPoints));
            packetList.Add(new BuildersClubMembershipComposer());
            packetList.Add(new CfhTopicsInitComposer(WibboEnvironment.GetGame().GetModerationManager().UserActionPresets));
            packetList.Add(new UserSettingsComposer(session.User.ClientVolume, session.User.OldChat, session.User.IgnoreRoomInvites, session.User.CameraFollowDisabled, 1, 0));
            //packetList.Add(new AvatarEffectsComposer(WibboEnvironment.GetGame().GetEffectManager().Effects));

            packetList.Add(new CreditBalanceComposer(session.User.Credits));

            // if (user.Rank > 12)
            // {
            //     var day = DateTime.Now.Day;
            //     var days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            //     var missDays = new List<int>();
            //     for (var i = 0; i < day; i++)
            //     {
            //         missDays.Add(i);
            //     }

            //     packetList.Add(new CampaignCalendarDataComposer("premium", "/album1584/LOL.gif", day, days, missDays, missDays));
            //     packetList.Add(new InClientLinkComposer("openView/calendar"));
            // }

            if (IsNewUser(session, dbClient))
            {
                packetList.Add(new NuxAlertComposer(2));
                packetList.Add(new InClientLinkComposer("nux/lobbyoffer/hide"));
            }

            if (session.User.HasPermission("mod"))
            {
                WibboEnvironment.GetGame().GetGameClientManager().AddUserStaff(session.User.Id);
                packetList.Add(new ModeratorInitComposer(
                    WibboEnvironment.GetGame().GetModerationManager().UserMessagePresets(),
                    WibboEnvironment.GetGame().GetModerationManager().RoomMessagePresets(),
                    WibboEnvironment.GetGame().GetModerationManager().Tickets()));
            }

            if (session.User.HasPermission("helptool"))
            {
                var guideManager = WibboEnvironment.GetGame().GetHelpManager();
                guideManager.AddGuide(session.User.Id);
                session.User.OnDuty = true;

                packetList.Add(new HelperToolComposer(session.User.OnDuty, guideManager.GuidesCount));
            }

            session.SendPacket(packetList);
        }
        catch (Exception ex)
        {
            ExceptionLogger.LogException("Invalid Dario bug duing user login: " + ex.ToString());
        }
    }

    private static void IsFirstConnexionToday(GameClient session, IQueryAdapter dbClient, ServerPacketList packetList)
    {
        if (!session.User.IsFirstConnexionToday)
        {
            return;
        }

        session.User.IsFirstConnexionToday = false;

        var notifImage = "";
        var nbLot = 0;
        var respectCount = 5;
        var creditCount = 10000;
        var wibboPointCount = 0;
        var winwinCount = 0;

        if (session.User.HasPermission("premium_legend"))
        {
            notifImage = "premium_legend";
            nbLot = 5;
            respectCount = 30;
            creditCount += creditCount * 3;
            wibboPointCount = 65;
            winwinCount = 100;
        }
        else if (session.User.HasPermission("premium_epic"))
        {
            notifImage = "premium_epic";
            nbLot = 3;
            respectCount = 20;
            creditCount += creditCount * 2;
            wibboPointCount = 32;
            winwinCount = 50;
        }
        else if (session.User.HasPermission("premium_classic"))
        {
            notifImage = "premium_classic";
            nbLot = 1;
            respectCount = 10;
            creditCount += creditCount;
            wibboPointCount = 12;
            winwinCount = 20;
        }

        if (nbLot > 0)
        {
            var lootboxId = WibboEnvironment.GetSettings().GetData<int>("givelot.lootbox.id");

            if (WibboEnvironment.GetGame().GetItemManager().GetItem(lootboxId, out var itemData))
            {
                var items = ItemFactory.CreateMultipleItems(dbClient, itemData, session.User, "", nbLot);

                foreach (var purchasedItem in items)
                {
                    session.User.InventoryComponent.TryAddItem(purchasedItem);
                }
            }
        }

        if (wibboPointCount > 0)
        {
            session.User.WibboPoints += wibboPointCount;
            session.SendPacket(new ActivityPointNotificationComposer(session.User.WibboPoints, 0, 105));

            UserDao.UpdateAddPoints(dbClient, session.User.Id, wibboPointCount);
        }

        if (winwinCount > 0)
        {
            UserStatsDao.UpdateAchievementScore(dbClient, session.User.Id, winwinCount);

            session.User.AchievementPoints += winwinCount;
        }

        session.User.Credits += creditCount;
        session.User.DailyRespectPoints = respectCount;
        session.User.DailyPetRespectPoints = respectCount;

        if (winwinCount > 0 || wibboPointCount > 0 || nbLot > 0)
        {
            packetList.Add(RoomNotificationComposer.SendBubble(notifImage, $"Vous avez reçu {wibboPointCount} WibboPoints, {winwinCount} Win-wins ainsi que {nbLot} LootBox!"));
        }
    }

    private static bool IsNewUser(GameClient session, IQueryAdapter dbClient)
    {
        if (!session.User.NewUser)
        {
            return false;
        }

        session.User.NewUser = false;

        var homeId = WibboEnvironment.GetSettings().GetData<int>("default.home.id");

        var roomId = RoomDao.InsertDuplicate(dbClient, session.User.Username, WibboEnvironment.GetLanguageManager().TryGetValue("room.welcome.desc", session.Langue));

        UserDao.UpdateNuxEnable(dbClient, session.User.Id, homeId > 0 ? homeId : roomId);

        session.User.HomeRoom = homeId > 0 ? homeId : roomId;

        if (roomId == 0)
        {
            return false;
        }

        ItemDao.InsertDuplicate(dbClient, session.User.Id, roomId);

        if (!session.User.UsersRooms.Contains(roomId))
        {
            session.User.UsersRooms.Add(roomId);
        }

        return true;
    }
}
