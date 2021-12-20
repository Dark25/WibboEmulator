﻿using Butterfly.Communication.Packets.Outgoing;
using Butterfly.Communication.Packets.Outgoing.Handshake;
using Butterfly.Communication.Packets.Outgoing.Help;
using Butterfly.Communication.Packets.Outgoing.Inventory.Purse;
using Butterfly.Communication.Packets.Outgoing.Navigator;
using Butterfly.Communication.Packets.Outgoing.Rooms.Engine;
using Butterfly.Communication.Packets.Outgoing.Rooms.Session;
using Butterfly.Core;
using Butterfly.Database.Daos;
using Butterfly.Database.Interfaces;
using Butterfly.Game.Chat.Logs;
using Butterfly.Game.Clients;
using Butterfly.Game.Roleplay;
using Butterfly.Game.Roleplay.Player;
using Butterfly.Game.Rooms;
using Butterfly.Game.Users.Achievements;
using Butterfly.Game.Users.Badges;
using Butterfly.Game.Users.Inventory;
using Butterfly.Game.Users.Messenger;
using Butterfly.Game.Users.Wardrobes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Butterfly.Game.Users
{
    public class User
    {
        public int Id;
        public string Username;
        public int Rank;
        public string Motto;
        public string Look;
        public string BackupLook;
        public string Gender;
        public string BackupGender;
        public bool LastMovFGate;
        public int Credits;
        public int WibboPoints;
        public int AccountCreated;
        public int AchievementPoints;
        public int Duckets;
        public int Respect;
        public int DailyRespectPoints;
        public int DailyPetRespectPoints;
        public int CurrentRoomId;
        public int LoadingRoomId;
        public int HomeRoom;
        public int LastOnline;
        public bool IsTeleporting;
        public int TeleportingRoomID;
        public int TeleporterId;
        public List<int> ClientVolume;
        public string MachineId;
        public Language Langue;

        public bool ForceOpenGift;
        public int ForceUse = -1;
        public int ForceRot = -1;

        public List<RoomData> RoomRightsList;
        public List<RoomData> FavoriteRooms;
        public List<RoomData> UsersRooms;
        public List<int> MutedUsers;
        public List<int> RatedRooms;
        public List<int> MyGroups;
        public Dictionary<int, int> Quests;
        public Dictionary<double, RoomData> Visits;

        private MessengerComponent _messengerComponent;
        private BadgeComponent _badgeComponent;
        private AchievementComponent _achievementComponent;
        private InventoryComponent _inventoryComponent;
        private WardrobeComponent _wardrobeComponent;
        private ChatlogManager _chatMessageManager;

        public Client ClientInstance { get; private set; }
        public bool SpectatorMode;
        public bool Disconnected;
        public bool HasFriendRequestsDisabled;
        public int FavouriteGroupId;

        public int FloodCount;
        public DateTime FloodTime;
        public bool SpamEnable;
        public int SpamProtectionTime;
        public DateTime SpamFloodTime;
        public DateTime EveryoneTimer;
        public DateTime LastGiftPurchaseTime;

        public int CurrentQuestId;
        public int LastCompleted;
        public int LastQuestId;
        public bool HabboinfoSaved;
        public bool AcceptTrading;
        public bool HideInRoom;
        public int PubDectectCount = 0;
        public DateTime OnlineTime;
        public bool PremiumProtect;
        public int ControlUserId;
        public string IP;
        public bool ViewMurmur = true;
        public bool HideOnline;
        public string LastPhotoId;

        public int GuideOtherUserId;
        public bool OnDuty;

        public int Mazo;
        public int MazoHighScore;

        public bool NewUser;
        public bool Nuxenable;
        public int PassedNuxCount;

        public bool AllowDoorBell;
        public bool CanChangeName;
        public int GiftPurchasingWarnings;
        public bool SessionGiftBlocked;

        public int RolePlayId;
        public double IgnoreAllExpireTime;
        public bool IgnoreAll
        {
            get
            {
                return this.IgnoreAllExpireTime > ButterflyEnvironment.GetUnixTimestamp();
            }
        }

        public bool InRoom => this.CurrentRoomId > 0;

        public Room CurrentRoom
        {
            get
            {
                if (this.CurrentRoomId <= 0)
                {
                    return null;
                }
                else
                {
                    return ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.CurrentRoomId);
                }
            }
        }

        public bool SendWebPacket(IServerPacket Message)
        {
            WebClients.WebClient ClientWeb = ButterflyEnvironment.GetGame().GetClientWebManager().GetClientByUserID(this.Id);
            if (ClientWeb != null)
            {
                ClientWeb.SendPacket(Message);
                return true;
            }

            return false;
        }

        public User(int Id, string Username, int Rank, string Motto, string Look, string Gender, int Credits,
            int WPoint, int ActivityPoints, int HomeRoom, int Respect, int DailyRespectPoints,
            int DailyPetRespectPoints, bool HasFriendRequestsDisabled, int currentQuestID, int achievementPoints,
            int LastOnline, int FavoriteGroup, int accountCreated, bool accepttrading, string ip, bool HideInroom,
            bool HideOnline, int MazoHighScore, int Mazo, string clientVolume, bool nuxenable, string MachineId, bool ChangeName, Language Langue, int ignoreAllExpire)
        {
            this.Id = Id;
            this.Username = Username;
            this.Rank = Rank;
            this.Motto = Motto;
            this.MachineId = MachineId;
            this.Look = ButterflyEnvironment.GetFigureManager().ProcessFigure(Look, Gender, true);
            this.Gender = Gender.ToLower();
            this.Credits = Credits;
            this.WibboPoints = WPoint;
            this.Duckets = ActivityPoints;
            this.AchievementPoints = achievementPoints;
            this.CurrentRoomId = 0;
            this.LoadingRoomId = 0;
            this.HomeRoom = HomeRoom;
            this.FavoriteRooms = new List<RoomData>();
            this.RoomRightsList = new List<RoomData>();
            this.UsersRooms = new List<RoomData>();
            this.MutedUsers = new List<int>();
            this.RatedRooms = new List<int>();
            this.Respect = Respect;
            this.DailyRespectPoints = DailyRespectPoints;
            this.DailyPetRespectPoints = DailyPetRespectPoints;
            this.IsTeleporting = false;
            this.TeleporterId = 0;
            this.HasFriendRequestsDisabled = HasFriendRequestsDisabled;
            this.ClientVolume = new List<int>(3);
            this.CanChangeName = ChangeName;
            this.Langue = Langue;
            this.IgnoreAllExpireTime = ignoreAllExpire;


            if (clientVolume.Contains(','))
            {
                foreach (string Str in clientVolume.Split(','))
                {
                    if (int.TryParse(Str, out int Val))
                    {
                        this.ClientVolume.Add(int.Parse(Str));
                    }
                    else
                    {
                        this.ClientVolume.Add(100);
                    }
                }
            }
            else
            {
                this.ClientVolume.Add(100);
                this.ClientVolume.Add(100);
                this.ClientVolume.Add(100);
            }

            this.LastOnline = LastOnline;
            this.MyGroups = new List<int>();
            this.Quests = new Dictionary<int, int>();
            this.FavouriteGroupId = FavoriteGroup;

            this.AccountCreated = accountCreated;

            this.CurrentQuestId = currentQuestID;
            this.AcceptTrading = accepttrading;

            this.OnlineTime = DateTime.Now;
            this.PremiumProtect = (this.Rank > 1);

            this.ControlUserId = 0;
            this.IP = ip;
            this.SpectatorMode = false;
            this.Disconnected = false;
            this.HideInRoom = HideInroom;
            this.HideOnline = HideOnline;
            this.MazoHighScore = MazoHighScore;
            this.Mazo = Mazo;

            this.LastGiftPurchaseTime = DateTime.Now;

            this.Nuxenable = nuxenable;
            this.NewUser = nuxenable;
            this.Visits = new Dictionary<double, RoomData>();
        }

        public void Init(Client client)
        {
            this.ClientInstance = client;

            this._badgeComponent = new BadgeComponent(this);
            this._achievementComponent = new AchievementComponent(this);
            this._inventoryComponent = new InventoryComponent(this);
            this._wardrobeComponent = new WardrobeComponent(this);
            this._messengerComponent = new MessengerComponent(this);
            this._chatMessageManager = new ChatlogManager();

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                this._badgeComponent.Init(dbClient);
                this._wardrobeComponent.Init(dbClient);
                this._achievementComponent.Init(dbClient);
                this._messengerComponent.Init(dbClient, this.HideOnline);
                this._chatMessageManager.LoadUserChatlogs(dbClient, this.Id);

                DataTable dUserRooms = RoomDao.GetAllByOwner(dbClient, this.Username);
                foreach (DataRow dRow in dUserRooms.Rows)
                {
                    this.UsersRooms.Add(ButterflyEnvironment.GetGame().GetRoomManager().FetchRoomData(Convert.ToInt32(dRow["id"]), dRow));
                }

                DataTable dGroupMemberships = GuildMembershipDao.GetOneByUserId(dbClient, this.Id);
                foreach (DataRow dRow in dGroupMemberships.Rows)
                {
                    this.MyGroups.Add(Convert.ToInt32(dRow["group_id"]));
                }

                DataTable dQuests = UserQuestDao.GetAll(dbClient, this.Id);
                foreach (DataRow dataRow in dQuests.Rows)
                {
                    int questId = Convert.ToInt32(dataRow["quest_id"]);
                    int progress = Convert.ToInt32(dataRow["progress"]);
                    this.Quests.Add(questId, progress);
                }

                DataTable dFavorites = UserFavoriteDao.GetAll(dbClient, this.Id);
                foreach (DataRow dataRow in dFavorites.Rows)
                {
                    int roomId = Convert.ToInt32(dataRow["room_id"]);

                    RoomData roomdata = ButterflyEnvironment.GetGame().GetRoomManager().GenerateRoomData(roomId);
                    if(roomdata != null)
                        this.FavoriteRooms.Add(roomdata);
                }

                DataTable dRoomRights = RoomRightDao.GetAllByUserId(dbClient, this.Id);
                foreach (DataRow dataRow in dRoomRights.Rows)
                {
                    int roomId = Convert.ToInt32(dataRow["room_id"]);

                    RoomData roomdata = ButterflyEnvironment.GetGame().GetRoomManager().GenerateRoomData(roomId);
                    if(roomdata != null)
                        this.RoomRightsList.Add(roomdata);
                }
            }
        }

        public void PrepareRoom(int Id, string Password = "", bool override_doorbell = false)
        {
            if (this.GetClient() == null || this.GetClient().GetHabbo() == null)
            {
                return;
            }

            if (this.GetClient().GetHabbo().InRoom)
            {
                Room OldRoom = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.GetClient().GetHabbo().CurrentRoomId);

                if (OldRoom != null)
                {
                    OldRoom.GetRoomUserManager().RemoveUserFromRoom(this.GetClient(), false, false);
                }
            }

            Room room = ButterflyEnvironment.GetGame().GetRoomManager().LoadRoom(Id);
            if (room == null)
            {
                this.GetClient().SendPacket(new CloseConnectionComposer());
                return;
            }

            if (this.GetClient().GetHabbo().IsTeleporting && this.GetClient().GetHabbo().TeleportingRoomID != Id)
            {
                this.GetClient().GetHabbo().TeleportingRoomID = 0;
                this.GetClient().GetHabbo().IsTeleporting = false;
                this.GetClient().GetHabbo().TeleporterId = 0;
                this.GetClient().SendPacket(new CloseConnectionComposer());

                return;
            }

            if (!this.GetClient().GetHabbo().HasFuse("fuse_mod") && room.UserIsBanned(this.GetClient().GetHabbo().Id))
            {
                if (room.HasBanExpired(this.GetClient().GetHabbo().Id))
                {
                    room.RemoveBan(this.GetClient().GetHabbo().Id);
                }
                else
                {
                    this.GetClient().SendPacket(new CantConnectComposer(1));

                    this.GetClient().SendPacket(new CloseConnectionComposer());
                    return;
                }
            }

            if (room.RoomData.UsersNow >= room.RoomData.UsersMax && !this.GetClient().GetHabbo().HasFuse("fuse_enter_full_rooms") && !ButterflyEnvironment.GetGame().GetPermissionManager().RankHasRight(this.GetClient().GetHabbo().Rank, "fuse_enter_full_rooms"))
            {
                if (room.CloseFullRoom)
                {
                    room.RoomData.State = 1;
                    room.CloseFullRoom = false;
                }

                if (this.GetClient().GetHabbo().Id != room.RoomData.OwnerId)
                {
                    this.GetClient().SendPacket(new CantConnectComposer(1));

                    this.GetClient().SendPacket(new CloseConnectionComposer());
                    return;
                }
            }

            string[] OwnerEnterNotAllowed = { "WibboGame", "LieuPublic", "WorldRunOff", "SeasonRunOff", "CasinoRunOff", "WibboParty", "MovieRunOff", "officialrooms", "Seonsaengnim", "Jason", "Ximbay" };

            //if (!this.GetClient().GetHabbo().HasFuse("fuse_mod"))
            if (this.GetClient().GetHabbo().Rank < 8)
            {
                if (!(this.GetClient().GetHabbo().HasFuse("fuse_enter_any_room") && !OwnerEnterNotAllowed.Any(x => x == room.RoomData.OwnerName)) && !room.CheckRights(this.GetClient(), true) && !(this.GetClient().GetHabbo().IsTeleporting && this.GetClient().GetHabbo().TeleportingRoomID == room.Id))
                {
                    if (room.RoomData.State == 1 && (!override_doorbell && !room.CheckRights(this.GetClient())))
                    {
                        if (room.UserCount == 0)
                        {
                            ServerPacket message = new ServerPacket(ServerPacketHeader.ROOM_DOORBELL_DENIED);
                            this.GetClient().SendPacket(message);
                        }
                        else
                        {
                            this.GetClient().SendPacket(new DoorbellComposer(""));
                            room.SendPacket(new DoorbellComposer(this.GetClient().GetHabbo().Username), true);
                            this.GetClient().GetHabbo().LoadingRoomId = Id;
                            this.GetClient().GetHabbo().AllowDoorBell = false;
                        }
                        return;
                    }
                    else if (room.RoomData.State == 2 && Password.ToLower() != room.RoomData.Password.ToLower())
                    {
                        this.GetClient().SendPacket(new GenericErrorComposer(-100002));
                        this.GetClient().SendPacket(new CloseConnectionComposer());
                        return;
                    }
                }
            }

            if (room.RoomData.OwnerName == "WibboGame" || room.RoomData.OwnerName == "WibboParty")
            {
                if (room.GetRoomUserManager().GetUserByTracker(this.IP, this.GetClient().MachineId) != null)
                {
                    this.GetClient().SendPacket(new CloseConnectionComposer());
                    return;
                }
            }

            if (!this.EnterRoom(room))
            {
                this.GetClient().SendPacket(new CloseConnectionComposer());
            }
            else
            {
                this.GetClient().GetHabbo().LoadingRoomId = Id;
                this.GetClient().GetHabbo().AllowDoorBell = true;
            }

        }

        public bool EnterRoom(Room Room)
        {
            Client Session = this.GetClient();
            if (Session == null)
            {
                return false;
            }

            if (Room == null)
            {
                return false;
            }

            Session.SendPacket(new RoomReadyComposer(Room.Id, Room.RoomData.ModelName));

            if (Room.RoomData.Wallpaper != "0.0")
            {
                Session.SendPacket(new RoomPropertyComposer("wallpaper", Room.RoomData.Wallpaper));
            }

            if (Room.RoomData.Floor != "0.0")
            {
                Session.SendPacket(new RoomPropertyComposer("floor", Room.RoomData.Floor));
            }

            Session.SendPacket(new RoomPropertyComposer("landscape", Room.RoomData.Landscape));
            Session.SendPacket(new RoomRatingComposer(Room.RoomData.Score, !(Session.GetHabbo().RatedRooms.Contains(Room.Id) || Room.RoomData.OwnerId == Session.GetHabbo().Id)));


            return true;
        }

        public bool HasFuse(string Fuse)
        {
            if (ButterflyEnvironment.GetGame().GetPermissionManager().RankHasRight(this.Rank, Fuse))
            {
                return true;
            }

            return false;
        }

        public void OnDisconnect()
        {
            if (this.Disconnected)
            {
                return;
            }

            this.Disconnected = true;

            ButterflyEnvironment.GetGame().GetClientManager().UnregisterClient(this.Id, this.Username);

            if (this.HasFuse("fuse_mod"))
            {
                ButterflyEnvironment.GetGame().GetClientManager().RemoveUserStaff(this.Id);
            }

            Logging.WriteLine(this.Username + " has logged out.");

            if (!this.HabboinfoSaved)
            {
                this.HabboinfoSaved = true;
                TimeSpan TimeOnline = DateTime.Now - this.OnlineTime;
                int TimeOnlineSec = (int)TimeOnline.TotalSeconds;
                using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    UserDao.UpdateOffline(dbClient, this.Id, this.Duckets, this.Credits);
                    UserStatsDao.UpdateAll(dbClient, this.Id, this.FavouriteGroupId, TimeOnlineSec, this.CurrentQuestId, this.Respect, this.DailyRespectPoints, this.DailyPetRespectPoints);
                }
            }

            if (this.InRoom && this.CurrentRoom != null)
            {
                this.CurrentRoom.GetRoomUserManager().RemoveUserFromRoom(this.ClientInstance, false, false);
            }

            if (this.RolePlayId > 0)
            {
                RolePlayerManager RPManager = ButterflyEnvironment.GetGame().GetRoleplayManager().GetRolePlay(this.RolePlayId);
                if (RPManager != null)
                {
                    RolePlayer Rp = RPManager.GetPlayer(this.Id);
                    if (Rp != null)
                    {
                        RPManager.RemovePlayer(this.Id);
                    }
                }
                this.RolePlayId = 0;
            }

            if (this.GuideOtherUserId != 0)
            {
                Client requester = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(this.GuideOtherUserId);
                if (requester != null)
                {
                    requester.SendPacket(new OnGuideSessionEndedComposer(1));

                    requester.GetHabbo().GuideOtherUserId = 0;
                }
            }
            if (this.OnDuty)
            {
                ButterflyEnvironment.GetGame().GetHelpManager().RemoveGuide(this.Id);
            }

            if (this._messengerComponent != null)
            {
                this._messengerComponent.AppearOffline = true;
                this._messengerComponent.Dispose();
            }

            if (this._inventoryComponent != null)
            {
                this._inventoryComponent.Dispose();
                this._inventoryComponent = null;
            }

            if (this._badgeComponent != null)
            {
                this._badgeComponent.Dispose();
                this._badgeComponent = null;
            }

            if (this._wardrobeComponent != null)
            {
                this._wardrobeComponent.Dispose();
                this._wardrobeComponent = null;
            }

            if (this._achievementComponent != null)
            {
                this._achievementComponent.Dispose();
                this._achievementComponent = null;
            }

            if (this.UsersRooms != null)
            {
                this.UsersRooms.Clear();
            }

            if (this.RoomRightsList != null)
            {
                this.RoomRightsList.Clear();
            }

            if (this.FavoriteRooms != null)
            {
                this.FavoriteRooms.Clear();
            }

            this.ClientInstance = null;
        }

        public void UpdateCreditsBalance()
        {
            Client client = this.GetClient();
            if (client == null)
            {
                return;
            }

            client.SendPacket(new CreditBalanceComposer(this.Credits));
        }

        public void UpdateActivityPointsBalance()
        {
            Client client = this.GetClient();
            if (client == null)
            {
                return;
            }

            client.SendPacket(new HabboActivityPointNotificationComposer(this.Duckets, 1));
        }

        public Client GetClient()
        {
            return ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(this.Id);
        }
        
        public MessengerComponent GetMessenger()
        {
            return this._messengerComponent;
        }

        public WardrobeComponent GetWardrobeComponent()
        {
            return this._wardrobeComponent;
        }

        public AchievementComponent GetAchievementComponent()
        {
            return this._achievementComponent;
        }

        public BadgeComponent GetBadgeComponent()
        {
            return this._badgeComponent;
        }

        public InventoryComponent GetInventoryComponent()
        {
            return this._inventoryComponent;
        }

        public ChatlogManager GetChatMessageManager()
        {
            return this._chatMessageManager;
        }

        public int GetQuestProgress(int p)
        {
            this.Quests.TryGetValue(p, out int num);
            return num;
        }
    }
}