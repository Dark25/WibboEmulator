using Butterfly.Communication.Packets.Outgoing.Moderation;
using Butterfly.Game.Clients;
using Butterfly.Game.Rooms;
using System;

namespace Butterfly.Game.Chat.Commands.Cmd
{
    internal class About : IChatCommand
    {
        public void Execute(Client Session, Room Room, RoomUser UserRoom, string[] Params)
        {
            TimeSpan Uptime = DateTime.Now - ButterflyEnvironment.ServerStarted;

            Session.SendPacket(new BroadcastMessageAlertComposer("<b>Butterfly Edition Wibbo</b>\n\n" +
                "   <b>Credits</b>:\n" +
                "   Meth0d, Matinmine, Carlos, Super0ca,\n" +
                "   Mike, Sledmore, Joopie, Tweeny, \n" +
                "   JasonDhose, Leenster, Moogly, Niels, AKllX, rbi0s\n\n"));
        }
    }
}