namespace WibboEmulator.Games.Items.Wired.Conditions;
using System.Data;
using WibboEmulator.Database.Interfaces;
using WibboEmulator.Games.Items.Wired.Bases;
using WibboEmulator.Games.Items.Wired.Interfaces;
using WibboEmulator.Games.Rooms;

public class FurniNotCollisionIs : WiredConditionBase, IWiredCondition, IWired
{
    public FurniNotCollisionIs(Item item, Room room) : base(item, room, (int)WiredConditionType.NOT_FURNI_IS_OF_TYPE)
    {
    }

    public bool AllowsExecution(RoomUser user, Item item)
    {
        if (item == null)
        {
            return false;
        }

        foreach (var roomItem in this.Items.ToList())
        {
            if (roomItem.Id == item.Id)
            {
                return false;
            }
        }
        return true;
    }

    public void SaveToDatabase(IQueryAdapter dbClient) => WiredUtillity.SaveTriggerItem(dbClient, this.Id, string.Empty, string.Empty, false, this.Items);

    public void LoadFromDatabase(DataRow row)
    {
        var triggerItems = row["triggers_item"].ToString();

        if (triggerItems is null or "")
        {
            return;
        }

        foreach (var itemId in triggerItems.Split(';'))
        {
            if (!int.TryParse(itemId, out var id))
            {
                continue;
            }

            if (!this.StuffIds.Contains(id))
            {
                this.StuffIds.Add(id);
            }
        }
    }
}