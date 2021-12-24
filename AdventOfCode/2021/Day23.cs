using static System.Math;

namespace AdventOfCode._2021;

public class Day23 : Solution
{
    public Day23() : base(23, 2021) { }

    protected override void Solve()
    {
        var rooms = new[]
        {
            new byte[] { 8,  9,  10, 11 },
            new byte[] { 12, 13, 14, 15 },
            new byte[] { 16, 17, 18, 19 },
            new byte[] { 20, 21, 22, 23 },
        };
        int GetRoom(int val) => (val - 8) / 4;

        IEnumerable<int> SlotsBetweenRooms(int room1, int room2)
        {
            (room1 != room2).Assert();
            (room1, room2) = (Min(room1, room2), Max(room1, room2));
            // 2, 3, 4
            if (room1 == 0) yield return 2;
            if (room1 <= 1 && room2 >= 2) yield return 3;
            if (room2 == 3) yield return 4;
        }

        IEnumerable<int> SlotsBetweenSlotAndRoom(int slot, int roomIndex)
        {
            var slotOther = 1.5 + roomIndex; // 0 -> 1.5, 1 -> 2.5, 2 -> 3.5, 3 -> 4.5
            var (slot1, slot2) = (Min(slot, slotOther), Max(slot, slotOther));
            if (slotOther == slot1) return ((int)Ceiling(slot1)..(int)(slot2 + 1)).ToEnumerable();
            return ((int)slot1..(int)Ceiling(slot2)).ToEnumerable();
        }

        // var r1 = SlotsBetweenSlotAndRoom(0, 0).ToArray(); // 0, 1
        // var r2 = SlotsBetweenSlotAndRoom(0, 1).ToArray(); // 0, 1, 2
        // var r3 = SlotsBetweenSlotAndRoom(6, 0).ToArray(); // 2, 3, 4, 5, 6

        var initState = new State2(
            new SubState(15, 19, 18, 21),
            new SubState(11, 20, 14, 17),
            new SubState(16, 23, 13, 22),
            new SubState(8,  12, 9,  10));

        var frontier = new PriorityQueue<State2, int>();
        frontier.Enqueue(initState, initState.MinPossible());
        var visited = new Dictionary<State2, int>();
        var enqueued = new Dictionary<State2, int>();
        var minScore = int.MaxValue;
        var minItem = initState;
        while (frontier.Count > 0)
        {
            frontier.TryDequeue(out var item, out var priority);
            if (priority >= minScore)
                continue;
            var score = priority - item.MinPossible();
            visited[item] = score;

            if (item.IsFinal())
            {
                if (score < minScore)
                {
                    minScore = score;
                    minItem = item;
                }
                continue;
            }

            void Enqueue(State2 newItem, int newScore)
            {
                var newPriority = newScore + newItem.MinPossible();
                if (newPriority >= minScore) return;

                if (visited.TryGetValue(newItem, out var oldScore) && oldScore <= newScore)
                {
                    return;
                }

                if (enqueued.TryGetValue(newItem, out var oldPriority) && oldPriority <= newPriority)
                {
                    return;
                }
                {
                    frontier.Enqueue(newItem, newPriority);
                    enqueued[newItem] = newPriority;
                }
            }

            // generate possible steps
            var amphipods = item.Amphipods();
            for (var i = 0; i < amphipods.Length; i++)
            {
                var (val, targetRoomIndex, index) = amphipods[i];
                var targetRoom = rooms[targetRoomIndex];
                if (val >= 8)
                {
                    // check if someone is above us in the same room
                    for (var j = 0; j < val % 4; j++)
                    {
                        var posToCheck = val - (val % 4) + j;
                        if (amphipods.Any(x => x.val == posToCheck))
                            goto endLoop3;
                    }
                }
                // 1. straight into a room
                {
                    // skip if in target slot
                    if (targetRoom.Contains(val))
                        goto endLoop;

                    var targetValIndex = 3;
                    for (var j = 3; j >= 0; j--)
                    {
                        var tar = targetRoom[j];
                        var inDeep2 = amphipods.FirstOrDefault(x => x.val == tar);
                        if (inDeep2 != default)
                        {
                            if (inDeep2.targetRoom != targetRoomIndex)
                                goto endLoop;
                            targetValIndex = j - 1; // our teammate is already in target room
                        }
                    }

                    if (targetValIndex < 0)
                        goto endLoop;

                    var targetVal = targetRoom[targetValIndex];

                    if (val >= 8)
                    {
                        // we're currently in some other room
                        var ourRoomIndex = GetRoom(val);
                        var stepsCount = targetValIndex + (val % 4) + 2;
                        //  check if someone in the slots between rooms
                        foreach (var slotInd in SlotsBetweenRooms(ourRoomIndex, targetRoomIndex))
                        {
                            if (amphipods.Any(x => x.val == slotInd))
                                goto endLoop;
                            stepsCount += 2;
                        }

                        var newItem = item.SetIndex(i, targetVal);
                        var newScore = score + stepsCount * (int)Pow(10, targetRoomIndex);
                        Enqueue(newItem, newScore);
                    }
                    else
                    {
                        // we're in some slot
                        var stepsCount = 0;
                        foreach (var slotInd in SlotsBetweenSlotAndRoom(val, targetRoomIndex))
                        {
                            if (slotInd != val && amphipods.Any(x => x.val == slotInd))
                                goto endLoop;
                            stepsCount++;
                            if (slotInd is >= 1 and <= 5)
                                stepsCount++;
                        }

                        stepsCount += targetVal % 4;
                        (targetVal % 4 == targetValIndex).Assert();
                        var newItem = item.SetIndex(i, targetVal);
                        var newScore = score + stepsCount * (int)Pow(10, targetRoomIndex);
                        Enqueue(newItem, newScore);
                    }

                    endLoop: ;
                }
                // 2: into some slot
                {
                    if (val < 8) continue;
                    // skip if in target room
                    var inTargetRoomIndex = Array.IndexOf(targetRoom, val);
                    if (inTargetRoomIndex != -1)
                    {
                        var allBelowAreTeammates = true;
                        for (var j = inTargetRoomIndex + 1; j <= 3; j++)
                        {
                            if (!amphipods.Any(a => a.targetRoom == targetRoomIndex && a.index != index && a.val == targetRoom[j]))
                            {
                                allBelowAreTeammates = false;
                            }
                        }

                        if (allBelowAreTeammates)
                            continue;
                    }

                    var ourRoomIndex = GetRoom(val);

                    for (byte j = 0; j < 7; j++)
                    {
                        var stepsCount = (val % 4);
                        foreach (var slotInd in SlotsBetweenSlotAndRoom(j, ourRoomIndex))
                        {
                            if (amphipods.Any(x => x.val == slotInd))
                                goto endLoop2;
                            stepsCount++;
                            if (slotInd is >= 1 and <= 5)
                                stepsCount++;
                        }

                        var newItem = item.SetIndex(i, j);
                        var newScore = score + stepsCount * (int)Pow(10, targetRoomIndex);
                        Enqueue(newItem, newScore);

                        endLoop2: ;
                    }
                }
                endLoop3: ;
            }

        }

        minScore.ToString().Print();
    }

