﻿using WibboEmulator.Game.Clients;
using WibboEmulator.Game.Rooms;

namespace WibboEmulator.Game.Chat.Commands.Cmd
{
    internal class DND : IChatCommand
    {
        public void Execute(Client Session, Room Room, RoomUser UserRoom, string[] Params)
        {
            Session.GetUser().IgnoreRoomInvites = !Session.GetUser().IgnoreRoomInvites;
            Session.SendWhisper("Tu " + (Session.GetUser().IgnoreRoomInvites == true ? "acceptes" : "refuses") + " les messages dans ta console d'amis");
        }
    }
}
