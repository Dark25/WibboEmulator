using Butterfly.HabboHotel.GameClients;
            {
                return;
            }

            GameClient clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            {
                return;
            }
                {
                    Raison = CommandManager.MergeParams(Params, 2);
                }

                ButterflyEnvironment.GetGame().GetClientManager().BanUser(clientByUsername, Session.GetHabbo().Username, 788922000, Raison, true, false);

                if (clientByUsername.GetHabbo().Rank > 5 && Session.GetHabbo().Rank < 12)
                {
                    ButterflyEnvironment.GetGame().GetClientManager().BanUser(Session, "Robot", 788922000, "Votre compte � �t� banni par s�curit�", false, false);
                }