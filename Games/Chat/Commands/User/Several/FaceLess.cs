namespace WibboEmulator.Games.Chat.Commands.User.Several;
using WibboEmulator.Communication.Packets.Outgoing.Rooms.Engine;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Rooms;

internal class FaceLess : IChatCommand
{
    public void Execute(GameClient session, Room Room, RoomUser UserRoom, string[] parameters)
    {
        if (UserRoom.IsTransf || UserRoom.IsSpectator)
        {
            return;
        }

        var look = session.GetUser().Look;

        if (look.Contains("hd-"))
        {
            var hdlook = look.Split(new string[] { "hd-" }, StringSplitOptions.None)[1];
            var hdcode = "hd-" + hdlook.Split(new char[] { '.' })[0]; //ex : hd-180-22
            var hdcodecolor = "";
            if (hdcode.Split('-').Length == 3)
            {
                hdcodecolor = hdcode.Split('-')[2];
            }

            var hdcodenoface = "hd-99999-" + hdcodecolor; //hd-9999-22

            look = look.Replace(hdcode, hdcodenoface);

            session.GetUser().Look = look;

            Room.SendPacket(new UserChangeComposer(UserRoom, false));
        }
    }
}
