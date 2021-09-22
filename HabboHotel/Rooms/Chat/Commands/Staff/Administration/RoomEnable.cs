using Butterfly.HabboHotel.GameClients;
            {
                return;
            }

            if (!ButterflyEnvironment.GetGame().GetEffectManager().HaveEffect(NumEnable, Session.GetHabbo().HasFuse("fuse_sysadmin")))
            {
                return;
            }

            foreach (RoomUser User in Room.GetRoomUserManager().GetUserList().ToList())
                if (!User.IsBot)
                {
                    User.ApplyEffect(NumEnable);
                }