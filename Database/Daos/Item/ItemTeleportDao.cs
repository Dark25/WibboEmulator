namespace WibboEmulator.Database.Daos.Item;
using System.Data;
using WibboEmulator.Database.Interfaces;

internal sealed class ItemTeleportDao
{
    internal static DataRow GetOne(IQueryAdapter dbClient, int teleId)
    {
        dbClient.SetQuery("SELECT tele_two_id FROM `item_teleport` WHERE tele_one_id = '" + teleId + "'");
        return dbClient.GetRow();
    }

    internal static void Insert(IQueryAdapter dbClient, int newId, int newIdTwo) => dbClient.RunQuery("INSERT INTO `item_teleport` (tele_one_id, tele_two_id) VALUES ('" + newId + "', '" + newIdTwo + "')");
}