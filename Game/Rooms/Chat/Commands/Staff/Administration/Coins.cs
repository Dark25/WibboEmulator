using Butterfly.Game.GameClients;
                {
                    Session.SendNotification(ButterflyEnvironment.GetLanguageManager().TryGetValue("input.intonly", Session.Langue));
                }
            }
            {
                Session.SendNotification(ButterflyEnvironment.GetLanguageManager().TryGetValue("input.usernotfound", Session.Langue));
            }