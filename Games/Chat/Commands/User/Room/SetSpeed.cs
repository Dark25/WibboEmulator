namespace WibboEmulator.Games.Chat.Commands.User.Room;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Rooms;

internal class SetSpeed : IChatCommand
{
    public void Execute(GameClient session, Room Room, RoomUser UserRoom, string[] parameters)
    {
        if (parameters.Length < 2)
        {
            session.SendWhisper(WibboEnvironment.GetLanguageManager().TryGetValue("input.intonly", session.Langue));
            return;
        }

        if (int.TryParse(parameters[1], out var setSpeedCount))
        {
            Room.GetRoomItemHandler().SetSpeed(setSpeedCount);
        }
        else
        {
            session.SendWhisper(WibboEnvironment.GetLanguageManager().TryGetValue("input.intonly", session.Langue));
        }
    }
}
