﻿using Butterfly.Game.Items;
namespace Butterfly.Game.Rooms.Wired.WiredHandlers.Interfaces
{
    public interface IWiredCycleable
    {
        bool OnCycle(RoomUser User, Item item);

        bool Disposed();

        int DelayCycle { get; set; }
    }
}
