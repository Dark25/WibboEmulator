using System.Data;
using WibboEmulator.Database.Interfaces;

namespace WibboEmulator.Database.Daos
{
    class NavigatorPublicDao
    {
        internal static DataTable GetAll(IQueryAdapter dbClient)
        {
            dbClient.SetQuery("SELECT room_id, image_url, enabled, langue, category_type FROM `navigator_public` ORDER BY order_num ASC");
            return dbClient.GetTable();
        }
    }
}