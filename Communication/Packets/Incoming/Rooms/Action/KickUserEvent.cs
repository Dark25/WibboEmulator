namespace WibboEmulator.Communication.Packets.Incoming.Rooms.Action;
using WibboEmulator.Games.GameClients;

internal class KickUserEvent : IPacketEvent
{
    public double Delay => 250;

    public void Parse(GameClient session, ClientPacket packet)
    {
        if (session.GetUser() == null)
        {
            return;
        }

        if (!WibboEnvironment.GetGame().GetRoomManager().TryGetRoom(session.GetUser().CurrentRoomId, out var room))
        {
            return;
        }

        if (room.Data.WhoCanKick != 2 && (room.Data.WhoCanKick != 1 || !room.CheckRights(session)) && !room.CheckRights(session, true) && session.GetUser().Rank < 6)
        {
            return;
        }

        var pId = packet.PopInt();

        var roomUserByUserId = room.GetRoomUserManager().GetRoomUserByUserId(pId);
        if (roomUserByUserId == null || roomUserByUserId.IsBot || room.CheckRights(roomUserByUserId.Client, true) || roomUserByUserId.Client.GetUser().HasPermission("perm_mod") || roomUserByUserId.Client.GetUser().HasPermission("perm_no_kick"))
        {
            return;
        }

        room.GetRoomUserManager().RemoveUserFromRoom(roomUserByUserId.Client, true, true);
    }
}