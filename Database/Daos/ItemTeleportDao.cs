using System.Data;
using Butterfly.Database;
using Butterfly.Database.Interfaces;

namespace Butterfly.Database.Daos
{
    class ItemTeleportDao
    {
        internal static void Insert(IQueryAdapter dbClient, int item1Id, int item2Id)
        {
            dbClient.SetQuery("INSERT INTO tele_links (tele_one_id, tele_two_id) VALUES (" + item1Id + ", " + item2Id + "), (" + item2Id + ", " + item1Id + ")");
            dbClient.RunQuery();
        }

        internal static DataRow GetOne(IQueryAdapter dbClient, int teleId)
        {
            dbClient.SetQuery("SELECT tele_two_id FROM tele_links WHERE tele_one_id = '" + teleId + "'");
            return dbClient.GetRow();
        }

        internal static void InsertOne(IQueryAdapter dbClient, int newId, int newIdTwo)
        {
            dbClient.RunQuery("INSERT INTO tele_links (tele_one_id, tele_two_id) VALUES ('" + newId + "', '" + newIdTwo + "');");
        }
    }
}