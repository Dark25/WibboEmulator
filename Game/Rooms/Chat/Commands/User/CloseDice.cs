using Butterfly.Game.GameClients;
using Butterfly.Game.Items;
using System.Collections.Generic;
using System.Linq;

namespace Butterfly.Game.Rooms.Chat.Commands.Cmd
{
    internal class CloseDice : IChatCommand
    {
        public void Execute(GameClient Session, Room Room, RoomUser UserRoom, string[] Params)
        {
            List<Item> userBooth = Room.GetRoomItemHandler().GetFloor.Where(x => x != null && Gamemap.TilesTouching(
                x.GetX, x.GetY, UserRoom.X, UserRoom.Y) && x.Data.InteractionType == Items.InteractionType.DICE).ToList();

            if (userBooth == null)
            {
                return;
            }

            userBooth.ForEach(x =>
            {
                x.ExtraData = "0";
                x.UpdateState();
            });
        }
    }
}