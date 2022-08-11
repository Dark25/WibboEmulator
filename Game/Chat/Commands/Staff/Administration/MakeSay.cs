using WibboEmulator.Game.Clients;
using WibboEmulator.Game.Rooms;

namespace WibboEmulator.Game.Chat.Commands.Cmd
{
    internal class MakeSay : IChatCommand
    {
        public void Execute(Client Session, Room Room, RoomUser UserRoom, string[] Params)
        {
            if (Params.Length < 3)
            {
                return;
            }

            string username = Params[1];
            string message = CommandManager.MergeParams(Params, 2);

            RoomUser roomUserByUserId = Session.GetUser().CurrentRoom.GetRoomUserManager().GetRoomUserByName(username);
            if (roomUserByUserId == null)
            {
                return;
            }

            roomUserByUserId.OnChat(message, 0, false);
        }
    }
}
