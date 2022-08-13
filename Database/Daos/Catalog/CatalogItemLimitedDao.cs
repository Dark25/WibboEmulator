using WibboEmulator.Database.Interfaces;
using System.Data;

namespace WibboEmulator.Database.Daos
{
    class CatalogItemLimitedDao
    {
        internal static void Update(IQueryAdapter dbClient, int itemId, int limitedEditionSells)
        {
            dbClient.SetQuery("UPDATE `catalog_item_limited` SET `limited_sells` = @limitSells WHERE `catalog_item_id` = @itemId LIMIT 1");
            dbClient.AddParameter("limitSells", limitedEditionSells);
            dbClient.AddParameter("itemId", itemId);
            dbClient.RunQuery();
        }
    }
}