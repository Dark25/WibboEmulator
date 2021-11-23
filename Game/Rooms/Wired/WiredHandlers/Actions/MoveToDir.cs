﻿using Butterfly.Communication.Packets.Outgoing;
using Butterfly.Database.Interfaces;
using Butterfly.Game.Items;
using Butterfly.Game.Rooms.Map.Movement;
using Butterfly.Game.Rooms.Wired.WiredHandlers.Interfaces;
using System;
using System.Data;
using System.Drawing;

namespace Butterfly.Game.Rooms.Wired.WiredHandlers.Actions
{
    public class MoveToDir : WiredActionBase, IWiredEffect, IWired
    {
        private MovementDirection MoveToDirMovement;

        public MoveToDir(Item item, Room room) : base(item, room, (int)WiredActionType.MOVE_TO_DIRECTION)
        {
            this.MoveToDirMovement = MovementDirection.none;
        }

        public void Handle(RoomUser user, Item TriggerItem)
        {
            this.HandleItems();
        }

        private void HandleItems()
        {
            foreach (Item roomItem in this.Items.ToArray())
            {
                this.HandleMovement(roomItem);
            }
        }
        
        public override void LoadItems(bool inDatabase = false)
        {
            base.LoadItems();

            MovementDirection startDirection = (MovementDirection)((this.IntParams.Count > 0) ? this.IntParams[0] : 0);

            this.MoveToDirMovement = startDirection;
        }

