namespace WibboEmulator.Database.Daos.Catalog;
using System.Data;
using WibboEmulator.Database.Interfaces;

internal sealed class CatalogPageDao
{
    internal static DataTable GetAll(IQueryAdapter dbClient)
    {
        dbClient.SetQuery("SELECT `catalog_page`.id, `catalog_page`.parent_id, `catalog_page`.caption, `catalog_page`.page_link, `catalog_page`.enabled, `catalog_page`.required_right, `catalog_page`.icon_image," +
                                " `catalog_page`.page_layout, `catalog_page`.page_strings_1, `catalog_page`.page_strings_2, `catalog_page_langue`.caption_en, `catalog_page_langue`.caption_br," +
                                " `catalog_page_langue`.page_strings_2_en, `catalog_page_langue`.page_strings_2_br, `catalog_page`.is_premium" +
                                " FROM `catalog_page`" +
                                " LEFT JOIN `catalog_page_langue` ON `catalog_page`.id = `catalog_page_langue`.page_id" +
                                " ORDER BY order_num, caption");

        return dbClient.GetTable();
    }
}
