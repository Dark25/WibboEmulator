namespace WibboEmulator.Games.Items.Wired.Conditions;
using WibboEmulator.Database.Interfaces;
using WibboEmulator.Games.Items.Wired.Bases;
using WibboEmulator.Games.Items.Wired.Interfaces;
using WibboEmulator.Games.Rooms;

public class MoreThanTimer : WiredConditionBase, IWiredCondition, IWired
{
    public MoreThanTimer(Item item, Room room) : base(item, room, (int)WiredConditionType.TIME_ELAPSED_MORE) => this.DefaultIntParams(new int[] { 0 });

    public bool AllowsExecution(RoomUser user, Item item)
    {
        var timeout = this.GetIntParam(0);

        _ = this.RoomInstance.LastTimerReset;
        return (DateTime.Now - this.RoomInstance.LastTimerReset).TotalSeconds > timeout / 2;
    }

    public void SaveToDatabase(IQueryAdapter dbClient)
    {
        var timeout = this.GetIntParam(0);

        WiredUtillity.SaveTriggerItem(dbClient, this.Id, string.Empty, timeout.ToString(), false, null);
    }

    public void LoadFromDatabase(string wiredTriggerData, string wiredTriggerData2, string wiredTriggersItem, bool wiredAllUserTriggerable, int wiredDelay)
    {
        if (int.TryParse(wiredTriggerData, out var timeout))
        {
            this.SetIntParam(0, timeout);
        }
    }
}
