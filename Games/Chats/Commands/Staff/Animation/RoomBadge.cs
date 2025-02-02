namespace WibboEmulator.Games.Chats.Commands.Staff.Animation;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Rooms;

internal sealed class RoomBadge : IChatCommand
{
    public void Execute(GameClient session, Room room, RoomUser userRoom, string[] parameters)
    {
        if (parameters.Length != 2)
        {
            return;
        }

        var badgeId = parameters[1];

        foreach (var user in room.RoomUserManager.GetUserList().ToList())
        {
            if (!user.IsBot)
            {
                user.Client?.User?.BadgeComponent.GiveBadge(badgeId, true);
            }
        }

    }
}
