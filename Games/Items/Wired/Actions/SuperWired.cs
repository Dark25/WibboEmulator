namespace WibboEmulator.Games.Items.Wired.Actions;
using System.Data;
using WibboEmulator.Communication.Packets.Outgoing.Inventory.Badges;
using WibboEmulator.Communication.Packets.Outgoing.Inventory.Furni;
using WibboEmulator.Communication.Packets.Outgoing.Notifications;
using WibboEmulator.Communication.Packets.Outgoing.RolePlay;
using WibboEmulator.Communication.Packets.Outgoing.Rooms.Avatar;
using WibboEmulator.Communication.Packets.Outgoing.Rooms.Engine;
using WibboEmulator.Communication.Packets.Outgoing.Rooms.Session;
using WibboEmulator.Communication.Packets.Outgoing.Sound;
using WibboEmulator.Communication.Packets.Outgoing.Users;
using WibboEmulator.Database.Daos.Roleplay;
using WibboEmulator.Database.Daos.User;
using WibboEmulator.Database.Interfaces;
using WibboEmulator.Games.Items.Wired.Bases;
using WibboEmulator.Games.Items.Wired.Interfaces;
using WibboEmulator.Games.Moderations;
using WibboEmulator.Games.Roleplays.Enemy;
using WibboEmulator.Games.Roleplays.Item;
using WibboEmulator.Games.Rooms;
using WibboEmulator.Games.Rooms.AI;
using WibboEmulator.Games.Rooms.Games.Teams;

public class SuperWired : WiredActionBase, IWired, IWiredEffect
{
    public SuperWired(Item item, Room room) : base(item, room, (int)WiredActionType.CHAT)
    {
    }

    public override void LoadItems(bool inDatabase = false)
    {
        base.LoadItems();

        if (inDatabase)
        {
            return;
        }

        this.CheckPermission();
    }

    private void CheckPermission()
    {
        string effet;
        if (this.StringParam.Contains(':'))
        {
            effet = this.StringParam.Split(':')[0];
        }
        else
        {
            effet = this.StringParam;
        }

        switch (effet.ToLower())
        {
            case "rpsendtimeuser":
            case "timespeed":
            case "cyclehoureffect":
            case "setenemy":
            case "enemyaggrostop":
            case "enemyaggrostart":
            case "addenemy":
            case "removeenemy":
            case "userpvp":
            case "pvp":
            case "addmunition":
            case "munition":
            case "rpresetuser":
            case "rpsay":
            case "rpsayme":
            case "rpexp":
            case "rpremoveexp":
            case "removemoney":
            case "addmoney":
            case "work":
            case "health":
            case "healthplus":
            case "hit":
            case "weaponfarid":
            case "weaponcacid":
            case "removeenergy":
            case "addenergy":
            case "removehygiene":
            case "addhygiene":
            case "allowitemsbuy":
            case "inventoryadd":
            case "inventoryremove":
            case "droprpitem":
            case "sendroomid":
            case "botchoose":
            case "botchoosenav":
            case "playsounduser":
            case "playsoundroom":
            case "playmusicroom":
            case "playmusicuser":
            case "stopsounduser":
            case "stopsoundroom":
            case "forcesound":
                if (this.RoomInstance.IsRoleplay)
                {
                    return;
                }
                break;
        }

        switch (effet)
        {
            case "botchoosenav":
            case "alert":
            case "send":
            case "enablestaff":
            case "teleportdisabled":
            case "roomingamechat":
            case "jackanddaisy":
            case "openpage":
            case "playsounduser":
            case "playsoundroom":
            case "playmusicroom":
            case "playmusicuser":
            case "stopsounduser":
            case "stopsoundroom":
            case "forcesound":
            case "badge":
            case "removebadge":
            case "roomalert":
            case "rpsay":
            case "rpsayme":
                if (this.IsStaff)
                {
                    return;
                }
                break;

            case "achievement":
            case "givelot":
            case "winmovierun":
                if (this.IsGod)
                {
                    return;
                }
                break;
        }

        switch (effet)
        {
            case "botchoose":
            case "roomstate":
            case "roomfreeze":
            case "roomkick":
            case "moveto":
            case "reversewalk":
            case "speedwalk":
            case "configbot":
            case "rot":
            case "roommute":
            case "enable":
            case "dance":
            case "sit":
            case "lay":
            case "handitem":
            case "setspeed":
            case "freeze":
            case "unfreeze":
            case "roomdiagonal":
            case "roomoblique":
            case "point":
            case "usertimer":
            case "addusertimer":
            case "removeusertimer":
            case "addpoint":
            case "removepoint":
            case "ingame":
            case "setitemmode":
            case "useitem":
            case "addpointteam":
            case "breakwalk":
            case "allowshoot":
            case "transf":
            case "transfstop":
            case "pushpull":
            case "stand":
            case "usermute":
                return;
        }

        this.StringParam = "";
    }

    public override bool OnCycle(RoomUser user, Item item)
    {
        if (string.IsNullOrEmpty(this.StringParam) || this.StringParam == ":")
        {
            return false;
        }

        string command;
        string value;
        if (this.StringParam.Contains(':'))
        {
            command = this.StringParam.Split(':')[0].ToLower();
            value = this.StringParam.Split(':')[1];
        }
        else
        {
            command = this.StringParam;
            value = string.Empty;
        }

        this.RpCommand(command, value, user);
        this.RoomCommand(command, value, user, item);
        this.UserCommand(command, value, user);
        this.BotCommand(command, value, user);

        return false;
    }


