using Butterfly.HabboHotel.GameClients;

namespace Butterfly.HabboHotel.Rooms.Chat.Commands.Cmd
{
    internal class AddFilter : IChatCommand
    {
        public void Execute(GameClient Session, Room Room, RoomUser UserRoom, string[] Params)
        {
            if (Params.Length != 2)
            {
                return;
            }

            ButterflyEnvironment.GetGame().GetChatManager().GetFilter().AddFilterPub(Params[1].ToLower());
        }
    }
}
