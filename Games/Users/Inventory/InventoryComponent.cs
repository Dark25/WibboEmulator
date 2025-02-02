namespace WibboEmulator.Games.Users.Inventory;
using System.Collections.Concurrent;
using System.Data;
using WibboEmulator.Communication.Packets.Outgoing.Inventory.Achievements;
using WibboEmulator.Communication.Packets.Outgoing.Inventory.Furni;
using WibboEmulator.Communication.Packets.Outgoing.Inventory.Purse;
using WibboEmulator.Communication.Packets.Outgoing.Rooms.Engine;
using WibboEmulator.Database.Daos.Bot;
using WibboEmulator.Database.Daos.Item;
using WibboEmulator.Database.Daos.User;
using WibboEmulator.Database.Interfaces;
using WibboEmulator.Games.Catalogs.Utilities;
using WibboEmulator.Games.Items;
using WibboEmulator.Games.Rooms.AI;
using WibboEmulator.Games.Users.Inventory.Bots;
using WibboEmulator.Utilities;

public class InventoryComponent : IDisposable
{
    private readonly ConcurrentDictionary<int, Item> _userItems;
    private readonly ConcurrentDictionary<int, Pet> _petItems;
    private readonly ConcurrentDictionary<int, Bot> _botItems;
    private readonly int _furniLimit;
    private readonly int _petLimit;
    private readonly int _botLimit;
    private readonly User _userInstance;
    private int _inventoryPoints;
    private bool _inventoryDefined;

    public InventoryComponent(User user)
    {
        this._userInstance = user;

        this._userItems = new ConcurrentDictionary<int, Item>();
        this._petItems = new ConcurrentDictionary<int, Pet>();
        this._botItems = new ConcurrentDictionary<int, Bot>();

        this._furniLimit = WibboEnvironment.GetSettings().GetData<int>("inventory.furni.limit");
        this._petLimit = WibboEnvironment.GetSettings().GetData<int>("inventory.pet.limit");
        this._botLimit = WibboEnvironment.GetSettings().GetData<int>("inventory.bot.limit");
    }

    public void ClearItems(bool all = false)
    {
        if (all)
        {
            using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();

            ItemDao.DeleteAll(dbClient, this._userInstance.Id);

            var rareAmounts = new Dictionary<int, int>();
            foreach (var roomItem in this.GetWallAndFloor)
            {
                if (roomItem == null || roomItem.GetBaseItem() == null || roomItem.GetBaseItem().Amount < 0)
                {
                    continue;
                }

                roomItem.Data.Amount -= 1;

                if (!rareAmounts.TryGetValue(roomItem.BaseItem, out var value))
                {
                    rareAmounts.Add(roomItem.BaseItem, 1);
                }
                else
                {
                    _ = rareAmounts.Remove(roomItem.BaseItem);
                    rareAmounts.Add(roomItem.BaseItem, value + 1);
                }
            }

            ItemStatDao.UpdateRemove(dbClient, rareAmounts);

            this._userItems.Clear();
            this._inventoryPoints = 0;
        }
        else
        {
            using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();

            ItemDao.DeleteAllWithoutRare(dbClient, this._userInstance.Id);

            this._userItems.Clear();

            var table = ItemDao.GetAllByUserId(dbClient, this._userInstance.Id, this._furniLimit);

            foreach (DataRow dataRow in table.Rows)
            {
                var id = Convert.ToInt32(dataRow["id"]);
                var baseItem = Convert.ToInt32(dataRow["base_item"]);
                var extraData = DBNull.Value.Equals(dataRow["extra_data"]) ? string.Empty : (string)dataRow["extra_data"];
                var limited = DBNull.Value.Equals(dataRow["limited_number"]) ? 0 : Convert.ToInt32(dataRow["limited_number"]);
                var limitedTo = DBNull.Value.Equals(dataRow["limited_stack"]) ? 0 : Convert.ToInt32(dataRow["limited_stack"]);

                var userItem = new Item(id, 0, baseItem, extraData, limited, limitedTo, 0, 0, 0.0, 0, "", null);
                _ = this._userItems.TryAdd(id, userItem);
            }
        }

        this._userInstance.Client.SendPacket(new FurniListUpdateComposer());
    }

    public List<Item> GetItemsByType(int baseItem, int amount = 10000) => this.GetWallAndFloor.Where(x => x.BaseItem == baseItem).Take(amount).ToList();

    public int GetInventoryPoints() => this._inventoryPoints;