    private void RpCommand(string command, string value, RoomUser user)
    {
        if (!this.RoomInstance.IsRoleplay)
        {
            return;
        }

        if (user == null || user.Client == null)
        {
            return;
        }

        var rp = user.Roleplayer;
        if (rp == null)
        {
            return;
        }

        switch (command)
        {
            case "rpsendtimeuser":
            {
                user.SendWhisperChat("Il est " + this.RoomInstance.RoomRoleplay.Hour + " heures et " + this.RoomInstance.RoomRoleplay.Minute + " minutes");
                break;
            }
            case "setenemy":
            {
                var parameters = value.Split(';');
                if (parameters.Length != 3)
                {
                    break;
                }

                var botOrPet = this.RoomInstance.RoomUserManager.GetBotOrPetByName(parameters[0]);
                if (botOrPet == null || botOrPet.BotData == null || botOrPet.BotData.RoleBot == null)
                {
                    break;
                }

                RPEnemy rpEnemyConfig;
                if (!botOrPet.IsPet)
                {
                    rpEnemyConfig = WibboEnvironment.GetGame().GetRoleplayManager().EnemyManager.GetEnemyBot(botOrPet.BotData.Id);
                }
                else
                {
                    rpEnemyConfig = WibboEnvironment.GetGame().GetRoleplayManager().EnemyManager.GetEnemyPet(botOrPet.BotData.Id);
                }

                if (rpEnemyConfig == null)
                {
                    break;
                }

                switch (parameters[1])
                {
                    case "health":
                    {
                        if (!int.TryParse(parameters[2], out var paramInt))
                        {
                            break;
                        }

                        if (paramInt <= 0)
                        {
                            paramInt = 0;
                        }

                        if (paramInt > 9999)
                        {
                            paramInt = 9999;
                        }

                        rpEnemyConfig.Health = paramInt;
                        botOrPet.BotData.RoleBot.SetConfig(rpEnemyConfig);

                        using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
                        RoleplayEnemyDao.UpdateHealth(dbClient, rpEnemyConfig.Id, paramInt);

                        break;
                    }
                    case "weaponfarid":
                    {
                        if (!int.TryParse(parameters[2], out var paramInt))
                        {
                            break;
                        }

                        if (paramInt <= 0)
                        {
                            paramInt = 0;
                        }

                        if (paramInt > 9999)
                        {
                            paramInt = 9999;
                        }

                        rpEnemyConfig.WeaponGunId = paramInt;
                        botOrPet.BotData.RoleBot.SetConfig(rpEnemyConfig);

                        using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
                        RoleplayEnemyDao.UpdateWeaponFarId(dbClient, rpEnemyConfig.Id, paramInt);

                        break;
                    }
                    case "weaponcacid":
                    {
                        if (!int.TryParse(parameters[2], out var paramInt))
                        {
                            break;
                        }

                        if (paramInt <= 0)
                        {
                            paramInt = 0;
                        }

                        if (paramInt > 9999)
                        {
                            paramInt = 9999;
                        }

                        rpEnemyConfig.WeaponCacId = paramInt;
                        botOrPet.BotData.RoleBot.SetConfig(rpEnemyConfig);

                        using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
                        RoleplayEnemyDao.UpdateWeaponCacId(dbClient, rpEnemyConfig.Id, paramInt);

                        break;
                    }
                    case "deadtimer":
                    {
                        if (!int.TryParse(parameters[2], out var paramInt))
                        {
                            break;
                        }

                        if (paramInt <= 0)
                        {
                            paramInt = 0;
                        }

                        if (paramInt > 9999)
                        {
                            paramInt = 9999;
                        }

                        rpEnemyConfig.DeadTimer = paramInt;
                        botOrPet.BotData.RoleBot.SetConfig(rpEnemyConfig);

                        using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
                        RoleplayEnemyDao.UpdateDeadTimer(dbClient, rpEnemyConfig.Id, paramInt);

                        break;
                    }
                    case "lootitemid":
                    {
                        if (!int.TryParse(parameters[2], out var paramInt))
                        {
                            break;
                        }

                        if (paramInt <= 0)
                        {
                            paramInt = 0;
                        }

                        if (paramInt > 9999)
                        {
                            paramInt = 9999;
                        }

                        rpEnemyConfig.LootItemId = paramInt;
                        botOrPet.BotData.RoleBot.SetConfig(rpEnemyConfig);

                        using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
                        RoleplayEnemyDao.UpdateLootItemId(dbClient, rpEnemyConfig.Id, paramInt);

                        break;
                    }
                    case "moneydrop":
                    {
                        if (!int.TryParse(parameters[2], out var paramInt))
                        {
                            break;
                        }

                        if (paramInt <= 0)
                        {
                            paramInt = 0;
                        }

                        if (paramInt > 9999)
                        {
                            paramInt = 9999;
                        }

                        rpEnemyConfig.MoneyDrop = paramInt;
                        botOrPet.BotData.RoleBot.SetConfig(rpEnemyConfig);

                        using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
                        RoleplayEnemyDao.UpdateMoneyDrop(dbClient, rpEnemyConfig.Id, paramInt);

                        break;
                    }
                    case "teamid":
                    {
                        if (!int.TryParse(parameters[2], out var paramInt))
                        {
                            break;
                        }

                        if (paramInt <= 0)
                        {
                            paramInt = 0;
                        }

                        if (paramInt > 9999)
                        {
                            paramInt = 9999;
                        }

                        rpEnemyConfig.TeamId = paramInt;
                        botOrPet.BotData.RoleBot.SetConfig(rpEnemyConfig);

                        using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
                        RoleplayEnemyDao.UpdateTeamId(dbClient, rpEnemyConfig.Id, paramInt);

                        break;
                    }
                    case "aggrodistance":
                    {
                        if (!int.TryParse(parameters[2], out var paramInt))
                        {
                            break;
                        }

                        if (paramInt <= 0)
                        {
                            paramInt = 0;
                        }

                        if (paramInt > 9999)
                        {
                            paramInt = 9999;
                        }

                        rpEnemyConfig.AggroDistance = paramInt;
                        botOrPet.BotData.RoleBot.SetConfig(rpEnemyConfig);

                        using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
                        RoleplayEnemyDao.UpdateAggroDistance(dbClient, rpEnemyConfig.Id, paramInt);

                        break;
                    }
                    case "zonedistance":
                    {
                        if (!int.TryParse(parameters[2], out var paramInt))
                        {
                            break;
                        }

                        if (paramInt <= 0)
                        {
                            paramInt = 0;
                        }

                        if (paramInt > 9999)
                        {
                            paramInt = 9999;
                        }

                        rpEnemyConfig.ZoneDistance = paramInt;
                        botOrPet.BotData.RoleBot.SetConfig(rpEnemyConfig);

                        using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
                        RoleplayEnemyDao.UpdateZoneDistance(dbClient, rpEnemyConfig.Id, paramInt);

                        break;
                    }
                    case "resetposition":
                    {
                        rpEnemyConfig.ResetPosition = parameters[2] == "true";
                        botOrPet.BotData.RoleBot.SetConfig(rpEnemyConfig);

                        using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
                        RoleplayEnemyDao.UpdateResetPosition(dbClient, rpEnemyConfig.Id, rpEnemyConfig.ResetPosition);

                        break;
                    }
                    case "lostaggrodistance":
                    {
                        if (!int.TryParse(parameters[2], out var paramInt))
                        {
                            break;
                        }

                        if (paramInt <= 0)
                        {
                            paramInt = 0;
                        }

                        if (paramInt > 9999)
                        {
                            paramInt = 9999;
                        }

                        rpEnemyConfig.LostAggroDistance = paramInt;
                        botOrPet.BotData.RoleBot.SetConfig(rpEnemyConfig);

                        using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
                        RoleplayEnemyDao.UpdateLostAggroDistance(dbClient, rpEnemyConfig.Id, paramInt);

                        break;
                    }
                    case "zombiemode":
                    {
                        rpEnemyConfig.ZombieMode = parameters[2] == "true";
                        botOrPet.BotData.RoleBot.SetConfig(rpEnemyConfig);

                        using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
                        RoleplayEnemyDao.UpdateZombieMode(dbClient, rpEnemyConfig.Id, rpEnemyConfig.ZombieMode);

                        break;
                    }
                }
                break;
            }
            case "removeenemy":
            {
                var botOrPet = this.RoomInstance.RoomUserManager.GetBotOrPetByName(value);
                if (botOrPet == null || botOrPet.BotData == null || botOrPet.BotData.RoleBot == null)
                {
                    break;
                }

                if (!botOrPet.IsPet)
                {
                    WibboEnvironment.GetGame().GetRoleplayManager().EnemyManager.RemoveEnemyBot(botOrPet.BotData.Id);
                    botOrPet.BotData.RoleBot = null;
                    botOrPet.BotData.AiType = BotAIType.Generic;
                    _ = botOrPet.BotData.GenerateBotAI(botOrPet.VirtualId);
                }
                else
                {
                    WibboEnvironment.GetGame().GetRoleplayManager().EnemyManager.RemoveEnemyPet(botOrPet.BotData.Id);
                    botOrPet.BotData.RoleBot = null;
                    botOrPet.BotData.AiType = BotAIType.Pet;
                    _ = botOrPet.BotData.GenerateBotAI(botOrPet.VirtualId);
                }
                break;
            }
            case "addenemy":
            {
                var botOrPet = this.RoomInstance.RoomUserManager.GetBotOrPetByName(value);
                if (botOrPet == null || botOrPet.BotData == null || botOrPet.BotData.RoleBot != null)
                {
                    break;
                }

                if (!botOrPet.IsPet)
                {
                    var rpEnemyConfig = WibboEnvironment.GetGame().GetRoleplayManager().EnemyManager.AddEnemyBot(botOrPet.BotData.Id);
                    if (rpEnemyConfig != null)
                    {
                        botOrPet.BotData.RoleBot = new RoleBot(rpEnemyConfig);
                        botOrPet.BotData.AiType = BotAIType.RoleplayBot;
                        _ = botOrPet.BotData.GenerateBotAI(botOrPet.VirtualId);
                    }
                }
                else
                {
                    var rpEnemyConfig = WibboEnvironment.GetGame().GetRoleplayManager().EnemyManager.AddEnemyPet(botOrPet.BotData.Id);
                    if (rpEnemyConfig != null)
                    {
                        botOrPet.BotData.RoleBot = new RoleBot(rpEnemyConfig);
                        botOrPet.BotData.AiType = BotAIType.RoleplayPet;
                        _ = botOrPet.BotData.GenerateBotAI(botOrPet.VirtualId);
                    }
                }
                break;
            }
            case "enemyaggrostop":
            {
                var botOrPet = this.RoomInstance.RoomUserManager.GetBotOrPetByName(value);
                if (botOrPet == null || botOrPet.BotData == null || botOrPet.BotData.RoleBot == null)
                {
                    break;
                }

                botOrPet.BotData.RoleBot.ResetAggro();

                break;
            }
            case "enemyaggrostart":
            {
                var botOrPet = this.RoomInstance.RoomUserManager.GetBotOrPetByName(value);
                if (botOrPet == null || botOrPet.BotData == null || botOrPet.BotData.RoleBot == null)
                {
                    break;
                }

                botOrPet.BotData.RoleBot.AggroVirtuelId = user.VirtualId;
                botOrPet.BotData.RoleBot.AggroTimer = 0;

                break;
            }
            case "sendroomid":
            {
                if (int.TryParse(value, out var roomId))
                {
                    var roomDataTarget = WibboEnvironment.GetGame().GetRoomManager().GenerateRoomData(roomId);
                    if (roomDataTarget != null && roomDataTarget.OwnerId == this.RoomInstance.RoomData.OwnerId)
                    {
                        user.Client.User.IsTeleporting = true;
                        user.Client.User.TeleportingRoomID = roomId;
                        user.Client.SendPacket(new RoomForwardComposer(roomId));
                    }
                }
                break;
            }
            case "inventoryadd":
            {
                _ = int.TryParse(value, out var itemId);

                var rpItem = WibboEnvironment.GetGame().GetRoleplayManager().ItemManager.GetItem(itemId);
                if (rpItem == null)
                {
                    break;
                }

                rp.AddInventoryItem(rpItem.Id);
                break;
            }
            case "inventoryremove":
            {
                _ = int.TryParse(value, out var itemId);

                var rpItem = WibboEnvironment.GetGame().GetRoleplayManager().ItemManager.GetItem(itemId);
                if (rpItem == null)
                {
                    break;
                }

                rp.RemoveInventoryItem(rpItem.Id);
                break;
            }

            case "rpresetuser":
            {
                rp.Reset();

                break;
            }
            case "userpvp":
            {
                if (value == "true")
                {
                    rp.PvpEnable = true;
                }
                else
                {
                    rp.PvpEnable = false;
                }

                break;
            }
            case "allowitemsbuy":
            {
                var itemsList = new List<RPItem>();
                user.AllowBuyItems.Clear();

                if (string.IsNullOrEmpty(value))
                {
                    rp.SendItemsList(itemsList);
                    break;
                }

                if (value.Contains(','))
                {
                    foreach (var pId in value.Split(','))
                    {
                        if (!int.TryParse(pId, out var id))
                        {
                            continue;
                        }

                        var rpItem = WibboEnvironment.GetGame().GetRoleplayManager().ItemManager.GetItem(id);
                        if (rpItem == null)
                        {
                            continue;
                        }

                        itemsList.Add(rpItem);
                        user.AllowBuyItems.Add(id);
                    }
                }
                else
                {
                    if (!int.TryParse(value, out var id))
                    {
                        break;
                    }

                    var rpItem = WibboEnvironment.GetGame().GetRoleplayManager().ItemManager.GetItem(id);
                    if (rpItem == null)
                    {
                        break;
                    }

                    itemsList.Add(rpItem);
                    user.AllowBuyItems.Add(id);
                }

                rp.SendItemsList(itemsList);

                break;
            }
            case "removeenergy":
            {
                _ = int.TryParse(value, out var count);

                rp.RemoveEnergy(count);

                rp.SendUpdate();
                break;
            }
            case "addenergy":
            {
                _ = int.TryParse(value, out var count);

                rp.AddEnergy(count);

                rp.SendUpdate();
                break;
            }
            case "weaponfarid":
            {
                _ = int.TryParse(value, out var count);
                if (count is < 0 or > 2)
                {
                    count = 0;
                }

                rp.WeaponGun = WibboEnvironment.GetGame().GetRoleplayManager().WeaponManager.GetWeaponGun(count);

                break;
            }
            case "weaponcacid":
            {
                _ = int.TryParse(value, out var count);

                if (count is < 0 or > 3)
                {
                    count = 0;
                }

                rp.WeaponCac = WibboEnvironment.GetGame().GetRoleplayManager().WeaponManager.GetWeaponCac(count);
                break;
            }
            case "pvp":
            {
                if (value == "true")
                {
                    this.RoomInstance.RoomRoleplay.Pvp = true;
                }
                else
                {
                    this.RoomInstance.RoomRoleplay.Pvp = false;
                }

                break;
            }
            case "munition":
            {
                _ = int.TryParse(value, out var count);
                if (count > 99)
                {
                    count = 99;
                }

                if (count < 0)
                {
                    count = 0;
                }

                rp.Munition = count;

                rp.SendUpdate();
                break;
            }
            case "addmunition":
            {
                _ = int.TryParse(value, out var count);

                rp.AddMunition(count);
                rp.SendUpdate();
                break;
            }
            case "removemunition":
            {
                _ = int.TryParse(value, out var count);

                rp.RemoveMunition(count);
                rp.SendUpdate();
                break;
            }
            case "rpexp":
            {
                _ = int.TryParse(value, out var count);
                if (count <= 0)
                {
                    break;
                }

                rp.AddExp(count);
                break;
            }
            case "rpremoveexp":
            {
                _ = int.TryParse(value, out var count);
                if (count <= 0)
                {
                    break;
                }

                rp.RemoveExp(count);
                break;
            }
            case "removemoney":
            {
                _ = int.TryParse(value, out var count);
                if (count <= 0)
                {
                    break;
                }

                if (rp.Money - count < 0)
                {
                    rp.Money = 0;
                }
                else
                {
                    rp.Money -= count;
                }
                rp.SendUpdate();
                break;
            }
            case "addmoney":
            {
                _ = int.TryParse(value, out var count);
                if (count <= 0)
                {
                    break;
                }

                rp.Money += count;
                rp.SendUpdate();
                break;
            }
            case "health":
            {
                _ = int.TryParse(value, out var count);
                if (count <= 0)
                {
                    break;
                }

                if (count > rp.HealthMax)
                {
                    rp.Health = rp.HealthMax;
                }
                else
                {
                    rp.Health = count;
                }

                rp.SendUpdate();
                break;
            }
            case "healthplus":
            {
                _ = int.TryParse(value, out var count);
                if (count <= 0)
                {
                    break;
                }

                rp.AddHealth(count);

                rp.SendUpdate();
                break;
            }
            case "hit":
            {
                _ = int.TryParse(value, out var nb);
                if (nb <= 0)
                {
                    break;
                }

                rp.Hit(user, nb, this.RoomInstance, false, true);
                rp.SendUpdate();
                break;
            }
            case "rpsay":
            {
                user.OnChat(value, 0, false);
                break;
            }
            case "rpsayme":
            {
                user.OnChatMe(value, 0, false);
                break;
            }
            case "droprpitem":
            {
                _ = int.TryParse(value, out var valueNumber);
                if (valueNumber <= 0)
                {
                    break;
                }

                _ = this.RoomInstance.RoomItemHandling.AddTempItem(user.VirtualId, valueNumber, user.SetX, user.SetY, user.Z, "1", 0, InteractionTypeTemp.RpItem);
                break;
            }
        }
    }

