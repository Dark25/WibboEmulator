﻿namespace WibboEmulator.Communication.Packets.Incoming.Settings;
using WibboEmulator.Database.Daos.User;
using WibboEmulator.Games.GameClients;

internal class UserSettingsCameraFollowEvent : IPacketEvent
{
    public double Delay => 0;

    public void Parse(GameClient session, ClientPacket Packet)
    {
        var flag = Packet.PopBoolean();

        if (session == null || session.GetUser() == null)
        {
            return;
        }

        using (var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor())
        {
            UserDao.UpdateCameraFollowDisabled(dbClient, session.GetUser().Id, flag);
        }

        session.GetUser().CameraFollowDisabled = flag;
    }
}
