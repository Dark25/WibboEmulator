﻿using Butterfly.Communication.Packets.Outgoing;
using Butterfly.Database.Interfaces;
using Butterfly.Game.Clients;
using Butterfly.Game.Items;
using Butterfly.Game.Rooms.Games;
using Butterfly.Game.Rooms.Wired.WiredHandlers.Interfaces;
using System;
using System.Data;

namespace Butterfly.Game.Rooms.Wired.WiredHandlers.Triggers
{
    public class GameStarts : WiredTriggerBase, IWired
    {
        private readonly RoomEventDelegate gameStartsDeletgate;

        public GameStarts(Item item, Room room) : base(item, room, (int)WiredTriggerType.GAME_STARTS)
        {
            this.gameStartsDeletgate = new RoomEventDelegate(this.gameManager_OnGameStart);
            this.RoomInstance.GetGameManager().OnGameStart += this.gameStartsDeletgate;
        }

        private void gameManager_OnGameStart(object sender, EventArgs e)
        {
            this.RoomInstance.GetWiredHandler().ExecutePile(this.ItemInstance.Coordinate, null, null);
        }

        public override void Dispose()
        {
            base.Dispose();
            
            this.RoomInstance.GetWiredHandler().GetRoom().GetGameManager().OnGameStart -= this.gameStartsDeletgate;
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
        }

        public void LoadFromDatabase(DataRow row)
        {
        }
    }
}
