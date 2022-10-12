namespace WibboEmulator.Communication.Packets.Incoming.Groups;
using WibboEmulator.Communication.Packets.Outgoing.Groups;
using WibboEmulator.Communication.Packets.Outgoing.Rooms.Permissions;
using WibboEmulator.Database.Daos.Guild;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Groups;

internal class UpdateGroupSettingsEvent : IPacketEvent
{
    public double Delay => 500;

    public void Parse(GameClient session, ClientPacket packet)
    {
        var groupId = packet.PopInt();

        if (!WibboEnvironment.GetGame().GetGroupManager().TryGetGroup(groupId, out var group))
        {
            return;
        }

        if (group.CreatorId != session.GetUser().Id)
        {
            return;
        }

        var type = packet.PopInt();
        var furniOptions = packet.PopInt();

        group.GroupType = type switch
        {
            1 => GroupType.LOCKED,
            2 => GroupType.PRIVATE,
            _ => GroupType.OPEN,
        };
        if (group.GroupType != GroupType.LOCKED)
        {
            if (group.GetRequests.Count > 0)
            {
                foreach (var userId in group.GetRequests.ToList())
                {
                    group.HandleRequest(userId, false);
                }

                group.ClearRequests();
            }
        }

        using (var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor())
        {
            GuildDao.UpdateStateAndDeco(dbClient, group.Id, group.GroupType == GroupType.OPEN ? 0 : group.GroupType == GroupType.LOCKED ? 1 : 2, furniOptions);
        }

        group.AdminOnlyDeco = furniOptions;

        if (!WibboEnvironment.GetGame().GetRoomManager().TryGetRoom(group.RoomId, out var room))
        {
            return;
        }

        foreach (var user in room.GetRoomUserManager().GetRoomUsers().ToList())
        {
            if (room.Data.OwnerId == user.UserId || group.IsAdmin(user.UserId) || !group.IsMember(user.UserId))
            {
                continue;
            }

            if (furniOptions == 1)
            {
                user.RemoveStatus("flatctrl");
                user.UpdateNeeded = true;

                user.Client.SendPacket(new YouAreControllerComposer(0));
            }
            else if (furniOptions == 0 && !user.ContainStatus("flatctrl"))
            {
                user.SetStatus("flatctrl", "1");
                user.UpdateNeeded = true;

                user.Client.SendPacket(new YouAreControllerComposer(1));
            }
        }

        session.SendPacket(new GroupInfoComposer(group, session));
    }
}
