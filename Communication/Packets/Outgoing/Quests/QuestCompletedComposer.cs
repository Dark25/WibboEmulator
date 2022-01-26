using Butterfly.Game.Clients;
using Butterfly.Game.Quests;

namespace Butterfly.Communication.Packets.Outgoing.Quests
{
    internal class QuestCompletedComposer : ServerPacket
    {
        public QuestCompletedComposer(Client Session, Quest Quest)
            : base(ServerPacketHeader.QUEST_COMPLETED)
        {
            int questsInCategory = ButterflyEnvironment.GetGame().GetQuestManager().GetAmountOfQuestsInCategory(Quest.Category);
            int i = Quest.Number - 1;
            int num = Session.GetHabbo().GetQuestProgress(Quest.Id);
            if (Quest.IsCompleted(num))
            {
                i++;
            }

            this.WriteString(Quest.Category);
            this.WriteInteger(i);
            this.WriteInteger(questsInCategory);
            this.WriteInteger(0);
            this.WriteInteger(Quest.Id);
            this.WriteBoolean(Session.GetHabbo().CurrentQuestId == Quest.Id);
            this.WriteString(Quest.ActionName);
            this.WriteString(Quest.DataBit);
            this.WriteInteger(Quest.Reward);
            this.WriteString(Quest.Name);
            this.WriteInteger(num);
            this.WriteInteger(Quest.GoalData);
            this.WriteInteger(GetIntValue(Quest.Category));
            this.WriteString("set_kuurna");
            this.WriteString("MAIN_CHAIN");
            this.WriteBoolean(true);
            this.WriteBoolean(true);
        }

        private int GetIntValue(string QuestCategory)
        {
            switch (QuestCategory)
            {
                case "room_builder":
                    return 2;
                case "social":
                    return 3;
                case "identity":
                    return 4;
                case "explore":
                    return 5;
                case "battleball":
                    return 7;
                case "freeze":
                    return 8;
                default:
                    return 0;
            }
        }
    }
}
