using Butterfly.Communication.Packets.Outgoing.Rooms.Engine;
using Butterfly.Game.Clients;
using Butterfly.Game.Rooms;

namespace Butterfly.Game.Chat.Commands.Cmd
{
    internal class SetTransf : IChatCommand
    {
        public void Execute(Client Session, Room Room, RoomUser UserRoom, string[] Params)
        {
            string username = Params[1];

            RoomUser roomUserByHabbo = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByName(username);
            if (roomUserByHabbo == null)
            {
                return;
            }

            Client clientByUsername = roomUserByHabbo.GetClient();
            if (clientByUsername == null)
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
            if (RoomClient == null)
            {
                return;
            }

            int raceid = 0;
            if (Params.Length == 4)
            {
                string x = Params[3];
                if (int.TryParse(x, out int value))
                {
                    int.TryParse(Params[2], out raceid);
                    if (raceid < 1 || raceid > 50)
                    {
                        raceid = 0;
                    }
                }
            }
            else
            {
                raceid = 0;
            }

            if (!roomUserByHabbo.SetPetTransformation(Params[2], raceid))
            {
                Session.SendHugeNotif(ButterflyEnvironment.GetLanguageManager().TryGetValue("cmd.transf.help", Session.Langue));
                return;
            }

            roomUserByHabbo.SendWhisperChat(ButterflyEnvironment.GetLanguageManager().TryGetValue("cmd.transf.helpstop", Session.Langue));

            roomUserByHabbo.transformation = true;

            RoomClient.SendPacket(new UserRemoveComposer(roomUserByHabbo.VirtualId));
            RoomClient.SendPacket(new UsersComposer(roomUserByHabbo));

        }
    }
}
