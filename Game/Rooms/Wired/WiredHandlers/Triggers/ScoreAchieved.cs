﻿using Butterfly.Communication.Packets.Outgoing;
using Butterfly.Database.Interfaces;
using Butterfly.Game.Clients;
using Butterfly.Game.Items;
using Butterfly.Game.Rooms.Games;
using Butterfly.Game.Rooms.Wired.WiredHandlers.Interfaces;
using System.Data;

namespace Butterfly.Game.Rooms.Wired.WiredHandlers.Triggers
{
    public class ScoreAchieved : WiredTriggerBase, IWired
    {
        private readonly TeamScoreChangedDelegate scoreChangedDelegate;

        public ScoreAchieved(Item item, Room room) : base(item, room, (int)WiredTriggerType.SCORE_ACHIEVED)
        {
            this.scoreChangedDelegate = new TeamScoreChangedDelegate(this.gameManager_OnScoreChanged);
            this.RoomInstance.GetGameManager().OnScoreChanged += this.scoreChangedDelegate;
        }

        private void gameManager_OnScoreChanged(object sender, TeamScoreChangedArgs e)
        {
            int scoreLevel = ((this.IntParams.Count > 0) ? this.IntParams[0] : 0);
            if (e.Points <= scoreLevel - 1)
            {
                return;
            }

            this.RoomInstance.GetWiredHandler().ExecutePile(this.ItemInstance.Coordinate, e.user, null);
        }

        public override void Dispose()
        {
            this.RoomInstance.GetWiredHandler().GetRoom().GetGameManager().OnScoreChanged -= this.scoreChangedDelegate;

            base.Dispose();
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            int scoreLevel = ((this.IntParams.Count > 0) ? this.IntParams[0] : 0);
            WiredUtillity.SaveTriggerItem(dbClient, this.Id, string.Empty, scoreLevel.ToString(), false, null);
        }

        public void LoadFromDatabase(DataRow row)
        {
            if (int.TryParse(row["trigger_data"].ToString(), out int score))
                this.IntParams.Add(score);
        }
    }
}