    private void BotCommand(string command, string value, RoomUser user)
    {
        if (user == null || !user.IsBot)
        {
            return;
        }

        switch (command)
        {
            case "dance":
            {
                if (int.TryParse(value, out var danceId))
                {
                    if (danceId is < 0 or > 4)
                    {
                        danceId = 0;
                    }

                    if (danceId > 0 && user.CarryItemID > 0)
                    {
                        user.CarryItem(0);
                    }

                    user.DanceId = danceId;
                    this.RoomInstance.SendPacket(new DanceComposer(user.VirtualId, danceId));
                }

                break;
            }

            case "handitem":
            {
                if (int.TryParse(value, out var carryid))
                {
                    user.CarryItem(carryid, true);
                }

                break;
            }
            case "sit":
            {
                if (user.RotBody % 2 == 0)
                {
                    user.SetStatus("sit", "0.5");

                    user.IsSit = true;
                    user.UpdateNeeded = true;
                }
                break;
            }

            case "lay":
            {
                if (user.RotBody % 2 == 0)
                {
                    user.SetStatus("lay", "0.7");

                    user.IsLay = true;
                    user.UpdateNeeded = true;
                }
                break;
            }

            case "stand":
            {
                if (user.ContainStatus("lay"))
                {
                    user.RemoveStatus("lay");
                }

                if (user.ContainStatus("sit"))
                {
                    user.RemoveStatus("sit");
                }

                if (user.ContainStatus("sign"))
                {
                    user.RemoveStatus("sign");
                }

                user.UpdateNeeded = true;
                break;
            }

            case "enable":
            {
                if (!int.TryParse(value, out var numEnable))
                {
                    return;
                }

                if (!WibboEnvironment.GetGame().GetEffectManager().HaveEffect(numEnable, false))
                {
                    return;
                }

                user.ApplyEffect(numEnable);
                break;
            }

            case "breakwalk":
            {
                if (value == "true")
                {
                    user.BreakWalkEnable = true;
                }
                else
                {
                    user.BreakWalkEnable = false;
                }

                break;
            }

            case "freeze":
            {
                _ = int.TryParse(value, out var seconde);
                seconde *= 2;
                user.Freeze = true;
                user.FreezeEndCounter = seconde;
                break;
            }
            case "unfreeze":
            {
                user.Freeze = false;
                user.FreezeEndCounter = 0;
                break;
            }
        }
    }

