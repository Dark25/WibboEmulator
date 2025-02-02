namespace WibboEmulator.Games.Roleplays.Weapon;
using System.Data;
using WibboEmulator.Database.Daos.Roleplay;
using WibboEmulator.Database.Interfaces;
using WibboEmulator.Utilities;

public class RPWeaponManager
{
    private readonly Dictionary<int, RPWeapon> _weaponCac;
    private readonly Dictionary<int, RPWeapon> _weaponGun;

    public RPWeaponManager()
    {
        this._weaponCac = new Dictionary<int, RPWeapon>();
        this._weaponGun = new Dictionary<int, RPWeapon>();
    }

    public RPWeapon GetWeaponCac(int id)
    {
        var weapon = new RPWeapon(0, 1, 3, RPWeaponInteraction.None, 0, 1, 1);
        if (!this._weaponCac.ContainsKey(id))
        {
            return weapon;
        }

        _ = this._weaponCac.TryGetValue(id, out weapon);
        return weapon;
    }

    public RPWeapon GetWeaponGun(int id)
    {
        var weapon = new RPWeapon(0, 5, 10, RPWeaponInteraction.None, 164, 3, 10);
        if (!this._weaponGun.ContainsKey(id))
        {
            return weapon;
        }

        _ = this._weaponGun.TryGetValue(id, out weapon);
        return weapon;
    }

    public void Init(IQueryAdapter dbClient)
    {
        this._weaponCac.Clear();
        this._weaponGun.Clear();

        var table = RoleplayWeaponDao.GetAll(dbClient);
        if (table != null)
        {
            foreach (DataRow dataRow in table.Rows)
            {
                if (this._weaponCac.ContainsKey(Convert.ToInt32(dataRow["id"])) || this._weaponGun.ContainsKey(Convert.ToInt32(dataRow["id"])))
                {
                    continue;
                }

                if ((string)dataRow["type"] == "cac")
                {
                    this._weaponCac.Add(Convert.ToInt32(dataRow["id"]), new RPWeapon(Convert.ToInt32(dataRow["id"]), Convert.ToInt32(dataRow["domage_min"]), Convert.ToInt32(dataRow["domage_max"]), ((string)dataRow["interaction"]).ToEnum(RPWeaponInteraction.None), Convert.ToInt32(dataRow["enable"]), Convert.ToInt32(dataRow["freeze_time"]), Convert.ToInt32(dataRow["distance"])));
                }
                else
                {
                    this._weaponGun.Add(Convert.ToInt32(dataRow["id"]), new RPWeapon(Convert.ToInt32(dataRow["id"]), Convert.ToInt32(dataRow["domage_min"]), Convert.ToInt32(dataRow["domage_max"]), ((string)dataRow["interaction"]).ToEnum(RPWeaponInteraction.None), Convert.ToInt32(dataRow["enable"]), Convert.ToInt32(dataRow["freeze_time"]), Convert.ToInt32(dataRow["distance"])));
                }
            }
        }
    }
}
