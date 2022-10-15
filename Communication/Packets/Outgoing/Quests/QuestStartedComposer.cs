namespace WibboEmulator.Communication.Packets.Outgoing.Quests;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Quests;

internal class QuestStartedComposer : ServerPacket
{
    public QuestStartedComposer(GameClient session, Quest quest)
        : base(ServerPacketHeader.QUEST)
    {
        var questsInCategory = WibboEnvironment.GetGame().GetQuestManager().GetAmountOfQuestsInCategory(quest.Category);
        var i = quest == null ? questsInCategory : quest.Number - 1;
        var num = quest == null ? 0 : session.User.GetQuestProgress(quest.Id);
        if (quest != null && quest.IsCompleted(num))
        {
            i++;
        }

        this.WriteString(quest?.Category ?? string.Empty);
        this.WriteInteger(i);
        this.WriteInteger(questsInCategory);
        this.WriteInteger(0);
        this.WriteInteger(quest == null ? 0 : quest.Id);
        this.WriteBoolean(quest != null && session.User.CurrentQuestId == quest.Id);
        this.WriteString(quest == null ? string.Empty : quest.ActionName);
        this.WriteString(quest == null ? string.Empty : quest.DataBit);
        this.WriteInteger(quest == null ? 0 : quest.Reward);
        this.WriteString(quest == null ? string.Empty : quest.Name);
        this.WriteInteger(num);
        this.WriteInteger(quest == null ? 0 : quest.GoalData);
        this.WriteInteger(QuestTypeUtillity.GetIntValue(quest?.Category ?? ""));
        this.WriteString("set_kuurna");
        this.WriteString("MAIN_CHAIN");
        this.WriteBoolean(true);
    }
}
