using Butterfly.Communication.Packets.Outgoing.Rooms.Engine;
using Butterfly.HabboHotel.GameClients;

namespace Butterfly.HabboHotel.Rooms.Chat.Commands.Cmd
            {
                return;
            }

            if (!UserRoom.transformation && !UserRoom.IsSpectator)

                    RoomClient.SendPacket(new UserRemoveComposer(UserRoom.VirtualId));