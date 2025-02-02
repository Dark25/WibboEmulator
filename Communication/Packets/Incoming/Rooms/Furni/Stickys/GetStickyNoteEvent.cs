namespace WibboEmulator.Communication.Packets.Incoming.Rooms.Furni.Stickys;
using WibboEmulator.Communication.Packets.Outgoing.Rooms.Furni.Stickys;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Items;

internal sealed class GetStickyNoteEvent : IPacketEvent
{
    public double Delay => 250;

    public void Parse(GameClient session, ClientPacket packet)
    {
        if (!WibboEnvironment.GetGame().GetRoomManager().TryGetRoom(session.User.CurrentRoomId, out var room))
        {
            return;
        }

        var roomItem = room.RoomItemHandling.GetItem(packet.PopInt());
        if (roomItem == null || roomItem.GetBaseItem().InteractionType != InteractionType.POSTIT)
        {
            return;
        }

        session.SendPacket(new StickyNoteComposer(roomItem.Id, roomItem.ExtraData));
    }
}
