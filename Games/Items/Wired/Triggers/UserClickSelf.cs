namespace WibboEmulator.Games.Items.Wired.Triggers;
using WibboEmulator.Database.Interfaces;
using WibboEmulator.Games.Items.Wired.Bases;
using WibboEmulator.Games.Items.Wired.Interfaces;
using WibboEmulator.Games.Rooms;
using WibboEmulator.Games.Rooms.Events;

public class UserClickSelf : WiredTriggerBase, IWired
{
    public UserClickSelf(Item item, Room room) : base(item, room, (int)WiredTriggerType.AVATAR_CLICK)
    {
        this.DefaultIntParams(new int[] { 1 });

        room.OnUserClickSelf += this.OnUserClick;
    }

    private void OnUserClick(object sender, UserTargetEventArgs e)
    {
        if (sender is null or not RoomUser)
        {
            return;
        }

        var user = (RoomUser)sender;
        var userTarget = e.UserTarget;

        if (user == null || user.IsBot || userTarget == null)
        {
            return;
        }

        var distance = this.GetIntParam(0);

        distance += 1;

        if (Math.Abs(userTarget.X - user.X) >= distance || Math.Abs(userTarget.Y - user.Y) >= distance)
        {
            return;
        }

        this.RoomInstance.WiredHandler.ExecutePile(this.ItemInstance.Coordinate, user, null);
    }

    public override void Dispose()
    {
        base.Dispose();

        this.RoomInstance.OnUserClickSelf -= this.OnUserClick;
    }

    public void SaveToDatabase(IQueryAdapter dbClient)
    {
        var distance = this.GetIntParam(0);

        WiredUtillity.SaveTriggerItem(dbClient, this.Id, string.Empty, distance.ToString(), false, null);
    }

    public void LoadFromDatabase(string wiredTriggerData, string wiredTriggerData2, string wiredTriggersItem, bool wiredAllUserTriggerable, int wiredDelay)
    {
        if (int.TryParse(wiredTriggerData, out var distance))
        {
            this.SetIntParam(0, distance);
        }
    }
}