    public void DeleteItems(IQueryAdapter dbClient, List<Item> items, int baseItem = 0)
    {
        var listMessage = new ServerPacketList();
        var deleteItem = new List<Item>();
        var rareAmounts = new Dictionary<int, int>();

        foreach (var roomItem in items)
        {
            if (roomItem == null || roomItem.GetBaseItem() == null)
            {
                continue;
            }

            if (roomItem.GetBaseItem().Amount > 0)
            {
                roomItem.Data.Amount -= 1;

                if (!rareAmounts.TryGetValue(roomItem.BaseItem, out var value))
                {
                    rareAmounts.Add(roomItem.BaseItem, 1);
                }
                else
                {
                    _ = rareAmounts.Remove(roomItem.BaseItem);
                    rareAmounts.Add(roomItem.BaseItem, value + 1);
                }
            }

            listMessage.Add(new FurniListRemoveComposer(roomItem.Id));
            deleteItem.Add(roomItem);
        }

        foreach (var roomItem in deleteItem)
        {
            this.RemoveItem(roomItem.Id, false);
        }

        if (baseItem > 0)
        {
            ItemDao.DeleteAllByRoomIdAndBaseItem(dbClient, 0, this._userInstance.Id, baseItem);
        }
        else
        {
            ItemDao.DeleteItems(dbClient, deleteItem);
        }

        ItemStatDao.UpdateRemove(dbClient, rareAmounts);

        this._userInstance.Client.SendPacket(listMessage);
    }

    public void ConvertMagot()
    {
        var wibboPointsCount = 0;
        var creditCount = 0;
        var winwinCount = 0;

        var deleteItem = new List<Item>();

        foreach (var roomItem in this.GetWallAndFloor)
        {
            if (roomItem == null || roomItem.GetBaseItem() == null || roomItem.GetBaseItem().InteractionType != InteractionType.EXCHANGE)
            {
                continue;
            }

            var magotCount = int.Parse(roomItem.GetBaseItem().ItemName.Split(new char[1] { '_' })[1]);

            if (magotCount > 0)
            {
                if (roomItem.GetBaseItem().ItemName.StartsWith("CF_") || roomItem.GetBaseItem().ItemName.StartsWith("CFC_"))
                {
                    creditCount += magotCount;
                }
                else if (roomItem.GetBaseItem().ItemName.StartsWith("PntEx_"))
                {
                    wibboPointsCount += magotCount;
                }
                else if (roomItem.GetBaseItem().ItemName.StartsWith("WwnEx_"))
                {
                    winwinCount += magotCount;
                }
            }

            deleteItem.Add(roomItem);
        }

        using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();

        this.DeleteItems(dbClient, deleteItem);

        if (creditCount > 0)
        {
            this._userInstance.Credits += creditCount;
            this._userInstance.Client.SendPacket(new CreditBalanceComposer(this._userInstance.Credits));
        }

        if (wibboPointsCount > 0)
        {
            this._inventoryPoints = 0;
            this._userInstance.WibboPoints += wibboPointsCount;
            this._userInstance.Client.SendPacket(new ActivityPointNotificationComposer(this._userInstance.WibboPoints, 0, 105));

            UserDao.UpdateAddPoints(dbClient, this._userInstance.Id, wibboPointsCount);
        }

        if (winwinCount > 0)
        {
            UserStatsDao.UpdateAchievementScore(dbClient, this._userInstance.Id, winwinCount);

            this._userInstance.AchievementPoints += winwinCount;
            this._userInstance.Client.SendPacket(new AchievementScoreComposer(this._userInstance.AchievementPoints));

            var room = this._userInstance.CurrentRoom;

            if (room != null)
            {
                var roomUserByUserId = room.RoomUserManager.GetRoomUserByUserId(this._userInstance.Id);
                if (roomUserByUserId != null)
                {
                    this._userInstance.Client.SendPacket(new UserChangeComposer(roomUserByUserId, true));
                    room.SendPacket(new UserChangeComposer(roomUserByUserId, false));
                }
            }
        }
    }

    public void ClearPets()
    {
        using (var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor())
        {
            BotPetDao.Delete(dbClient, this._userInstance.Id);
        }

        this._petItems.Clear();
    }

    public void ClearBots()
    {
        using (var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor())
        {
            BotUserDao.Delete(dbClient, this._userInstance.Id);
        }

        this._botItems.Clear();
    }

    public void Dispose()
    {
        this._userItems.Clear();
        this._petItems.Clear();
        this._botItems.Clear();

        GC.SuppressFinalize(this);
    }