        private void HandleMovement(Item item)
        {
            if (this.RoomInstance.GetRoomItemHandler().GetItem(item.Id) == null)
            {
                return;
            }

            Point newPoint = MovementManagement.HandleMovementDir(item.GetX, item.GetY, this.MoveToDirMovement);

            RoomUser roomUser = this.RoomInstance.GetRoomUserManager().GetUserForSquare(newPoint.X, newPoint.Y);
            if (roomUser != null)
            {
                this.RoomInstance.GetWiredHandler().TriggerCollision(roomUser, item);
                return;
            }

            MovementDirection startDirection = (MovementDirection)((this.IntParams.Count > 0) ? this.IntParams[0] : 0);
            WhenMovementBlock whenMoveIsBlocked = (WhenMovementBlock)((this.IntParams.Count > 1) ? this.IntParams[1] : 0);

            int OldX = item.GetX;
            int OldY = item.GetY;
            double OldZ = item.GetZ;
            if (this.RoomInstance.GetRoomItemHandler().SetFloorItem(null, item, newPoint.X, newPoint.Y, item.Rotation, false, false, false))
            {
                ServerPacket Message = new ServerPacket(ServerPacketHeader.ROOM_ROLLING);
                Message.WriteInteger(OldX);
                Message.WriteInteger(OldY);
                Message.WriteInteger(newPoint.X);
                Message.WriteInteger(newPoint.Y);
                Message.WriteInteger(1);
                Message.WriteInteger(item.Id);
                Message.WriteString(OldZ.ToString().Replace(',', '.'));
                Message.WriteString(item.GetZ.ToString().Replace(',', '.'));
                Message.WriteInteger(0);
                this.RoomInstance.SendPacket(Message);
                return;
            }

            switch (whenMoveIsBlocked)
            {
                #region None
                case WhenMovementBlock.none:
                    {
                        //this.movetodirMovement = MovementDirection.none;
                        break;
                    }
                #endregion
                #region Right45
                case WhenMovementBlock.right45:
                    {
                        if (this.MoveToDirMovement == MovementDirection.right)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.left)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.up)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.down)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.upleft)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.upright)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                            return;
                        }
                        else if (this.MoveToDirMovement == MovementDirection.downright)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.downleft)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                        }

                        break;
                    }
                #endregion
                #region Right90
                case WhenMovementBlock.right90:
                    {
                        if (this.MoveToDirMovement == MovementDirection.right)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.left)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.up)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.down)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.upleft)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.upright)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                            return;
                        }
                        else if (this.MoveToDirMovement == MovementDirection.downright)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.downleft)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                        }

                        break;
                    }
                #endregion
                #region Left45
                case WhenMovementBlock.left45:
                    {
                        if (this.MoveToDirMovement == MovementDirection.right)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.left)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.up)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.down)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.upleft)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.upright)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.downright)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.downleft)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                        }

                        break;
                    }
                #endregion
                #region Left90
                case WhenMovementBlock.left90:
                    {
                        if (this.MoveToDirMovement == MovementDirection.right)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.left)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.up)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.down)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY)) // derecha
                            {
                                this.MoveToDirMovement = MovementDirection.right;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY - 1)) // arriba
                            {
                                this.MoveToDirMovement = MovementDirection.up;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY)) // izq
                            {
                                this.MoveToDirMovement = MovementDirection.left;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX, item.GetY + 1)) // abajo
                            {
                                this.MoveToDirMovement = MovementDirection.down;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.upleft)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.upright)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.downright)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                        }
                        else if (this.MoveToDirMovement == MovementDirection.downleft)
                        {
                            if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY + 1)) // abajo derecha
                            {
                                this.MoveToDirMovement = MovementDirection.downright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX + 1, item.GetY - 1)) // arriba derecha
                            {
                                this.MoveToDirMovement = MovementDirection.upright;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY - 1)) // arriba izq
                            {
                                this.MoveToDirMovement = MovementDirection.upleft;
                                break;
                            }
                            else if (this.RoomInstance.GetGameMap().CanStackItem(item.GetX - 1, item.GetY + 1)) // abajo izq
                            {
                                this.MoveToDirMovement = MovementDirection.downleft;
                                break;
                            }
                        }

                        break;
                    }
                #endregion
                #region Turn Back
                case WhenMovementBlock.turnback:
                    {
                        if (this.MoveToDirMovement == MovementDirection.right)
                        {
                            this.MoveToDirMovement = MovementDirection.left;
                        }
                        else if (this.MoveToDirMovement == MovementDirection.left)
                        {
                            this.MoveToDirMovement = MovementDirection.right;
                        }
                        else if (this.MoveToDirMovement == MovementDirection.up)
                        {
                            this.MoveToDirMovement = MovementDirection.down;
                        }
                        else if (this.MoveToDirMovement == MovementDirection.down)
                        {
                            this.MoveToDirMovement = MovementDirection.up;
                        }
                        else if (this.MoveToDirMovement == MovementDirection.upright)
                        {
                            this.MoveToDirMovement = MovementDirection.downleft;
                        }
                        else if (this.MoveToDirMovement == MovementDirection.downleft)
                        {
                            this.MoveToDirMovement = MovementDirection.upright;
                        }
                        else if (this.MoveToDirMovement == MovementDirection.upleft)
                        {
                            this.MoveToDirMovement = MovementDirection.downright;
                        }
                        else if (this.MoveToDirMovement == MovementDirection.downright)
                        {
                            this.MoveToDirMovement = MovementDirection.upleft;
                        }
                        break;
                    }
                #endregion
                #region Random
                case WhenMovementBlock.turnrandom:
                    {
                        this.MoveToDirMovement = (MovementDirection)new Random().Next(1, 7);
                        break;
                    }
                    #endregion
            }

            newPoint = MovementManagement.HandleMovementDir(item.GetX, item.GetY, this.MoveToDirMovement);

            if (newPoint != item.Coordinate)
            {
                OldX = item.GetX;
                OldY = item.GetY;
                OldZ = item.GetZ;

                if (this.RoomInstance.GetRoomItemHandler().SetFloorItem(null, item, newPoint.X, newPoint.Y, item.Rotation, false, false, false))
                {
                    ServerPacket Message = new ServerPacket(ServerPacketHeader.ROOM_ROLLING);
                    Message.WriteInteger(OldX);
                    Message.WriteInteger(OldY);
                    Message.WriteInteger(newPoint.X);
                    Message.WriteInteger(newPoint.Y);
                    Message.WriteInteger(1);
                    Message.WriteInteger(item.Id);
                    Message.WriteString(OldZ.ToString().Replace(',', '.'));
                    Message.WriteString(item.GetZ.ToString().Replace(',', '.'));
                    Message.WriteInteger(0);
                    this.RoomInstance.SendPacket(Message);
                }
            }
            return;
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            MovementDirection startDirection = (MovementDirection)((this.IntParams.Count > 0) ? this.IntParams[0] : 0);
            WhenMovementBlock whenMoveIsBlocked = (WhenMovementBlock)((this.IntParams.Count > 1) ? this.IntParams[1] : 0);

            WiredUtillity.SaveTriggerItem(dbClient, this.Id, Convert.ToInt32(startDirection).ToString(), Convert.ToInt32(whenMoveIsBlocked).ToString(), false, this.Items);
        }

        public void LoadFromDatabase(DataRow row)
        {
            string triggerItems = row["triggers_item"].ToString();

            if (int.TryParse(row["trigger_data_2"].ToString(), out int startDirection))
                this.IntParams.Add(startDirection);

            if (int.TryParse(row["trigger_data"].ToString(), out int whenMoveIsBlocked))
                this.IntParams.Add(whenMoveIsBlocked);

            if (triggerItems == "")
            {
                return;
            }

            foreach (string itemId in triggerItems.Split(';'))
            {
                if (!int.TryParse(itemId, out int id))
                    continue;

                if(!this.StuffIds.Contains(id))
                    this.StuffIds.Add(id);
            }
        }
    }
}
