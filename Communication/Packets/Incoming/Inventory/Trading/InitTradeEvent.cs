namespace WibboEmulator.Communication.Packets.Incoming.Inventory.Trading;
using WibboEmulator.Games.GameClients;

internal class InitTradeEvent : IPacketEvent
{
    public double Delay => 5000;

    public void Parse(GameClient session, ClientPacket packet)
    {
        if (!WibboEnvironment.GetGame().GetRoomManager().TryGetRoom(session.GetUser().CurrentRoomId, out var room))
        {
            return;
        }

        var virtualId = packet.PopInt();

        var roomUser = room.GetRoomUserManager().GetRoomUserByUserId(session.GetUser().Id);
        var roomUserTarget = room.GetRoomUserManager().GetRoomUserByVirtualId(virtualId);
        if (roomUser == null || roomUser.GetClient() == null || roomUser.GetClient().GetUser() == null)
        {
            return;
        }

        if (roomUserTarget == null || roomUserTarget.GetClient() == null || roomUserTarget.GetClient().GetUser() == null)
        {
            return;
        }

        if (room.IsRoleplay)
        {
            var rp = roomUser.Roleplayer;
            if (rp == null || rp.TradeId > 0 || rp.Dead || rp.SendPrison || (rp.PvpEnable && room.Roleplay.Pvp) || rp.AggroTimer > 0)
            {
                roomUser.SendWhisperChat("Vous devez être en zone safe pour pouvoir troquer");
                return;
            }

            var rpTarget = roomUserTarget.Roleplayer;
            if (rpTarget == null || rpTarget.TradeId > 0 || rpTarget.Dead || rpTarget.SendPrison || (rpTarget.PvpEnable && room.Roleplay.Pvp) || rpTarget.AggroTimer > 0)
            {
                roomUser.SendWhisperChat("Ce joueur ne peut pas troc");
                return;
            }

            WibboEnvironment.GetGame().GetRoleplayManager().GetTrocManager().AddTrade(room.RoomData.OwnerId, roomUser.UserId, roomUserTarget.UserId, roomUser.GetUsername(), roomUserTarget.GetUsername());
            return;
        }

        if (room.RoomData.TrocStatus == 0)
        {
            session.SendNotification(WibboEnvironment.GetLanguageManager().TryGetValue("notif.trade.error.1", session.Langue));
            return;
        }
        else if (room.RoomData.TrocStatus == 1 && !room.CheckRights(session))
        {
            session.SendNotification(WibboEnvironment.GetLanguageManager().TryGetValue("notif.trade.error.2", session.Langue));
            return;
        }

        if (!roomUserTarget.GetClient().GetUser().AcceptTrading && session.GetUser().Rank < 3)
        {
            session.SendNotification(WibboEnvironment.GetLanguageManager().TryGetValue("user.tradedisabled", session.Langue));
        }
        else if (roomUserTarget.IsTransf || roomUser.IsTransf || roomUser.IsSpectator || roomUserTarget.IsSpectator)
        {
            session.SendNotification(WibboEnvironment.GetLanguageManager().TryGetValue("notif.trade.error.3", session.Langue));
        }
        else
        {
            room.TryStartTrade(roomUser, roomUserTarget);
        }
    }
}
