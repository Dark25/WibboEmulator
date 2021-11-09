using Butterfly.HabboHotel.GameClients;namespace Butterfly.HabboHotel.Rooms.Chat.Commands.Cmd{    internal class SayBot : IChatCommand    {        public void Execute(GameClient Session, Room Room, RoomUser UserRoom, string[] Params)        {            if (Params.Length < 3)
            {
                return;
            }

            string username = Params[1];            RoomUser Bot = Room.GetRoomUserManager().GetBotOrPetByName(username);            if (Bot == null)
            {
                return;
            }

            string Message = CommandManager.MergeParams(Params, 2);            if (string.IsNullOrEmpty(Message))
            {
                return;
            }

            Bot.OnChat(Message, (Bot.IsPet) ? 0 : 2, false);        }    }}