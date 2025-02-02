namespace WibboEmulator.Games.Chats.Commands.Staff.Animation;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Items;
using WibboEmulator.Games.Rooms;

internal sealed class ExtraBox : IChatCommand
{
    public void Execute(GameClient session, Room room, RoomUser userRoom, string[] parameters)
    {

        _ = int.TryParse(parameters[1], out var nbLot);

        if (nbLot is < 0 or > 10)
        {
            return;
        }

        var lootboxId = WibboEnvironment.GetSettings().GetData<int>("givelot.lootbox.id");
        if (!WibboEnvironment.GetGame().GetItemManager().GetItem(lootboxId, out var itemData))
        {
            return;
        }

        using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();

        var items = ItemFactory.CreateMultipleItems(dbClient, itemData, session.User, "", nbLot);
        foreach (var purchasedItem in items)
        {
            session.User.InventoryComponent.TryAddItem(purchasedItem);
        }
    }
}
