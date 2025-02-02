namespace WibboEmulator.Games.Chats.Commands.User.Room;

using WibboEmulator.Database.Daos.Room;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Rooms;

internal sealed class SetMax : IChatCommand
{
    public void Execute(GameClient session, Room room, RoomUser userRoom, string[] parameters)
    {
        if (parameters.Length < 2)
        {
            return;
        }

        if (!int.TryParse(parameters[1], out var maxUsers))
        {
            return;
        }

        if ((maxUsers > 100 || maxUsers <= 0) && !session.User.HasPermission("mod"))
        {
            room.RoomData.UsersMax = 100;
        }
        else
        {
            room.RoomData.UsersMax = maxUsers;
        }

        using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
        RoomDao.UpdateUsersMax(dbClient, room.Id, maxUsers);
    }
}
