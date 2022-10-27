namespace WibboEmulator.Games.Chats.Commands.Staff.Administration;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Rooms;

internal class StartGameJD : IChatCommand
{
    public void Execute(GameClient session, Room room, RoomUser userRoom, string[] parameters)
    {
        var roomId = 0;
        if (parameters.Length > 1)
        {
            _ = int.TryParse(parameters[1], out roomId);
        }

        WibboEnvironment.GetGame().GetAnimationManager().StartGame(roomId);
        session.SendWhisper("Lancement de l'animation de Jack & Daisy !");
    }
}
