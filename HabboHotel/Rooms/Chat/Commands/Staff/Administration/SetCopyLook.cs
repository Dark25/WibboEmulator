using Butterfly.Communication.Packets.Outgoing.Rooms.Engine;
using Butterfly.HabboHotel.GameClients;
            {
                return;
            }

            string username = Params[1];
            {
                return;
            }

            Room currentRoom = roomUserByHabbo.Room;
            {
                return;
            }

            clientByUsername.GetHabbo().Gender = Session.GetHabbo().Gender;
            {
                return;
            }

            if (!clientByUsername.GetHabbo().InRoom)
            {
                return;
            }

            clientByUsername.SendPacket(new UserChangeComposer(roomUserByHabbo, true));