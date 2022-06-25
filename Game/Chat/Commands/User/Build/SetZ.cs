using WibboEmulator.Game.Clients;
using WibboEmulator.Game.Rooms;

namespace WibboEmulator.Game.Chat.Commands.Cmd
{
    internal class SetZ : IChatCommand
    {
        public void Execute(Client Session, Room Room, RoomUser UserRoom, string[] Params)
        {
            if (Params.Length != 2)
            {
                return;
            }

            string Heigth = Params[1];
            if (!double.TryParse(Heigth, out double Result))
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
            UserRoom.ConstruitHeigth = Result;

            Session.SendWhisper("SetZ: " + Result);

            if (Result >= 0)
            {
                Session.SendPacket(Room.GetGameMap().Model.SetHeightMap((Result > 63) ? 63 : Result));
            }
        }
    }
}
