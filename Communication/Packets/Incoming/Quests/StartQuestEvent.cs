namespace WibboEmulator.Communication.Packets.Incoming.Quests;
using WibboEmulator.Games.GameClients;

internal sealed class StartQuestEvent : IPacketEvent
{
    public double Delay => 0;

    public void Parse(GameClient session, ClientPacket packet)
    {
        var questId = packet.PopInt();

        WibboEnvironment.GetGame().GetQuestManager().ActivateQuest(session, questId);
    }
}
