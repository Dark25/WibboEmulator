using Butterfly.Communication.Packets.Outgoing;
using Butterfly.Game.Clients;
using Butterfly.Game.Users;
using Butterfly.Game.Users.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Butterfly.Communication.Packets.Incoming.Structure
{
    internal class GetRelationshipsEvent : IPacketEvent
    {
        public void Parse(Client Session, ClientPacket Packet)
        {
            int Id = Packet.PopInt();

            Client Client = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(Id);

            if (Client == null || Client.GetHabbo() == null)
            {
                ServerPacket ResponseOff = new ServerPacket(ServerPacketHeader.MESSENGER_RELATIONSHIPS);
                ResponseOff.WriteInteger(Id);
                ResponseOff.WriteInteger(0);
                Session.SendPacket(ResponseOff);
                return;
            }

            User habbo = Client.GetHabbo();
            if (habbo == null || habbo.GetMessenger() == null)
            {
                ServerPacket ResponseOff = new ServerPacket(ServerPacketHeader.MESSENGER_RELATIONSHIPS);
                ResponseOff.WriteInteger(Id);
                ResponseOff.WriteInteger(0);
                Session.SendPacket(ResponseOff);
                return;
            }

            Dictionary<int, Relationship> Loves = habbo.GetMessenger().Relation.Where(x => x.Value.Type == 1).ToDictionary(item => item.Key, item => item.Value);
            Dictionary<int, Relationship> Likes = habbo.GetMessenger().Relation.Where(x => x.Value.Type == 2).ToDictionary(item => item.Key, item => item.Value);
            Dictionary<int, Relationship> Hates = habbo.GetMessenger().Relation.Where(x => x.Value.Type == 3).ToDictionary(item => item.Key, item => item.Value);
            int Nbrela = 0;
            if (Loves.Count > 0)
            {
                Nbrela++;
            }

            if (Likes.Count > 0)
            {
                Nbrela++;
            }

            if (Hates.Count > 0)
            {
                Nbrela++;
            }

            ServerPacket Response = new ServerPacket(ServerPacketHeader.MESSENGER_RELATIONSHIPS);
            Response.WriteInteger(habbo.Id);
            Response.WriteInteger(Nbrela); // Count //Habbo.Relationships.Count

            if (Loves.Count > 0)
            {
                //Loves
                Response.WriteInteger(1); //type
                Response.WriteInteger(Loves.Count); //Total personne love

                Random randlove = new Random();
                int useridlove = Loves.ElementAt(randlove.Next(0, Loves.Count)).Value.UserId;//Loves[randlove.Next(Loves.Count)].UserId;
                User HHablove = ButterflyEnvironment.GetHabboById(Convert.ToInt32(useridlove));
                Response.WriteInteger(useridlove); // UserId
                Response.WriteString((HHablove == null) ? "" : HHablove.Username);
                Response.WriteString((HHablove == null) ? "" : HHablove.Look); // look
            }
            if (Likes.Count > 0)
            {
                //Likes
                Response.WriteInteger(2); //type
                Response.WriteInteger(Likes.Count); //Total personne Like

                Random randLikes = new Random();
                int useridLikes = Likes.ElementAt(randLikes.Next(0, Likes.Count)).Value.UserId;//Likes[randLikes.Next(Likes.Count)].UserId;
                User HHabLikes = ButterflyEnvironment.GetHabboById(Convert.ToInt32(useridLikes));
                Response.WriteInteger(useridLikes); // UserId
                Response.WriteString((HHabLikes == null) ? "" : HHabLikes.Username);
                Response.WriteString((HHabLikes == null) ? "" : HHabLikes.Look); // look
            }
            if (Hates.Count > 0)
            {
                //Hates
                Response.WriteInteger(3); //type
                Response.WriteInteger(Hates.Count); //Total personne hates

                Random randHates = new Random();
                int useridHates = Hates.ElementAt(randHates.Next(0, Hates.Count)).Value.UserId;//Hates[randHates.Next(Hates.Count)].UserId;
                User HHabHates = ButterflyEnvironment.GetHabboById(Convert.ToInt32(useridHates));
                Response.WriteInteger(useridHates); // UserId
                Response.WriteString((HHabHates == null) ? "" : HHabHates.Username);
                Response.WriteString((HHabHates == null) ? "" : HHabHates.Look); // look
            }
            Session.SendPacket(Response);
        }
    }
}