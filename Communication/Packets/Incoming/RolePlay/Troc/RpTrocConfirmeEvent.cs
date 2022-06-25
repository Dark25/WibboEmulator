﻿using WibboEmulator.Game.Clients;
using WibboEmulator.Game.Roleplay.Player;
using WibboEmulator.Game.Rooms;

namespace WibboEmulator.Communication.Packets.Incoming.RolePlay.Troc
{
    internal class RpTrocConfirmeEvent : IPacketEvent
    {
        public double Delay => 0;

        public void Parse(Client Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetUser() == null)
            {
                return;
            }

            Room Room = Session.GetUser().CurrentRoom;
            if (Room == null || !Room.IsRoleplay)
            {
                return;
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByUserId(Session.GetUser().Id);
            if (User == null)
            {
                return;
            }

            RolePlayer Rp = User.Roleplayer;
            if (Rp == null || Rp.TradeId == 0)
            {
                return;
            }

            WibboEnvironment.GetGame().GetRoleplayManager().GetTrocManager().Confirme(Rp.TradeId, User.UserId);
        }
    }
}