    // struct State
    // {
    //     public byte[] InSlots { get; } = new byte[7];
    //     public byte[,] InRooms { get; } = new byte[4, 2];
    //
    //     public State Copy()
    //     {
    //         var res = new State();
    //         InSlots.CopyTo(res.InSlots, InSlots.Length);
    //         InRooms.CopyTo(res.InRooms, InRooms.Length);
    //         return res;
    //     }
    // }

    private readonly record struct SubState(byte X1, byte X2, byte X3, byte X4)
    {
        public IEnumerable<byte> Items()
        {
            yield return X1;
            yield return X2;
            yield return X3;
            yield return X4;
        }
        public (byte x1, byte x2, byte x3, byte x4) Sorted()
        {
            var temp = new[] { X1, X2, X3, X4 }.OrderBy(x => x).ToArray();
            return (temp[0], temp[1], temp[2], temp[3]);
        }

        public SubState SetIndex(int index, byte val)
        {
            return index switch
            {
                0 => this with { X1 = val },
                1 => this with { X2 = val },
                2 => this with { X3 = val },
                3 => this with { X4 = val },
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
        }
    }
    private readonly record struct State2(SubState A, SubState B, SubState C, SubState D)
    {
        public SubState[] SubStates()
        {
            return new[] { A, B, C, D };
        }
        public bool IsFinal()
        {
            var subStates = SubStates();
            for (var i = 0; i < subStates.Length; i++)
            {
                var start = 8 + i * 4;
                if (subStates[i].Sorted() != (start, start + 1, start + 2, start + 3)) return false;
            }

            return true;
        }

        public int MinPossible()
        {
            var res = 0;
            if (A.Items().Any(x => x == 0)) res += 3;
            if (A.Items().Any(x => x == 1)) res += 2;
            if (A.Items().Any(x => x == 2)) res += 2;
            if (A.Items().Any(x => x == 3)) res += 4;
            if (A.Items().Any(x => x == 4)) res += 6;
            if (A.Items().Any(x => x == 5)) res += 8;
            if (A.Items().Any(x => x == 6)) res += 9;

            if (B.Items().Any(x => x == 0)) res += 50;
            if (B.Items().Any(x => x == 1)) res += 40;
            if (B.Items().Any(x => x == 2)) res += 20;
            if (B.Items().Any(x => x == 3)) res += 20;
            if (B.Items().Any(x => x == 4)) res += 40;
            if (B.Items().Any(x => x == 5)) res += 60;
            if (B.Items().Any(x => x == 6)) res += 70;

            if (C.Items().Any(x => x == 0)) res += 700;
            if (C.Items().Any(x => x == 1)) res += 600;
            if (C.Items().Any(x => x == 2)) res += 400;
            if (C.Items().Any(x => x == 3)) res += 200;
            if (C.Items().Any(x => x == 4)) res += 200;
            if (C.Items().Any(x => x == 5)) res += 400;
            if (C.Items().Any(x => x == 6)) res += 500;

            if (D.Items().Any(x => x == 0)) res += 9000;
            if (D.Items().Any(x => x == 1)) res += 8000;
            if (D.Items().Any(x => x == 2)) res += 6000;
            if (D.Items().Any(x => x == 3)) res += 4000;
            if (D.Items().Any(x => x == 4)) res += 2000;
            if (D.Items().Any(x => x == 5)) res += 2000;
            if (D.Items().Any(x => x == 6)) res += 3000;

            A.Items().ForEach(x => { if (x is >= 12 and <= 15) res += x - 8; });
            A.Items().ForEach(x => { if (x is >= 16 and <= 19) res += x - 10; });
            A.Items().ForEach(x => { if (x is >= 20 and <= 23) res += x - 12; });

            B.Items().ForEach(x => { if (x is >= 16 and <= 19) res += (x - 12)*10; });
            B.Items().ForEach(x => { if (x is >= 20 and <= 23) res += (x - 14)*10; });
            B.Items().ForEach(x => { if (x is >= 8 and <= 11) res += (x - 4)*10; });

            C.Items().ForEach(x => { if (x is >= 8 and <= 11) res += (x - 2)*100; });
            C.Items().ForEach(x => { if (x is >= 12 and <= 15) res += (x - 8)*100; });
            C.Items().ForEach(x => { if (x is >= 20 and <= 23) res += (x - 16)*10; });

            D.Items().ForEach(x => { if (x is >= 8 and <= 11) res += (x - 0)*100; });
            D.Items().ForEach(x => { if (x is >= 12 and <= 15) res += (x - 6)*100; });
            D.Items().ForEach(x => { if (x is >= 16 and <= 19) res += (x - 12)*10; });

            return res;
        }

        public (byte val, int targetRoom, int index)[] Amphipods()
        {
            return A.Items()
                .Select((v, i) => (v, 0, i))
                .Concat(B.Items().Select((v, i) => (v, 1, i)))
                .Concat(C.Items().Select((v, i) => (v, 2, i)))
                .Concat(D.Items().Select((v, i) => (v, 3, i)))
                .ToArray();
        }

        public State2 SetIndex(int index, byte val)
        {
            return (index / 4) switch
            {
                0 => this with { A = A.SetIndex(index % 4, val) },
                1 => this with { B = B.SetIndex(index % 4, val) },
                2 => this with { C = C.SetIndex(index % 4, val) },
                3 => this with { D = D.SetIndex(index % 4, val) },
                _ => throw new InvalidOperationException()
            };
        }
    }
}