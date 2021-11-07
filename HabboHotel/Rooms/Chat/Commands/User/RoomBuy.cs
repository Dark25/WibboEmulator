﻿using Butterfly.Communication.Packets.Outgoing.Inventory.Purse;
using Butterfly.Communication.Packets.Outgoing.Rooms.Session;
using Butterfly.Database.Daos;
using Butterfly.Database.Interfaces;
using Butterfly.HabboHotel.GameClients;
using System.Collections.Generic;
using System.Linq;

namespace Butterfly.HabboHotel.Rooms.Chat.Commands.Cmd
{
    internal class RoomBuy : IChatCommand
    {
        public void Execute(GameClient Session, Room Room, RoomUser UserRoom, string[] Params)
        {
            if (Session == null || Session.GetHabbo() == null)
            {
                return;
            }

            if (Room.RoomData.SellPrice == 0)
            {
                return;
            }

            if(Session.GetHabbo().WibboPoints - Room.RoomData.SellPrice <= 0)
            {
                return;
            }

            Session.GetHabbo().WibboPoints -= Room.RoomData.SellPrice;
            Session.SendPacket(new ActivityPointsComposer(Session.GetHabbo().WibboPoints));

            GameClient ClientOwner = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(Room.RoomData.OwnerId);
            if (ClientOwner != null && ClientOwner.GetHabbo() != null)
            {
                ClientOwner.GetHabbo().WibboPoints += Room.RoomData.SellPrice;
                ClientOwner.SendPacket(new ActivityPointsComposer(ClientOwner.GetHabbo().WibboPoints));
            }

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE users SET vip_points = vip_points - '" + Room.RoomData.SellPrice + "' WHERE id = '" + Session.GetHabbo().Id + "'");
                dbClient.RunQuery("UPDATE users SET vip_points = vip_points + '" + Room.RoomData.SellPrice + "' WHERE id = '" + Room.RoomData.OwnerId + "'");

                RoomRightDao.Delete(dbClient, Room.Id);

                dbClient.SetQuery("UPDATE rooms SET owner = @newowner WHERE id = '" + Room.Id + "'");
                dbClient.AddParameter("newowner", Session.GetHabbo().Username);
                dbClient.RunQuery();

                dbClient.RunQuery("UPDATE rooms SET price = '0' WHERE id = '" + Room.Id + "' LIMIT 1");
            }

            Session.SendNotification(string.Format(ButterflyEnvironment.GetLanguageManager().TryGetValue("roombuy.sucess", Session.Langue), Room.RoomData.SellPrice));

            Room.RoomData.SellPrice = 0;

            List<RoomUser> UsersToReturn = Room.GetRoomUserManager().GetRoomUsers().ToList();
            ButterflyEnvironment.GetGame().GetRoomManager().UnloadRoom(Room);

            foreach (RoomUser User in UsersToReturn)
            {
                if (User == null || User.GetClient() == null)
                {
                    continue;
                }

                User.GetClient().SendPacket(new RoomForwardComposer(Room.Id));
            }
        }
    }
}
