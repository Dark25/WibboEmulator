﻿namespace WibboEmulator.Games.Items.Wired.Triggers;
using System.Data;
using WibboEmulator.Database.Interfaces;
using WibboEmulator.Games.Items.Wired.Bases;
using WibboEmulator.Games.Items.Wired.Interfaces;
using WibboEmulator.Games.Rooms;
using WibboEmulator.Games.Rooms.Wired;

public class WalksOnFurni : WiredTriggerBase, IWired, IWiredCycleable
{
    public int DelayCycle { get => this.Delay; }

    private readonly UserAndItemDelegate delegateFunction;

    public WalksOnFurni(Item item, Room room) : base(item, room, (int)WiredTriggerType.AVATAR_WALKS_ON_FURNI) => this.delegateFunction = new UserAndItemDelegate(this.OnUserWalksOnFurni);

    public bool OnCycle(RoomUser user, Item item)
    {
        if (user != null)
        {
            this.RoomInstance.GetWiredHandler().ExecutePile(this.ItemInstance.Coordinate, user, item);
        }

        return false;
    }

    private void OnUserWalksOnFurni(RoomUser user, Item item)
    {
        if (this.DelayCycle > 0)
        {
            this.RoomInstance.GetWiredHandler().RequestCycle(new WiredCycle(this, user, item));
        }
        else
        {
            this.RoomInstance.GetWiredHandler().ExecutePile(this.ItemInstance.Coordinate, user, item);
        }
    }

    public override void LoadItems(bool inDatabase = false)
    {
        base.LoadItems();

        if (this.Items != null)
        {
            foreach (var roomItem in this.Items.ToList())
            {
                roomItem.OnUserWalksOnFurni += this.delegateFunction;
            }
        }
    }

    public override void Dispose()
    {
        if (this.Items != null)
        {
            foreach (var roomItem in this.Items.ToList())
            {
                roomItem.OnUserWalksOnFurni -= this.delegateFunction;
            }
        }

        base.Dispose();
    }

    public void SaveToDatabase(IQueryAdapter dbClient) => WiredUtillity.SaveTriggerItem(dbClient, this.ItemInstance.Id, string.Empty, this.DelayCycle.ToString(), false, this.Items);

    public void LoadFromDatabase(DataRow row)
    {
        if (int.TryParse(row["trigger_data"].ToString(), out var delay))
        {
            this.Delay = delay;
        }

        var triggerItems = row["triggers_item"].ToString();

        if (triggerItems is null or "")
        {
            return;
        }

        foreach (var itemId in triggerItems.Split(';'))
        {
            if (!int.TryParse(itemId, out var id))
            {
                continue;
            }

            if (!this.StuffIds.Contains(id))
            {
                this.StuffIds.Add(id);
            }
        }
    }
}
