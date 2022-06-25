using WibboEmulator.Communication.Packets.Outgoing.Rooms.Engine;
using WibboEmulator.Game.Clients;
using WibboEmulator.Game.Rooms.Games;
using WibboEmulator.Game.Rooms;

namespace WibboEmulator.Game.Chat.Commands.Cmd
{
    internal class TransfBot : IChatCommand
    {
        public void Execute(Client Session, Room Room, RoomUser UserRoom, string[] Params)
        {
            if (UserRoom.Team != TeamType.NONE || UserRoom.InGame)
            {
                return;
            }

            if (!UserRoom.IsTransf && !UserRoom.IsSpectator)
            {
                Room RoomClient = Session.GetUser().CurrentRoom;
                if (RoomClient != null)
                {
                    UserRoom.TransfBot = !UserRoom.TransfBot;

                    RoomClient.SendPacket(new UserRemoveComposer(UserRoom.VirtualId));
                    RoomClient.SendPacket(new UsersComposer(UserRoom));
                }
            }
        }
    }
}
