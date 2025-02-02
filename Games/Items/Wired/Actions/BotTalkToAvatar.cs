namespace WibboEmulator.Games.Items.Wired.Actions;

using WibboEmulator.Communication.Packets.Outgoing.RolePlay;
using WibboEmulator.Communication.Packets.Outgoing.Rooms.Chat;
using WibboEmulator.Database.Interfaces;
using WibboEmulator.Games.Items.Wired.Bases;
using WibboEmulator.Games.Items.Wired.Interfaces;
using WibboEmulator.Games.Rooms;
using WibboEmulator.Games.Rooms.AI;

public class BotTalkToAvatar : WiredActionBase, IWired, IWiredEffect
{
    public BotTalkToAvatar(Item item, Room room) : base(item, room, (int)WiredActionType.BOT_TALK_DIRECT_TO_AVTR) => this.DefaultIntParams(new int[] { 0 });

    public override bool OnCycle(RoomUser user, Item item)
    {
        if (!this.StringParam.Contains('\t'))
        {
            return false;
        }

        var splitData = this.StringParam.Split('\t');

        var name = splitData[0].ToString();
        var message = splitData[1].ToString();

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(message) || user == null || user.Client == null)
        {
            return false;
        }

        var bot = this.RoomInstance.RoomUserManager.GetBotOrPetByName(name);
        if (bot == null || bot.BotData == null)
        {
            return false;
        }

        var isWhisper = this.GetIntParam(0) == 1;

        WiredUtillity.ParseMessage(user, this.RoomInstance, ref message);

        if (isWhisper && message.Contains(" : ") && (this.RoomInstance.IsRoleplay || this.RoomInstance.RoomData.OwnerName == "LieuPublic"))
        {
            SendBotChoose(message, user, bot.BotData);
        }

        if (isWhisper)
        {
            user.Client.SendPacket(new WhisperComposer(bot.VirtualId, message, 2));
        }
        else
        {
            user.Client.SendPacket(new ChatComposer(bot.VirtualId, message, 2));
        }

        return false;
    }

    private static void SendBotChoose(string textMessage, RoomUser user, RoomBot botData)
    {
        var splitText = textMessage.Split(new[] { " : " }, StringSplitOptions.None);
        if (splitText.Length != 2)
        {
            return;
        }

        var chooseList = new List<string[]>
        {
            new List<string>
            {
                botData.Name,
                splitText[0],
                splitText[1],
                botData.Look
            }.ToArray()
        };

        user.Client.SendPacket(new BotChooseComposer(chooseList));
    }

    public void SaveToDatabase(IQueryAdapter dbClient)
    {
        var isWhisper = this.GetIntParam(0) == 1;

        WiredUtillity.SaveTriggerItem(dbClient, this.Id, string.Empty, this.StringParam, isWhisper, null, this.Delay);
    }

    public void LoadFromDatabase(string wiredTriggerData, string wiredTriggerData2, string wiredTriggersItem, bool wiredAllUserTriggerable, int wiredDelay)
    {
        this.Delay = wiredDelay;

        this.SetIntParam(0, wiredAllUserTriggerable ? 1 : 0);

        this.StringParam = wiredTriggerData;
    }
}
