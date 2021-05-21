using Butterfly.HabboHotel.GameClients;using System.Linq;namespace Butterfly.HabboHotel.Rooms.Chat.Commands.Cmd{    internal class RoomEnable : IChatCommand    {        public void Execute(GameClient Session, Room Room, RoomUser UserRoom, string[] Params)        {            if (!int.TryParse(Params[1], out int NumEnable))
            {
                return;
            }

            if (!ButterflyEnvironment.GetGame().GetEffectsInventoryManager().HaveEffect(NumEnable, Session.GetHabbo().HasFuse("fuse_sysadmin")))
            {
                return;
            }

            foreach (RoomUser User in Room.GetRoomUserManager().GetUserList().ToList())            {
                if (!User.IsBot)
                {
                    User.ApplyEffect(NumEnable);
                }            }        }    }}