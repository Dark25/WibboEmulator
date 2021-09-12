﻿using Butterfly.HabboHotel.Items;
using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Butterfly.HabboHotel.Rooms.Wired
{
    public class WiredHandler
    {
        private readonly ConcurrentDictionary<Point, List<Item>> ActionStacks;
        private readonly ConcurrentDictionary<Point, List<Item>> ConditionStacks;

        private readonly ConcurrentDictionary<Point, List<RoomUser>> _wiredUsed;

        private readonly List<Point> SpecialRandom;
        private readonly Dictionary<Point, int> SpecialUnseen;

        private ConcurrentQueue<WiredCycle> _requestingUpdates;

        private readonly Room _room;
        private bool _doCleanup = false;

        public event BotCollisionDelegate TrgBotCollision;
        public event UserAndItemDelegate TrgCollision;
        public event RoomEventDelegate TrgTimer;

        public WiredHandler(Room room)
        {
            this.ActionStacks = new ConcurrentDictionary<Point, List<Item>>();
            this.ConditionStacks = new ConcurrentDictionary<Point, List<Item>>();
            this._requestingUpdates = new ConcurrentQueue<WiredCycle>();
            this._wiredUsed = new ConcurrentDictionary<Point, List<RoomUser>>();


            this.SpecialRandom = new List<Point>();
            this.SpecialUnseen = new Dictionary<Point, int>();

            this._room = room;
        }

        public void AddFurniture(Item item)
        {
            Point itemCoord = item.Coordinate;
            if (WiredUtillity.TypeIsWiredAction(item.GetBaseItem().InteractionType))
            {
                if (this.ActionStacks.ContainsKey(itemCoord))
                {
                    this.ActionStacks[itemCoord].Add(item);
                }
                else
                {
                    this.ActionStacks.TryAdd(itemCoord, new List<Item>() { item });
                }
            }
            else if (WiredUtillity.TypeIsWiredCondition(item.GetBaseItem().InteractionType))
            {
                if (this.ConditionStacks.ContainsKey(itemCoord))
                {
                    this.ConditionStacks[itemCoord].Add(item);
                }
                else
                {
                    this.ConditionStacks.TryAdd(itemCoord, new List<Item>() { item });
                }
            }
            else if (item.GetBaseItem().InteractionType == InteractionType.SPECIALRANDOM)
            {
                if (!this.SpecialRandom.Contains(itemCoord))
                {
                    this.SpecialRandom.Add(itemCoord);
                }
            }
            else if (item.GetBaseItem().InteractionType == InteractionType.SPECIALUNSEEN)
            {
                if (!this.SpecialUnseen.ContainsKey(itemCoord))
                {
                    this.SpecialUnseen.Add(itemCoord, 0);
                }
            }
        }

        public void RemoveFurniture(Item item)
        {
            Point itemCoord = item.Coordinate;
            if (WiredUtillity.TypeIsWiredAction(item.GetBaseItem().InteractionType))
            {
                Point coordinate = item.Coordinate;
                if (!this.ActionStacks.ContainsKey(coordinate))
                {
                    return;
                }
                this.ActionStacks[coordinate].Remove(item);
                if (this.ActionStacks[coordinate].Count == 0)
                {
                    List<Item> NewList = new List<Item>();
                    this.ActionStacks.TryRemove(coordinate, out NewList);
                }
            }
            else if (WiredUtillity.TypeIsWiredCondition(item.GetBaseItem().InteractionType))
            {
                if (!this.ConditionStacks.ContainsKey(itemCoord))
                {
                    return;
                }
                this.ConditionStacks[itemCoord].Remove(item);
                if (this.ConditionStacks[itemCoord].Count == 0)
                {
                    List<Item> newList = new List<Item>();
                    this.ConditionStacks.TryRemove(itemCoord, out newList);
                }
            }
            else if (item.GetBaseItem().InteractionType == InteractionType.SPECIALRANDOM)
            {
                if (this.SpecialRandom.Contains(itemCoord))
                {
                    this.SpecialRandom.Remove(itemCoord);
                }
            }
            else if (item.GetBaseItem().InteractionType == InteractionType.SPECIALUNSEEN)
            {
                if (this.SpecialUnseen.ContainsKey(itemCoord))
                {
                    this.SpecialUnseen.Remove(itemCoord);
                }
            }
        }

        public void OnCycle()
        {
            if (this._doCleanup)
            {
                this.ClearWired();
            }
            else
            {
                if (this._requestingUpdates.Count > 0)
                {
                    List<WiredCycle> toAdd = new List<WiredCycle>();
                    while (this._requestingUpdates.Count > 0)
                    {
                        if (!this._requestingUpdates.TryDequeue(out WiredCycle handler))
                        {
                            continue;
                        }

                        if (handler.IWiredCycleable.Disposed())
                        {
                            continue;
                        }

                        if (handler.OnCycle())
                        {
                            toAdd.Add(handler);
                        }
                    }

                    foreach (WiredCycle cycle in toAdd)
                    {
                        this._requestingUpdates.Enqueue(cycle);
                    }
                }

                this._wiredUsed.Clear();
            }
        }

        private void ClearWired()
        {
            foreach (List<Item> list in this.ActionStacks.Values)
            {
                foreach (Item roomItem in list)
                {
                    if (roomItem.WiredHandler != null)
                    {
                        roomItem.WiredHandler.Dispose();
                        roomItem.WiredHandler = null;
                    }
                }
            }

            foreach (List<Item> list in this.ConditionStacks.Values)
            {
                foreach (Item roomItem in list)
                {
                    if (roomItem.WiredHandler != null)
                    {
                        roomItem.WiredHandler.Dispose();
                        roomItem.WiredHandler = null;
                    }
                }
            }

            this.ConditionStacks.Clear();
            this.ActionStacks.Clear();
            this._wiredUsed.Clear();
            this._doCleanup = false;
        }

        public void OnPickall()
        {
            this._doCleanup = true;
        }

        public void ExecutePile(Point coordinate, RoomUser user, Item item)
        {
            if (!this.ActionStacks.ContainsKey(coordinate))
            {
                return;
            }

            if (this._wiredUsed.ContainsKey(coordinate))
            {
                if (this._wiredUsed[coordinate].Contains(user))
                {
                    return;
                }
                else
                {
                    this._wiredUsed[coordinate].Add(user);
                }
            }
            else
            {
                this._wiredUsed.TryAdd(coordinate, new List<RoomUser>() { user });
            }

            if (this.ConditionStacks.ContainsKey(coordinate))
            {
                List<Item> ConditionStack = this.ConditionStacks[coordinate];
                int CycleCountCdt = 0;
                foreach (Item roomItem in ConditionStack.ToArray())
                {
                    CycleCountCdt++;
                    if (CycleCountCdt > 20)
                    {
                        break;
                    }

                    if (roomItem == null || roomItem.WiredHandler == null)
                    {
                        continue;
                    }

                    if (!((IWiredCondition)roomItem.WiredHandler).AllowsExecution(user, item))
                    {
                        return;
                    }
                }
            }

            List<Item> ActionStack = this.ActionStacks[coordinate].OrderBy(p => p.GetZ).ToList();

            if (this.SpecialRandom.Contains(coordinate))
            {
                int CountAct = ActionStack.Count - 1;

                int RdnWired = ButterflyEnvironment.GetRandomNumber(0, CountAct);
                Item ActRand = ActionStack[RdnWired];
                ((IWiredEffect)ActRand.WiredHandler).Handle(user, item);
            }
            else if (this.SpecialUnseen.ContainsKey(coordinate))
            {
                int CountAct = ActionStack.Count - 1;

                int NextWired = this.SpecialUnseen[coordinate];
                if (NextWired > CountAct)
                {
                    NextWired = 0;
                    this.SpecialUnseen[coordinate] = 0;
                }

                this.SpecialUnseen[coordinate]++;

                Item ActNext = ActionStack[NextWired];
                if (ActNext != null && ActNext.WiredHandler != null)
                {
                    ((IWiredEffect)ActNext.WiredHandler).Handle(user, item);
                }
            }
            else
            {
                int CycleCount = 0;
                foreach (Item roomItem in ActionStack.ToArray())
                {
                    CycleCount++;

                    if (CycleCount > 20)
                    {
                        break;
                    }

                    if (roomItem != null && roomItem.WiredHandler != null)
                    {
                        ((IWiredEffect)roomItem.WiredHandler).Handle(user, item);
                    }
                }
            }
        }

        public void RequestCycle(WiredCycle handler)
        {
            this._requestingUpdates.Enqueue(handler);
        }

        public Room GetRoom()
        {
            return this._room;
        }

        public void Destroy()
        {
            if (this.ActionStacks != null)
            {
                this.ActionStacks.Clear();
            }

            if (this.ConditionStacks != null)
            {
                this.ConditionStacks.Clear();
            }

            if (this._requestingUpdates != null)
            {
                this._requestingUpdates = null;
            }

            this.TrgCollision = null;
            this.TrgBotCollision = null;
            this.TrgTimer = null;
            this._wiredUsed.Clear();
        }

        public void TriggerCollision(RoomUser roomUser, Item Item)
        {
            if (this.TrgCollision != null)
            {
                this.TrgCollision(roomUser, Item);
            }
        }

        public void TriggerBotCollision(RoomUser roomUser, string BotName)
        {
            if (this.TrgBotCollision != null)
            {
                this.TrgBotCollision(roomUser, BotName);
            }
        }

        public void TriggerTimer()
        {
            if (this.TrgTimer != null)
            {
                this.TrgTimer(null, null);
            }
        }
    }
}
