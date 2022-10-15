namespace WibboEmulator.Games.Items.Wired.Actions;

using System.Data;
using WibboEmulator.Communication.Packets.Outgoing.RolePlay;
using WibboEmulator.Communication.Packets.Outgoing.Rooms.Chat;
using WibboEmulator.Database.Interfaces;
using WibboEmulator.Games.Items.Wired.Bases;
using WibboEmulator.Games.Items.Wired.Interfaces;
using WibboEmulator.Games.Rooms;
using WibboEmulator.Games.Rooms.AI;

public class BotTalkToAvatar : WiredActionBase, IWired, IWiredEffect
{
    public BotTalkToAvatar(Item item, Room room) : base(item, room, (int)WiredActionType.BOT_TALK_DIRECT_TO_AVTR) => this.IntParams.Add(0);

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

        var isWhisper = ((this.IntParams.Count > 0) ? this.IntParams[0] : 0) == 1;

        var textMessage = message;
        textMessage = textMessage.Replace("#username#", user.GetUsername());
        textMessage = textMessage.Replace("#point#", user.WiredPoints.ToString());
        textMessage = textMessage.Replace("#roomname#", this.RoomInstance.RoomData.Name.ToString());
        textMessage = textMessage.Replace("#vote_yes#", this.RoomInstance.VotedYesCount.ToString());
        textMessage = textMessage.Replace("#vote_no#", this.RoomInstance.VotedNoCount.ToString());
        textMessage = textMessage.Replace("#wpcount#", user.Client.User != null ? user.Client.User.WibboPoints.ToString() : "0");

        if (user.Roleplayer != null)
        {
            textMessage = textMessage.Replace("#money#", user.Roleplayer.Money.ToString());
        }

        if (isWhisper && textMessage.Contains(" : ") && (this.RoomInstance.IsRoleplay || this.RoomInstance.RoomData.OwnerName == "LieuPublic"))
        {
            SendBotChoose(textMessage, user, bot.BotData);
        }

        if (isWhisper)
        {
            user.Client.SendPacket(new WhisperComposer(bot.VirtualId, textMessage, 2));
        }
        else
        {
            user.Client.SendPacket(new ChatComposer(bot.VirtualId, textMessage, 2));
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
        var isWhisper = ((this.IntParams.Count > 0) ? this.IntParams[0] : 0) == 1;

        WiredUtillity.SaveTriggerItem(dbClient, this.Id, string.Empty, this.StringParam, isWhisper, null, this.Delay);
    }

    public void LoadFromDatabase(DataRow row)
    {
        this.IntParams.Clear();

        if (int.TryParse(row["delay"].ToString(), out var delay))
        {
            this.Delay = delay;
        }

        if (bool.TryParse(row["all_user_triggerable"].ToString(), out var isWhisper))
        {
            this.IntParams.Add(isWhisper ? 1 : 0);
        }

        var data = row["trigger_data"].ToString();

        if (string.IsNullOrWhiteSpace(data) || !data.Contains('\t'))
        {
            return;
        }

        this.StringParam = data;
    }
}
