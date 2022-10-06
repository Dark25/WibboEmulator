﻿namespace WibboEmulator.Games.Chat.Commands.Staff.Administration;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Rooms;

internal class ForceSit : IChatCommand
{
    public void Execute(GameClient session, Room Room, RoomUser UserRoom, string[] parameters)
    {
        if (parameters.Length == 1)
        {
            return;
        }

        var User = Room.GetRoomUserManager().GetRoomUserByName(parameters[1]);
        if (User == null)
        {
            return;
        }

        if (User.ContainStatus("lay") || User.IsLay || User.RidingHorse || User.IsWalking || User.IsSit)
        {
            return;
        }

        if (!User.ContainStatus("sit"))
        {
            if (User.RotBody % 2 == 0)
            {
                if (User == null)
                {
                    return;
                }

                try
                {
                    User.SetStatus("sit", "1.0");
                    User.Z -= 0.35;
                    User.IsSit = true;
                    User.UpdateNeeded = true;
                }
                catch { }
            }
            else
            {
                User.RotBody--;
                User.SetStatus("sit", "1.0");
                User.Z -= 0.35;
                User.IsSit = true;
                User.UpdateNeeded = true;
            }
        }
        else if (User.IsSit)
        {
            User.Z += 0.35;
            User.RemoveStatus("sit");
            User.IsSit = false;
            User.UpdateNeeded = true;
        }
    }
}