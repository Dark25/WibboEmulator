﻿namespace WibboEmulator.Games.Chat.Commands.Staff.Administration;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Rooms;

internal class StartGameJD : IChatCommand
{
    public void Execute(GameClient session, Room Room, RoomUser UserRoom, string[] parameters)
    {
        var roomId = 0;
        if (parameters.Length > 1)
        {
            int.TryParse(parameters[1], out roomId);
        }

        WibboEnvironment.GetGame().GetAnimationManager().StartGame(roomId);
        session.SendWhisper("Lancement de l'animation de Jack & Daisy !");
    }
}
