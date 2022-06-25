using WibboEmulator.Database.Interfaces;

namespace WibboEmulator.Database.Daos
{
    class LogFlagmeDao
    {
        internal static void Insert(IQueryAdapter dbClient, int userId, string username, string newUsername)
        {
            dbClient.SetQuery("INSERT INTO `log_flagme` (user_id, oldusername, newusername, time) VALUES (@userid, @oldusername, @newusername, '" + WibboEnvironment.GetUnixTimestamp() + "')");
            dbClient.AddParameter("userid", userId);
            dbClient.AddParameter("oldusername", username);
            dbClient.AddParameter("newusername", newUsername);
            dbClient.RunQuery();
        }
    }
}