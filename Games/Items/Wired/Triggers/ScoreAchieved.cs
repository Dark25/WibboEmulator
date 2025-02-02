namespace WibboEmulator.Games.Items.Wired.Triggers;
using WibboEmulator.Database.Interfaces;
using WibboEmulator.Games.Items.Wired.Bases;
using WibboEmulator.Games.Items.Wired.Interfaces;
using WibboEmulator.Games.Rooms;
using WibboEmulator.Games.Rooms.Events;

public class ScoreAchieved : WiredTriggerBase, IWired
{
    public ScoreAchieved(Item item, Room room) : base(item, room, (int)WiredTriggerType.SCORE_ACHIEVED)
    {
        this.RoomInstance.GameManager.OnScoreChanged += this.OnScoreChanged;

        this.DefaultIntParams(new int[] { 0 });
    }

    private void OnScoreChanged(object sender, TeamScoreChangedEventArgs e)
    {
        var scoreLevel = this.GetIntParam(0);
        if (e.Points <= scoreLevel - 1)
        {
            return;
        }

        this.RoomInstance.WiredHandler.ExecutePile(this.ItemInstance.Coordinate, e.User, null);
    }

    public override void Dispose()
    {
        this.RoomInstance.GameManager.OnScoreChanged -= this.OnScoreChanged;

        base.Dispose();
    }

    public void SaveToDatabase(IQueryAdapter dbClient)
    {
        var scoreLevel = this.GetIntParam(0);
        WiredUtillity.SaveTriggerItem(dbClient, this.Id, string.Empty, scoreLevel.ToString(), false, null);
    }

    public void LoadFromDatabase(string wiredTriggerData, string wiredTriggerData2, string wiredTriggersItem, bool wiredAllUserTriggerable, int wiredDelay)
    {
        if (int.TryParse(wiredTriggerData, out var score))
        {
            this.SetIntParam(0, score);
        }
    }
}
