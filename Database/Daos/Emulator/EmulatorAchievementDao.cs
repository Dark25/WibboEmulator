namespace WibboEmulator.Database.Daos.Emulator;
using System.Data;
using WibboEmulator.Database.Interfaces;

internal sealed class EmulatorAchievementDao
{
    internal static DataTable GetAll(IQueryAdapter dbClient)
    {
        dbClient.SetQuery("SELECT id, category, group_name, level, reward_pixels, reward_points, progress_needed FROM `emulator_achievement`");
        return dbClient.GetTable();
    }
}