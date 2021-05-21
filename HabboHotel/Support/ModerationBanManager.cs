﻿using Butterfly.Database.Interfaces;
using Butterfly.HabboHotel.GameClients;

namespace Butterfly.HabboHotel.Support
{
    public class ModerationBanManager
    {
        public void BanUser(GameClient Client, string Moderator, double LengthSeconds, string Reason, bool IpBan, bool MachineBan)
        {
            if (string.IsNullOrEmpty(Reason))
            {
                Reason = "Ne respect pas les régles";
            }

            string Variable = Client.GetHabbo().Username.ToLower();
            string typeBan = "user";
            double Expire = ButterflyEnvironment.GetUnixTimestamp() + LengthSeconds;
            if (IpBan)
            {
                //Variable = Client.GetConnection().getIp();
                Variable = Client.GetHabbo().IP;
                typeBan = "ip";
            }

            if (MachineBan)
            {
                Variable = Client.MachineId;
                typeBan = "machine";
            }

            using (IQueryAdapter queryreactor = ButterflyEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                queryreactor.SetQuery("INSERT INTO bans (bantype,value,reason,expire,added_by,added_date) VALUES (@rawvar, @var, @reason, '" + Expire + "', @mod, UNIX_TIMESTAMP())");
                queryreactor.AddParameter("rawvar", typeBan);
                queryreactor.AddParameter("var", Variable);
                queryreactor.AddParameter("reason", Reason);
                queryreactor.AddParameter("mod", Moderator);
                queryreactor.RunQuery();
            }
            if (MachineBan)
            {
                this.BanUser(Client, Moderator, LengthSeconds, Reason, true, false);
            }
            else if (IpBan)
            {
                this.BanUser(Client, Moderator, LengthSeconds, Reason, false, false);
            }
            else
            {
                Client.Disconnect();
            }
        }
    }
}
