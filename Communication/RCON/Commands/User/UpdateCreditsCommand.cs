namespace WibboEmulator.Communication.RCON.Commands.User;
using WibboEmulator.Communication.Packets.Outgoing.Inventory.Purse;
using WibboEmulator.Database.Daos.User;

internal sealed class UpdateCreditsCommand : IRCONCommand
{
    public bool TryExecute(string[] parameters)
    {
        if (parameters.Length != 2)
        {
            return false;
        }

        if (!int.TryParse(parameters[1], out var userId))
        {
            return false;
        }

        if (userId == 0)
        {
            return false;
        }

        var client = WibboEnvironment.GetGame().GetGameClientManager().GetClientByUserID(userId);
        if (client == null)
        {
            return false;
        }

        int credits;
        using (var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor())
        {
            credits = UserDao.GetCredits(dbClient, client.User.Id);
        }

        client.User.Credits = credits;
        client.SendPacket(new CreditBalanceComposer(client.User.Credits));

        return true;
    }
}