    private void RoomCommand(string command, string value, RoomUser user, Item item)
    {
        switch (command)
        {
            case "roomfreeze":
            {
                this.RoomInstance.FreezeRoom = value == "true";
                break;
            }
            case "roomkick":
            {
                foreach (var rUser in this.RoomInstance.RoomUserManager.GetUserList().ToList())
                {
                    if (rUser != null && rUser.Client != null && !rUser.Client.User.HasPermission("no_kick") && this.RoomInstance.RoomData.OwnerId != rUser.UserId)
                    {
                        this.RoomInstance.RoomUserManager.RemoveUserFromRoom(rUser.Client, true, false);
                    }
                }
                break;
            }
            case "roomalert":
            {
                if (value.Length <= 0)
                {
                    break;
                }

                foreach (var rUser in this.RoomInstance.RoomUserManager.GetUserList().ToList())
                {
                    if (rUser != null && !rUser.IsBot)
                    {
                        rUser.Client?.SendNotification(value);
                    }
                }
                break;
            }
            case "stopsoundroom":
            {
                this.RoomInstance.SendPacket(new StopSoundComposer(value));
                break;
            }
            case "playsoundroom":
            {
                this.RoomInstance.SendPacket(new PlaySoundComposer(value, 1)); //Type = Trax
                break;
            }
            case "playmusicroom":
            {
                this.RoomInstance.SendPacket(new PlaySoundComposer(value, 2, true)); //Type = Trax
                break;
            }
            case "configbot":
            {
                var parameters = value.Split(';');

                if (parameters.Length < 3)
                {
                    break;
                }

                var bot = this.RoomInstance.RoomUserManager.GetBotByName(parameters[0]);
                if (bot == null)
                {
                    return;
                }

                switch (parameters[1])
                {
                    case "enable":
                    {
                        if (parameters.Length < 3)
                        {
                            break;
                        }

                        _ = int.TryParse(parameters[2], out var intValue);

                        if (!WibboEnvironment.GetGame().GetEffectManager().HaveEffect(intValue, false))
                        {
                            return;
                        }

                        if (bot.CurrentEffect != intValue)
                        {
                            bot.ApplyEffect(intValue);
                        }

                        if (bot.BotData.Enable != intValue)
                        {
                            bot.BotData.Enable = intValue;
                        }

                        break;
                    }
                    case "handitem":
                    {
                        if (parameters.Length < 3)
                        {
                            break;
                        }

                        _ = int.TryParse(parameters[2], out var intValue);

                        if (bot.CarryItemID != intValue)
                        {
                            bot.CarryItem(intValue, true);
                        }

                        if (bot.BotData.Handitem != intValue)
                        {
                            bot.BotData.Handitem = intValue;
                        }

                        break;
                    }
                    case "rot":
                    {
                        if (parameters.Length < 3)
                        {
                            break;
                        }

                        _ = int.TryParse(parameters[2], out var intValue);
                        intValue = (intValue is > 7 or < 0) ? 0 : intValue;

                        if (bot.RotBody != intValue)
                        {
                            bot.RotBody = intValue;
                            bot.RotHead = intValue;
                            bot.UpdateNeeded = true;
                        }

                        if (bot.BotData.Rot != intValue)
                        {
                            bot.BotData.Rot = intValue;
                        }

                        break;
                    }
                    case "sit":
                    {
                        if (bot.BotData.Status == 1)
                        {
                            bot.BotData.Status = 0;

                            bot.RemoveStatus("sit");
                            bot.IsSit = false;
                            bot.UpdateNeeded = true;
                        }
                        else
                        {
                            if (!bot.IsSit)
                            {
                                bot.SetStatus("sit", bot.IsPet ? "" : "0.5");
                                bot.IsSit = true;
                                bot.UpdateNeeded = true;
                            }

                            bot.BotData.Status = 1;
                        }

                        break;
                    }
                    case "lay":
                    {
                        if (bot.BotData.Status == 2)
                        {
                            bot.BotData.Status = 0;

                            bot.RemoveStatus("lay");
                            bot.IsSit = false;
                            bot.UpdateNeeded = true;
                        }
                        else
                        {
                            if (!bot.IsLay)
                            {
                                bot.SetStatus("lay", bot.IsPet ? "" : "0.7");
                                bot.IsLay = true;
                                bot.UpdateNeeded = true;
                            }

                            bot.BotData.Status = 2;
                        }

                        break;
                    }
                }
                break;
            }
            case "timespeed":
            {
                if (!this.RoomInstance.IsRoleplay)
                {
                    break;
                }

                if (value == "true")
                {
                    this.RoomInstance.RoomRoleplay.TimeSpeed = true;
                }
                else
                {
                    this.RoomInstance.RoomRoleplay.TimeSpeed = false;
                }

                break;
            }
            case "cyclehoureffect":
            {
                if (!this.RoomInstance.IsRoleplay)
                {
                    break;
                }

                if (value == "true")
                {
                    this.RoomInstance.RoomRoleplay.CycleHourEffect = true;
                }
                else
                {
                    this.RoomInstance.RoomRoleplay.CycleHourEffect = false;
                }

                break;
            }

            case "jackanddaisy":
            {
                var botName = "Daisy";
                if (WibboEnvironment.GetRandomNumber(0, 1) == 1)
                {
                    botName = "Jack";
                }

                var bot = this.RoomInstance.RoomUserManager.GetBotOrPetByName(botName);

                if (bot == null)
                {
                    break;
                }

                var phrases = new List<string>();

                switch (value)
                {
                    case "wait":
                    {
                        phrases.Add("Merci de patienter, le jeu va bientôt commencer !");
                        phrases.Add("Le jeu va commencer dans quelques instants !");
                        phrases.Add("Patience, le jeu débutera sous peu !");
                        phrases.Add("Silence dans la salle, le jeu va débuter !");
                        break;
                    }
                    case "win":
                    {
                        if (bot.BotData.Name == "Jack")
                        {
                            phrases.Add("Fichtre... #username# a gagné !");
                            phrases.Add("Et c'est ce moussaillon de #username# qui repart avec le trésor !");
                            phrases.Add("#username# vient de décrocher une très belle surprise !");
                        }
                        else
                        {
                            phrases.Add("Félicitation à #username# qui remporte la partie !");
                            phrases.Add("Félicitons #username# qui remporte la partie !");
                            phrases.Add("La chance était du côté de #username# aujourd'hui");
                            phrases.Add("#username# est divin!");
                            phrases.Add("#username# est légendaire !");
                        }
                        break;
                    }
                    case "loose":
                    {
                        if (bot.BotData.Name == "Jack")
                        {
                            phrases.Add("Oulà ! #username# vient de se faire botter l'arrière train' !");
                            phrases.Add("#username# rejoint l'équipe des loosers");
                            phrases.Add("Une défaite en bonne et due forme de #username# !");
                        }
                        else
                        {
                            phrases.Add("La prochaine fois tu y arriveras #username#, j'en suis sûre et certain !");
                            phrases.Add("Courage #username#, tu y arriveras la prochaine fois !");
                            phrases.Add("Ne soit pas triste #username#, d'autres occasions se présenteront à toi !");
                        }
                        break;
                    }
                    case "startgame":
                    {
                        phrases.Add("Allons y !");
                        phrases.Add("C'est parti !");
                        phrases.Add("A vos marques, prêts ? Partez !");
                        phrases.Add("Let's go!");
                        phrases.Add("Ne perdons pas plus de temps !");
                        phrases.Add("Que la partie commence !");
                        break;
                    }
                    case "endgame":
                    {
                        phrases.Add("L'animation est terminée, bravo aux gagnants !");
                        phrases.Add("L'animation est enfin terminée ! Reviens nous voir à la prochaine animation !");
                        break;
                    }
                    case "fungame":
                    {
                        if (bot.BotData.Name == "Jack")
                        {
                            phrases.Add("Mhhhh, le niveau n'est pas très haut...");
                            phrases.Add("On sait déjà tous qui sera le grand vaiqueur...");
                            phrases.Add("Qui ne tente rien, n'a rien");
                        }
                        else
                        {
                            phrases.Add("La victoire approche, tenez le coup !");
                            phrases.Add("C'est pour ça qu'il faut toujours avoir un trèfle à 4 feuilles sur soi");
                            phrases.Add("En essayant continuellement, on finit par réussir, plus ça rate, plus on a des chances que ça marque ;)");
                        }
                        break;
                    }
                }

                var textMessage = phrases[WibboEnvironment.GetRandomNumber(0, phrases.Count - 1)];
                if (user != null)
                {
                    textMessage = textMessage.Replace("#username#", user.GetUsername());
                }

                bot.OnChat(textMessage, 2, true);

                break;
            }
            case "roomingamechat":
            {
                if (value == "true")
                {
                    this.RoomInstance.IngameChat = true;
                }
                else
                {
                    this.RoomInstance.IngameChat = false;
                }

                break;
            }
            case "roomstate":
            {
                if (value == "close")
                {
                    this.RoomInstance.RoomData.Access = RoomAccess.Doorbell;
                }
                else
                {
                    this.RoomInstance.RoomData.Access = RoomAccess.Open;
                }

                break;
            }
            case "roommute":
            {
                if (value == "true")
                {
                    this.RoomInstance.RoomMuted = true;
                }
                else
                {
                    this.RoomInstance.RoomMuted = false;
                }

                break;
            }
            case "setspeed":
            {
                if (!int.TryParse(value, out var speed))
                {
                    break;
                }

                this.RoomInstance.RoomItemHandling.SetSpeed(speed);
                break;
            }
            case "roomdiagonal":
            {
                if (value == "true")
                {
                    this.RoomInstance.GameMap.DiagonalEnabled = true;
                }
                else
                {
                    this.RoomInstance.GameMap.DiagonalEnabled = false;
                }

                break;
            }
            case "roomoblique":
            {
                if (value == "true")
                {
                    this.RoomInstance.GameMap.ObliqueDisable = true;
                }
                else
                {
                    this.RoomInstance.GameMap.ObliqueDisable = false;
                }

                break;
            }

            case "setitemmode":
            {
                if (item == null)
                {
                    break;
                }

                if (!int.TryParse(value, out var count))
                {
                    break;
                }

                if (count > item.GetBaseItem().Modes - 1)
                {
                    break;
                }

                if (!int.TryParse(item.ExtraData, out _))
                {
                    break;
                }

                item.ExtraData = count.ToString();
                item.UpdateState();
                this.RoomInstance.GameMap.UpdateMapForItem(item);

                break;
            }

            case "useitem":
            {
                if (item == null)
                {
                    break;
                }

                if (item.GetBaseItem().Modes == 0)
                {
                    break;
                }

                if (!int.TryParse(value, out var count))
                {
                    break;
                }

                if (!int.TryParse(item.ExtraData, out var itemCount))
                {
                    break;
                }

                var newCount = (itemCount + count < item.GetBaseItem().Modes) ? itemCount + count : 0;

                item.ExtraData = newCount.ToString();
                item.UpdateState();
                this.RoomInstance.GameMap.UpdateMapForItem(item);

                break;
            }


            case "pushpull":
            {
                if (value == "true")
                {
                    this.RoomInstance.PushPullAllowed = true;
                }
                else
                {
                    this.RoomInstance.PushPullAllowed = false;
                }

                break;
            }
        }
    }