    public ICollection<Pet> GetPets() => this._petItems.Values;

    public bool TryAddPet(Pet pet)
    {
        pet.RoomId = 0;
        pet.PlacedInRoom = false;

        return this._petItems.TryAdd(pet.PetId, pet);
    }

    public bool TryRemovePet(int petId, out Pet petItem)
    {
        if (this._petItems.ContainsKey(petId))
        {
            return this._petItems.TryRemove(petId, out petItem);
        }
        else
        {
            petItem = null;
            return false;
        }
    }

    public bool TryGetPet(int petId, out Pet pet)
    {
        if (this._petItems.ContainsKey(petId))
        {
            return this._petItems.TryGetValue(petId, out pet);
        }
        else
        {
            pet = null;
            return false;
        }
    }

    public bool TryGetBot(int botId, out Bot bot)
    {
        if (this._botItems.ContainsKey(botId))
        {
            return this._botItems.TryGetValue(botId, out bot);
        }
        else
        {
            bot = null;
            return false;
        }
    }

    public bool TryRemoveBot(int botId, out Bot bot)
    {
        if (this._botItems.ContainsKey(botId))
        {
            return this._botItems.TryRemove(botId, out bot);
        }
        else
        {
            bot = null;
            return false;
        }
    }

    public bool TryAddBot(Bot bot) => this._botItems.TryAdd(bot.Id, bot);

    public ICollection<Bot> GetBots() => this._botItems.Values;

    public void TryAddItem(Item item)
    {
        if (this._userItems.TryAdd(item.Id, item))
        {
            this.AddInventoryPoint(item);

            this._userInstance.Client.SendPacket(new UnseenItemsComposer(item.Id, UnseenItemsType.Furni));
            this._userInstance.Client.SendPacket(new FurniListAddComposer(item));
        }
    }

    public void LoadInventory()
    {
        if (this._inventoryDefined)
        {
            return;
        }

        this._inventoryDefined = true;

        this._userItems.Clear();
        this._botItems.Clear();
        this._petItems.Clear();

        using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
        var dItems = ItemDao.GetAllByUserId(dbClient, this._userInstance.Id, this._furniLimit);

        foreach (DataRow dataRow in dItems.Rows)
        {
            var id = Convert.ToInt32(dataRow["id"]);
            var baseItem = Convert.ToInt32(dataRow["base_item"]);
            var extraData = DBNull.Value.Equals(dataRow["extra_data"]) ? string.Empty : (string)dataRow["extra_data"];
            var limited = DBNull.Value.Equals(dataRow["limited_number"]) ? 0 : Convert.ToInt32(dataRow["limited_number"]);
            var limitedTo = DBNull.Value.Equals(dataRow["limited_stack"]) ? 0 : Convert.ToInt32(dataRow["limited_stack"]);

            var userItem = new Item(id, 0, baseItem, extraData, limited, limitedTo, 0, 0, 0.0, 0, "", null);
            _ = this._userItems.TryAdd(id, userItem);

            this.AddInventoryPoint(userItem);
        }

        if (this._userItems.Count >= this._furniLimit)
        {
            this._userInstance.Client.SendHugeNotif(string.Format(WibboEnvironment.GetLanguageManager().TryGetValue("inventory.limit.furni", this._userInstance.Client.Langue), this._furniLimit));
        }

        var dPets = BotPetDao.GetAllByUserId(dbClient, this._userInstance.Id, this._petLimit);
        if (dPets != null)
        {
            foreach (DataRow row in dPets.Rows)
            {
                var pet = new Pet(Convert.ToInt32(row["id"]), Convert.ToInt32(row["user_id"]), Convert.ToInt32(row["room_id"]), (string)row["name"], Convert.ToInt32(row["type"]), (string)row["race"], (string)row["color"], Convert.ToInt32(row["experience"]), Convert.ToInt32(row["energy"]), Convert.ToInt32(row["nutrition"]), Convert.ToInt32(row["respect"]), (int)row["createstamp"], Convert.ToInt32(row["x"]), Convert.ToInt32(row["y"]), (double)row["z"], Convert.ToInt32(row["have_saddle"]), Convert.ToInt32(row["hairdye"]), Convert.ToInt32(row["pethair"]), Convert.ToBoolean(row["anyone_ride"]));
                _ = this._petItems.TryAdd(pet.PetId, pet);
            }
        }

        if (this._petItems.Count >= this._petLimit)
        {
            this._userInstance.Client.SendHugeNotif(string.Format(WibboEnvironment.GetLanguageManager().TryGetValue("inventory.limit.pet", this._userInstance.Client.Langue), this._petLimit));
        }

        var dBots = BotUserDao.GetAllByUserId(dbClient, this._userInstance.Id, this._botLimit);
        if (dBots != null)
        {
            foreach (DataRow row in dBots.Rows)
            {
                var bot = new Bot(Convert.ToInt32(row["id"]), Convert.ToInt32(row["user_id"]), (string)row["name"], (string)row["motto"], (string)row["look"], (string)row["gender"], Convert.ToBoolean(row["walk_enabled"]), Convert.ToBoolean(row["chat_enabled"]), (string)row["chat_text"], Convert.ToInt32(row["chat_seconds"]), Convert.ToBoolean(row["is_dancing"]), Convert.ToInt32(row["enable"]), Convert.ToInt32(row["handitem"]), Convert.ToInt32(row["status"]), BotUtility.GetAIFromString((string)row["ai_type"]));
                _ = this._botItems.TryAdd(Convert.ToInt32(row["id"]), bot);
            }
        }

        if (this._botItems.Count >= this._botLimit)
        {
            this._userInstance.Client.SendHugeNotif(string.Format(WibboEnvironment.GetLanguageManager().TryGetValue("inventory.limit.bot", this._userInstance.Client.Langue), this._botLimit));
        }
    }

