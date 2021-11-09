using Butterfly.HabboHotel.GameClients;namespace Butterfly.HabboHotel.Rooms.Chat.Commands.Cmd{    internal class WarpStaff : IChatCommand    {        public void Execute(GameClient Session, Room Room, RoomUser UserRoom, string[] Params)        {            if (Params.Length != 2)
            {
                return;
            }

            GameClient clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);            if (clientByUsername == null)
            {
                return;
            }

            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(clientByUsername.GetHabbo().CurrentRoomId);            if (room == null)
            {
                return;
            }

            RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabboId(clientByUsername.GetHabbo().Id);            if (roomUserByHabbo == null)
            {
                return;
            }

            RoomUser roomUserByHabbo2 = room.GetRoomUserManager().GetRoomUserByHabboId(Session.GetHabbo().Id);            if (roomUserByHabbo2 == null)
            {
                return;
            }

            room.SendPacket(room.GetRoomItemHandler().TeleportUser(roomUserByHabbo, roomUserByHabbo2.Coordinate, 0, room.GetGameMap().SqAbsoluteHeight(roomUserByHabbo2.X, roomUserByHabbo2.Y)));
            //room.GetRoomUserManager().UpdateUserStatus(roomUserByHabbo, false);

        }    }}