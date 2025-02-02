namespace WibboEmulator.Communication.Packets.Incoming.Rooms.AI.Bots;
using WibboEmulator.Communication.Packets.Outgoing.Rooms.Avatar;
using WibboEmulator.Communication.Packets.Outgoing.Rooms.Engine;
using WibboEmulator.Database.Daos.Bot;
using WibboEmulator.Games.GameClients;

internal sealed class SaveBotActionEvent : IPacketEvent
{
    public double Delay => 250;

    public void Parse(GameClient session, ClientPacket packet)
    {
        if (session.User == null || !session.User.InRoom)
        {
            return;
        }

        var room = session.User.CurrentRoom;
        if (room == null || !room.CheckRights(session, true))
        {
            return;
        }

        var botId = packet.PopInt();
        var actionId = packet.PopInt();
        var dataString = packet.PopString(2000);

        if (botId <= 0)
        {
            return;
        }

        if (actionId is < 1 or > 5)
        {
            return;
        }

        if (!room.RoomUserManager.TryGetBot(botId, out var bot))
        {
            return;
        }

        var roomBot = bot.BotData;
        if (roomBot == null)
        {
            return;
        }

        /* 1 = Copy looks
         * 2 = Setup Speech
         * 3 = Relax
         * 4 = Dance
         * 5 = Change Name
         */

        switch (actionId)
        {
            case 1:
            {
                //Change the defaults
                bot.BotData.Look = session.User.Look;
                bot.BotData.Gender = session.User.Gender;

                room.SendPacket(new UserChangeComposer(bot));

                using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
                BotUserDao.UpdateLookGender(dbClient, bot.BotData.Id, session.User.Gender, session.User.Look);
                break;
            }

            case 2:
            {
                var configData = dataString.Split(";#;", StringSplitOptions.None);

                if (configData.Length != 4)
                {
                    return;
                }

                var speechData = configData[0].Split(new char[]
                {
                        '\r',
                        '\n'
                }, StringSplitOptions.RemoveEmptyEntries);

                var automaticChat = Convert.ToString(configData[1]);
                var speakingIntervalIsInt = int.TryParse(configData[2], out var speakingInterval);
                var mixChat = Convert.ToString(configData[3]);

                if (speakingInterval <= 0 || speakingInterval < 7 || !speakingIntervalIsInt)
                {
                    speakingInterval = 7;
                }

                roomBot.AutomaticChat = Convert.ToBoolean(automaticChat);
                roomBot.SpeakingInterval = speakingInterval;
                roomBot.MixSentences = Convert.ToBoolean(mixChat);

                var text = "";
                for (var i = 0; i <= speechData.Length - 1; i++)
                {
                    var line = speechData[i];
                    if (line.Length > 150)
                    {
                        line = line[..150];
                    }

                    text += line + "\r";
                }

                roomBot.ChatText = text;
                roomBot.LoadRandomSpeech(text);

                using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
                BotUserDao.UpdateChat(dbClient, botId, roomBot.AutomaticChat, roomBot.SpeakingInterval, roomBot.MixSentences, roomBot.ChatText);

                break;
            }

            case 3:
            {
                bot.BotData.WalkingEnabled = !bot.BotData.WalkingEnabled;
                using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
                BotUserDao.UpdateWalkEnabled(dbClient, bot.BotData.Id, bot.BotData.WalkingEnabled);
                break;
            }

            case 4:
            {
                if (bot.DanceId > 0)
                {
                    bot.DanceId = 0;
                    bot.BotData.IsDancing = false;
                }
                else
                {
                    bot.DanceId = WibboEnvironment.GetRandomNumber(1, 4);
                    bot.BotData.IsDancing = true;
                }

                room.SendPacket(new DanceComposer(bot.VirtualId, bot.DanceId));

                using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
                BotUserDao.UpdateIsDancing(dbClient, bot.BotData.Id, bot.BotData.IsDancing);

                break;
            }

            case 5:
            {
                if (dataString.Length == 0)
                {
                    return;
                }
                else if (dataString.Length >= 16)
                {
                    return;
                }

                bot.BotData.Name = dataString;

                using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
                BotUserDao.UpdateName(dbClient, bot.BotData.Id, dataString);

                room.SendPacket(new UserNameChangeComposer(bot.BotData.Name, bot.VirtualId));
                break;
            }
        }
    }
}
