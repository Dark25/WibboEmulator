using Butterfly.Game.GameClients;
            {
                return;
            }

            RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabboId(Session.GetHabbo().Id);
            {
                return;
            }

            if (roomUserByHabbo.Statusses.ContainsKey("sit") || roomUserByHabbo.Statusses.ContainsKey("lay"))
            {
                return;
            }

            if (roomUserByHabbo.RotBody % 2 == 0)
                {
                    roomUserByHabbo.SetStatus("sit", "");
                }
                else
                {
                    roomUserByHabbo.SetStatus("sit", "0.5");
                }

                roomUserByHabbo.IsSit = true;