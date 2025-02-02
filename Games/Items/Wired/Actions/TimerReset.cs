namespace WibboEmulator.Games.Items.Wired.Actions;
using WibboEmulator.Database.Interfaces;
using WibboEmulator.Games.Items.Wired.Bases;
using WibboEmulator.Games.Items.Wired.Interfaces;
using WibboEmulator.Games.Rooms;

public class TimerReset : WiredActionBase, IWiredEffect, IWired
{
    public TimerReset(Item item, Room room) : base(item, room, (int)WiredActionType.RESET)
    {
    }

    public override bool OnCycle(RoomUser user, Item item)
    {
        this.RoomInstance.WiredHandler.TriggerTimer();
        this.RoomInstance.LastTimerReset = DateTime.Now;

        return false;
    }

    public void SaveToDatabase(IQueryAdapter dbClient) => WiredUtillity.SaveTriggerItem(dbClient, this.Id, string.Empty, string.Empty, false, null, this.Delay);

    public void LoadFromDatabase(string wiredTriggerData, string wiredTriggerData2, string wiredTriggersItem, bool wiredAllUserTriggerable, int wiredDelay)
    {
        this.Delay = wiredDelay;

        if (int.TryParse(wiredTriggerData, out var delay))
        {
            this.Delay = delay;
        }
    }
}
