namespace WibboEmulator.Games.Items.Wired.Conditions;
using System.Data;
using WibboEmulator.Database.Interfaces;
using WibboEmulator.Games.Items.Wired.Bases;
using WibboEmulator.Games.Items.Wired.Interfaces;
using WibboEmulator.Games.Rooms;
using WibboEmulator.Games.Rooms.Games.Teams;

public class ActorInTeam : WiredConditionBase, IWiredCondition, IWired
{
    public ActorInTeam(Item item, Room room) : base(item, room, (int)WiredConditionType.ACTOR_IS_IN_TEAM) => this.IntParams.Add((int)TeamType.RED);

    public bool AllowsExecution(RoomUser user, Item item)
    {
        if (user == null)
        {
            return false;
        }

        var teamId = (this.IntParams.Count > 0) ? this.IntParams[0] : 1;
        if (teamId is < 1 or > 4)
        {
            teamId = 1;
        }

        if (user.Team != (TeamType)teamId)
        {
            return false;
        }

        return true;
    }

    public void SaveToDatabase(IQueryAdapter dbClient)
    {
        var teamId = (this.IntParams.Count > 0) ? this.IntParams[0] : 1;

        WiredUtillity.SaveTriggerItem(dbClient, this.Id, string.Empty, teamId.ToString(), false, null);
    }

    public void LoadFromDatabase(DataRow row)
    {
        this.IntParams.Clear();

        if (int.TryParse(row["trigger_data"].ToString(), out var teamId))
        {
            this.IntParams.Add(teamId);
        }
    }
}