    public void AddInventoryPoint(Item roomItem)
    {
        if (roomItem == null || roomItem.GetBaseItem() == null || roomItem.GetBaseItem().InteractionType != InteractionType.EXCHANGE)
        {
            return;
        }

        if (roomItem.GetBaseItem().ItemName.StartsWith("PntEx_") == false)
        {
            return;
        }

        var magotCount = int.Parse(roomItem.GetBaseItem().ItemName.Split(new char[1] { '_' })[1]);

        this._inventoryPoints += magotCount;
    }

    public void RemoveInventoryPoint(Item roomItem)
    {
        if (roomItem == null || roomItem.GetBaseItem() == null || roomItem.GetBaseItem().InteractionType != InteractionType.EXCHANGE)
        {
            return;
        }

        if (roomItem.GetBaseItem().ItemName.StartsWith("PntEx_") == false)
        {
            return;
        }

        var magotCount = int.Parse(roomItem.GetBaseItem().ItemName.Split(new char[1] { '_' })[1]);

        this._inventoryPoints -= magotCount;
    }

    public Item GetItem(int id)
    {
        if (this._userItems.TryGetValue(id, out var value))
        {
            return value;
        }
        else
        {
            return null;
        }
    }

    private bool UserHoldsItem(int itemID) => this._userItems.ContainsKey(itemID);

    public void RemoveItem(int id, bool sendComposed = true)
    {
        if (this._userItems.TryGetValue(id, out var item))
        {
            this.RemoveInventoryPoint(item);

            _ = this._userItems.TryRemove(id, out _);
        }

        if (sendComposed)
        {
            this._userInstance.Client.SendPacket(new FurniListRemoveComposer(id));
        }
    }

    public IEnumerable<Item> GetWallAndFloor => this._userItems.Values;

    public void AddItemArray(List<Item> roomItemList)
    {
        using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
        foreach (var roomItem in roomItemList)
        {
            this.AddItem(dbClient, roomItem);
        }
    }

    public void AddItem(IQueryAdapter dbClient, Item item)
    {
        ItemDao.UpdateRoomIdAndUserId(dbClient, item.Id, 0, this._userInstance.Id);

        var userItem = new Item(item.Id, 0, item.BaseItem, item.ExtraData, item.Limited, item.LimitedStack, 0, 0, 0.0, 0, "", null);
        if (this.UserHoldsItem(item.Id))
        {
            this.RemoveItem(item.Id);
        }

        _ = this._userItems.TryAdd(userItem.Id, userItem);

        this.AddInventoryPoint(userItem);

        this._userInstance.Client.SendPacket(new FurniListAddComposer(userItem));
    }

    public bool IsOverlowLimit(int amountPurchase, string type)
    {
        if (type is "s" or "i")
        {
            return this._userItems.Count + amountPurchase >= this._furniLimit;
        }
        else if (type is "r")
        {
            return this._botItems.Count + amountPurchase >= this._botLimit;
        }
        else if (type is "p")
        {
            return this._petItems.Count + amountPurchase >= this._petLimit;
        }

        return false;
    }
}
