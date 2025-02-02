﻿namespace WibboEmulator.Communication.Packets.Incoming.Settings;
using WibboEmulator.Database.Daos.User;
using WibboEmulator.Games.GameClients;

internal sealed class UserSettingsRoomInvitesEvent : IPacketEvent
{
    public double Delay => 0;

    public void Parse(GameClient session, ClientPacket packet)
    {
        var flag = packet.PopBoolean();

        if (session == null || session.User == null)
        {
            return;
        }

        using (var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor())
        {
            UserDao.UpdateIgnoreRoomInvites(dbClient, session.User.Id, flag);
        }

        session.User.IgnoreRoomInvites = flag;
    }
}
