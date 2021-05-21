﻿using Butterfly.Communication.Packets.Outgoing;
using Butterfly.HabboHotel.GameClients;
using Butterfly.HabboHotel.Rooms;

namespace Butterfly.Communication.Packets.Incoming.Structure
{
    internal class AnswerPollEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (room == null)
            {
                return;
            }

            RoomUser User = room.GetRoomUserManager().GetRoomUserByHabboId(Session.GetHabbo().Id);
            if (User == null)
            {
                return;
            }

            int Id = Packet.PopInt();
            int QuestionId = Packet.PopInt();

            int Count = Packet.PopInt();//Count

            string Value = "0";
            for (int i = 0; i < Count; i++)
            {
                Value = Packet.PopString();
            }

            Value = (Value != "0" && Value != "1") ? "0" : Value;

            if (Value == "0")
            {
                room.VotedNoCount++;
            }
            else
            {
                room.VotedYesCount++;
            }

            ServerPacket Message = new ServerPacket(ServerPacketHeader.RoomUserQuestionAnsweredComposer);
            Message.WriteInteger(Session.GetHabbo().Id);
            Message.WriteString(Value);
            Message.WriteInteger(2);

            Message.WriteString("0");
            Message.WriteInteger(room.VotedNoCount);
            Message.WriteString("1");
            Message.WriteInteger(room.VotedYesCount);
            room.SendPacket(Message);

            string WiredCode = (Value == "0") ? "QUESTION_NO" : "QUESTION_YES";
            if (room.AllowsShous(User, WiredCode))
            {
                User.SendWhisperChat(WiredCode, false);
            }
        }
    }
}