﻿using Butterfly.Game.GameClients;
using Butterfly.Game.Rooms;

namespace Butterfly.Game.Items.Interactors
{
    public class InteractorFreezeTile : FurniInteractor
    {
        public override void OnPlace(GameClient Session, Item Item)
        {
        }

        public override void OnRemove(GameClient Session, Item Item)
        {
        }

        public override void OnTrigger(GameClient Session, Item Item, int Request, bool UserHasRights)
        {
            if (Session == null || Session.GetHabbo() == null || Item.InteractingUser > 0)
            {
                return;
            }

            string pName = Session.GetHabbo().Username;
            RoomUser roomUserByHabbo = Item.GetRoom().GetRoomUserManager().GetRoomUserByName(pName);
            if (roomUserByHabbo == null || roomUserByHabbo.CountFreezeBall == 0 || roomUserByHabbo.Freezed)
            {
                return;
            }

            Item.GetRoom().GetFreeze().throwBall(Item, roomUserByHabbo);
        }
    }
}