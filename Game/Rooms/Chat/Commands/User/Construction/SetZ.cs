using Butterfly.Game.GameClients;
namespace Butterfly.Game.Rooms.Chat.Commands.Cmd
            {
                return;
            }

            string Heigth = Params[1];
            {
                return;
            }

            if (Result < -100)
            {
                Result = 0;
            }

            if (Result > 100)
            {
                Result = 100;
            }

            UserRoom.ConstruitZMode = true;
            {
                Session.SendPacket(Room.GetGameMap().Model.setHeightMap((Result > 63) ? 63 : Result));
            }
        }