using Butterfly.Game.GameClients;
using Butterfly.Game.Rooms.Janken;

namespace Butterfly.Game.Rooms.Chat.Commands.Cmd
            {
                return;
            }

            if (UserRoom.Team != Team.none || UserRoom.InGame)
            {
                return;
            }

            if (Session.GetHabbo().SpectatorMode)
            {
                return;
            }

            string Username = Params[1];
            {
                return;
            }

            Room room = UserRoom.Room;
            {
                return;
            }

            RoomUser roomuser = room.GetRoomUserManager().GetRoomUserByName(Username);
            {
                return;
            }

            JankenManager Jankan = room.GetJanken();