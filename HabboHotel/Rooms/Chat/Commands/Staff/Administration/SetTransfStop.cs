using Butterfly.Communication.Packets.Outgoing.Rooms.Engine;
using Butterfly.HabboHotel.GameClients;
            {
                return;
            }

            string username = Params[1];
            {
                return;
            }

            if (roomUserByHabbo.transformation && !roomUserByHabbo.IsSpectator)
                    RoomClient.SendPacket(new UsersComposer(roomUserByHabbo));