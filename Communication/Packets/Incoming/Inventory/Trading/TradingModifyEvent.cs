namespace WibboEmulator.Communication.Packets.Incoming.Inventory.Trading;
using WibboEmulator.Games.GameClients;

internal sealed class TradingModifyEvent : IPacketEvent
{
    public double Delay => 0;

    public void Parse(GameClient session, ClientPacket packet)
    {
        if (!WibboEnvironment.GetGame().GetRoomManager().TryGetRoom(session.User.CurrentRoomId, out var room))
        {
            return;
        }

        var userTrade = room.GetUserTrade(session.User.Id);
        if (userTrade == null)
        {
            return;
        }

        userTrade.Unaccept(session.User.Id);
    }
}