    private void UserCommand(string cmd, string value, RoomUser user)
    {
        if (user == null || user.IsBot || user.Client == null)
        {
            return;
        }

        switch (cmd)
        {
            case "usermute":
            {
                if (value == "true")
                {
                    this.RoomInstance.AddMute(user.UserId, 24 * 60 * 60);
                }
                else
                {
                    this.RoomInstance.RemoveMute(user.UserId);
                }

                break;
            }
            case "botchoosenav":
            {
                var chooseList = new List<string[]>();

                if (string.IsNullOrEmpty(value))
                {
                    user.Client.SendPacket(new BotChooseComposer(chooseList));
                    break;
                }

                foreach (var roomIdString in value.Split(','))
                {
                    if (!int.TryParse(roomIdString, out var roomId))
                    {
                        continue;
                    }

                    var roomData = WibboEnvironment.GetGame().GetRoomManager().GenerateRoomData(roomId);

                    if (roomData == null)
                    {
                        continue;
                    }

                    var list = new List<string>
                        {
                            "", //Username
                            "sendroom_" + roomId, //Code
                            $"{roomData.Name} ({roomData.UsersNow}/{roomData.UsersMax})", //Message
                            "" //Look
                        };

                    chooseList.Add(list.ToArray());
                }

                user.Client.SendPacket(new BotChooseComposer(chooseList));

                break;
            }
            case "botchoose":
            {
                var chooseList = new List<string[]>();
                if (string.IsNullOrEmpty(value))
                {
                    user.Client.SendPacket(new BotChooseComposer(chooseList));
                    break;
                }

                if (value.Contains(','))
                {
                    foreach (var pChoose in value.Split(','))
                    {
                        var list = pChoose.Split(';').ToList();
                        if (list.Count == 3)
                        {
                            var botOrPet = this.RoomInstance.RoomUserManager.GetBotByName(list[0]);
                            if (botOrPet != null && botOrPet.BotData != null)
                            {
                                list.Add(botOrPet.BotData.Look);
                            }
                            else
                            {
                                list.Add("");
                            }

                            chooseList.Add(list.ToArray());
                        }
                    }
                }
                else
                {
                    var list = value.Split(';').ToList();
                    if (list.Count == 3)
                    {
                        var botOrPet = this.RoomInstance.RoomUserManager.GetBotByName(list[0]);
                        if (botOrPet != null && botOrPet.BotData != null)
                        {
                            list.Add(botOrPet.BotData.Look);
                        }
                        else
                        {
                            list.Add("");
                        }

                        chooseList.Add(list.ToArray());
                    }
                }

                user.Client.SendPacket(new BotChooseComposer(chooseList));

                break;
            }
            case "stopsounduser":
            {
                user.Client.SendPacket(new StopSoundComposer(value)); //Type = Trax

                break;
            }
            case "playsounduser":
            {
                user.Client.SendPacket(new PlaySoundComposer(value, 1)); //Type = furni

                break;
            }
            case "playmusicuser":
            {
                user.Client.SendPacket(new PlaySoundComposer(value, 2, true)); //Type = Trax

                break;
            }
            case "moveto":
            {
                if (value == "true")
                {
                    user.AllowMoveTo = true;
                }
                else
                {
                    user.AllowMoveTo = false;
                }

                break;
            }
            case "reversewalk":
            {
                if (value == "true")
                {
                    user.ReverseWalk = true;
                }
                else
                {
                    user.ReverseWalk = false;
                }

                break;
            }
            case "speedwalk":
            {
                if (value == "true")
                {
                    user.WalkSpeed = true;
                }
                else
                {
                    user.WalkSpeed = false;
                }

                break;
            }
            case "openpage":
            {
                user.Client.SendPacket(new InClientLinkComposer("habbopages/" + value));
                break;
            }
            case "rot":
            {
                _ = int.TryParse(value, out var valueInt);

                if (valueInt is > 7 or < 0)
                {
                    valueInt = 0;
                }

                if (user.RotBody == valueInt && user.RotHead == valueInt)
                {
                    break;
                }

                user.RotBody = valueInt;
                user.RotHead = valueInt;
                user.UpdateNeeded = true;

                break;
            }
            case "stand":
            {
                if (user.ContainStatus("lay"))
                {
                    user.RemoveStatus("lay");
                }

                if (user.ContainStatus("sit"))
                {
                    user.RemoveStatus("sit");
                }

                if (user.ContainStatus("sign"))
                {
                    user.RemoveStatus("sign");
                }

                user.UpdateNeeded = true;
                break;
            }
            case "allowshoot":
            {
                if (value == "true")
                {
                    user.AllowShoot = true;
                }
                else
                {
                    user.AllowShoot = false;
                }

                break;
            }
            case "addpointteam":
            {
                if (user.Team == TeamType.None)
                {
                    break;
                }

                _ = int.TryParse(value, out var count);

                if (this.RoomInstance == null)
                {
                    break;
                }

                this.RoomInstance.
                GameManager.AddPointToTeam(user.Team, count, user);
                break;
            }
            case "ingame":
            {
                if (value == "true")
                {
                    user.InGame = true;
                }
                else
                {
                    user.InGame = false;
                }

                break;
            }
            case "usertimer":
            {
                _ = int.TryParse(value, out var points);

                user.UserTimer = points;

                break;
            }
            case "addusertimer":
            {
                _ = int.TryParse(value, out var points);

                if (points == 0)
                {
                    break;
                }

                if (user.UserTimer + points <= int.MaxValue)
                {
                    user.UserTimer += points;
                }

                break;
            }
            case "removeusertimer":
            {
                _ = int.TryParse(value, out var points);

                if (points == 0)
                {
                    break;
                }

                if (points >= user.UserTimer)
                {
                    user.UserTimer = 0;
                }
                else
                {
                    user.UserTimer -= points;
                }

                break;
            }
            case "point":
            {
                _ = int.TryParse(value, out var points);

                user.WiredPoints = points;

                break;
            }
            case "addpoint":
            {
                _ = int.TryParse(value, out var points);

                if (points == 0)
                {
                    break;
                }

                if (user.WiredPoints + points <= int.MaxValue)
                {
                    user.WiredPoints += points;
                }

                break;
            }
            case "removepoint":
            {
                _ = int.TryParse(value, out var points);

                if (points == 0)
                {
                    break;
                }

                if (points >= user.WiredPoints)
                {
                    user.WiredPoints = 0;
                }
                else
                {
                    user.WiredPoints -= points;
                }
                break;
            }
            case "freeze":
            {
                _ = int.TryParse(value, out var seconde);
                seconde *= 2;
                user.Freeze = true;
                user.FreezeEndCounter = seconde;
                break;
            }
            case "unfreeze":
            {
                user.Freeze = false;
                user.FreezeEndCounter = 0;
                break;
            }
            case "breakwalk":
            {
                if (value == "true")
                {
                    user.BreakWalkEnable = true;
                }
                else
                {
                    user.BreakWalkEnable = false;
                }

                break;
            }
            case "enable":
            {
                if (!int.TryParse(value, out var numEnable))
                {
                    return;
                }

                if (!WibboEnvironment.GetGame().GetEffectManager().HaveEffect(numEnable, false))
                {
                    return;
                }

                user.ApplyEffect(numEnable);
                break;
            }
            case "enablestaff":
            {
                if (!int.TryParse(value, out var numEnable))
                {
                    return;
                }

                if (!WibboEnvironment.GetGame().GetEffectManager().HaveEffect(numEnable, true))
                {
                    return;
                }

                user.ApplyEffect(numEnable);
                break;
            }
            case "dance":
            {
                if (this.RoomInstance == null)
                {
                    break;
                }

                if (int.TryParse(value, out var danceId))
                {
                    if (danceId is < 0 or > 4)
                    {
                        danceId = 0;
                    }

                    if (danceId > 0 && user.CarryItemID > 0)
                    {
                        user.CarryItem(0);
                    }

                    user.DanceId = danceId;

                    this.RoomInstance.SendPacket(new DanceComposer(user.VirtualId, danceId));
                }
                break;
            }
            case "handitem":
            {
                if (int.TryParse(value, out var carryid))
                {
                    user.CarryItem(carryid, true);
                }

                break;
            }
            case "sit":
            {
                if (user.RotBody % 2 == 0)
                {
                    if (user.IsTransf)
                    {
                        user.SetStatus("sit", "0");
                    }
                    else
                    {
                        user.SetStatus("sit", "0.5");
                    }

                    user.IsSit = true;
                    user.UpdateNeeded = true;
                }
                break;
            }

            case "lay":
            {
                if (user.RotBody % 2 == 0)
                {
                    if (user.IsTransf)
                    {
                        user.SetStatus("lay", "0");
                    }
                    else
                    {
                        user.SetStatus("lay", "0.7");
                    }

                    user.IsLay = true;
                    user.UpdateNeeded = true;
                }
                break;
            }
            case "transf":
            {
                var raceId = 0;
                var petName = value;
                if (value.Contains(' '))
                {
                    if (int.TryParse(value.Split(' ')[1], out raceId))
                    {
                        if (raceId is < 1 or > 50)
                        {
                            raceId = 0;
                        }
                    }

                    petName = value.Split(' ')[0];
                }

                if (user.SetPetTransformation(petName, raceId))
                {
                    user.IsTransf = true;

                    this.RoomInstance.SendPacket(new UserRemoveComposer(user.VirtualId));
                    this.RoomInstance.SendPacket(new UsersComposer(user));
                }
                break;
            }
            case "transfstop":
            {
                user.IsTransf = false;

                this.RoomInstance.SendPacket(new UserRemoveComposer(user.VirtualId));
                this.RoomInstance.SendPacket(new UsersComposer(user));
                break;
            }
            case "badge":
            {
                user.Client.User.BadgeComponent.GiveBadge(value, true);
                user.Client.SendPacket(new ReceiveBadgeComposer(value));
                break;
            }
            case "removebadge":
            {
                user.Client.User.BadgeComponent.RemoveBadge(value);
                user.Client.SendPacket(new BadgesComposer(user.Client.User.BadgeComponent.BadgeList));
                break;
            }

            case "send":
            {
                if (int.TryParse(value, out var roomId))
                {
                    user.Client.User.IsTeleporting = true;
                    user.Client.User.TeleportingRoomID = roomId;
                    user.Client.SendPacket(new RoomForwardComposer(roomId));
                }
                break;
            }
            case "alert":
            {
                user.Client.SendNotification(value);
                break;
            }
            case "achievement":
            {
                _ = WibboEnvironment.GetGame().GetAchievementManager().ProgressAchievement(user.Client, value, 1);
                break;
            }
            case "winmovierun":
            {
                if (user.IsBot || user.Client == null || user.Client.User == null || user.Client.User.Rank > 4)
                {
                    break;
                }


                if (user.GetUsername() == this.RoomInstance.RoomData.OwnerName)
                {
                    break;
                }

                using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
                UserDao.UpdateAddRunPoints(dbClient, user.Client.User.Id);

                break;
            }
            case "givelot":
            {
                if (user.IsBot || user.Client == null || user.Client.User == null || user.Client.User.Rank > 4)
                {
                    break;
                }

                if (user.GetUsername() == this.RoomInstance.RoomData.OwnerName)
                {
                    break;
                }

                var allowedOwner = WibboEnvironment.GetSettings().GetData<string>("givelot.allowed.owner").Split(',');

                if (!allowedOwner.Contains(this.RoomInstance.RoomData.OwnerName))
                {
                    break;
                }

                if (user.WiredGivelot)
                {
                    break;
                }

                user.WiredGivelot = true;

                var lootboxId = WibboEnvironment.GetSettings().GetData<int>("givelot.lootbox.id");

                if (!WibboEnvironment.GetGame().GetItemManager().GetItem(lootboxId, out var itemData))
                {
                    break;
                }

                var nbLot = WibboEnvironment.GetRandomNumber(1, 2);

                if (user.Client.User.Rank > 1)
                {
                    nbLot = WibboEnvironment.GetRandomNumber(2, 3);
                }

                var items = ItemFactory.CreateMultipleItems(itemData, user.Client.User, "", nbLot);

                foreach (var purchasedItem in items)
                {
                    if (user.Client.User.InventoryComponent.TryAddItem(purchasedItem))
                    {
                        user.Client.SendPacket(new FurniListNotificationComposer(purchasedItem.Id, 1));
                    }
                }

                user.Client.SendNotification(string.Format(WibboEnvironment.GetLanguageManager().TryGetValue("notif.givelot.sucess", user.Client.Langue), nbLot));

                using (var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    UserDao.UpdateAddGamePoints(dbClient, user.Client.User.Id);
                }

                _ = WibboEnvironment.GetGame().GetAchievementManager().ProgressAchievement(user.Client, "ACH_Extrabox", 1);
                ModerationManager.LogStaffEntry(1953042, this.RoomInstance.RoomData.OwnerName, user.RoomId, string.Empty, "givelot", "SuperWired givelot: " + user.GetUsername());

                break;
            }
        }
    }

    public void SaveToDatabase(IQueryAdapter dbClient) => WiredUtillity.SaveTriggerItem(dbClient, this.Id, string.Empty, this.StringParam, false, null, this.Delay);

    public void LoadFromDatabase(DataRow row)
    {
        if (int.TryParse(row["delay"].ToString(), out var delay))
        {
            this.Delay = delay;
        }

        if (int.TryParse(row["trigger_data_2"].ToString(), out delay))
        {
            this.Delay = delay;
        }

        this.StringParam = row["trigger_data"].ToString();

    }
}
