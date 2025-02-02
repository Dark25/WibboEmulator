namespace WibboEmulator.Communication.Packets.Incoming.Navigator;
using WibboEmulator.Communication.Packets.Outgoing.Navigator;
using WibboEmulator.Database.Daos.User;
using WibboEmulator.Games.GameClients;

internal sealed class RemoveFavouriteRoomEvent : IPacketEvent
{
    public double Delay => 250;

    public void Parse(GameClient session, ClientPacket packet)
    {
        if (session.User == null)
        {
            return;
        }

        var roomId = packet.PopInt();

        var roomdata = WibboEnvironment.GetGame().GetRoomManager().GenerateRoomData(roomId);
        if (roomdata == null)
        {
            return;
        }

        _ = session.User.FavoriteRooms.Remove(roomId);

        session.SendPacket(new UpdateFavouriteRoomComposer(roomId, false));

        using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
        UserFavoriteDao.Delete(dbClient, session.User.Id, roomId);
    }
}
