using Butterfly.Communication.Packets.Outgoing.Rooms.Engine;
using Butterfly.HabboHotel.GameClients;
            {
                return;
            }

            GameClient clientByUsername = roomUserByHabbo.GetClient();
            {
                return;
            }

            if (clientByUsername.GetHabbo().SpectatorMode)
            {
                return;
            }

            if (Params.Length != 4 && Params.Length != 3)
            {
                return;
            }

            Room RoomClient = roomUserByHabbo.GetClient().GetHabbo().CurrentRoom;
            {
                return;
            }

            int raceid = 0;
                    int.TryParse(Params[2], out raceid);
            RoomClient.SendPacket(new UsersComposer(roomUserByHabbo));