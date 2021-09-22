﻿using Butterfly.Communication.Packets.Outgoing.Handshake;

using Butterfly.HabboHotel.GameClients;

namespace Butterfly.HabboHotel.Rooms.Chat.Commands.Cmd
{
    internal class Flagme : IChatCommand
    {
        public void Execute(GameClient Session, Room Room, RoomUser UserRoom, string[] Params)
        {
            if (Params.Length != 2)
            {
                return;
            }

            GameClient clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            {
                Session.SendNotification(ButterflyEnvironment.GetLanguageManager().TryGetValue("input.usernotfound", Session.Langue));
                return;
            }

            clientByUsername.GetHabbo().CanChangeName = true;
            clientByUsername.SendPacket(new UserObjectComposer(clientByUsername.GetHabbo()));
        }
    }
}