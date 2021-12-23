using static System.Math;

namespace AdventOfCode._2021;

public class Day23 : Solution
{
    public Day23() : base(23, 2021) { }

    protected override void Solve()
    {
        // var grid = ReadLines();
        // var slots = new[]
        // {
        //     (1, 1),
        //     (1, 2),
        //     (1, 4),
        //     (1, 6),
        //     (1, 8),
        //     (1, 10),
        //     (1, 11),
        // };
        // var rooms = new[]
        // {
        //     new[] { (2, 3), (3, 3) },
        //     new[] { (2, 5), (3, 5) },
        //     new[] { (2, 7), (3, 7) },
        //     new[] { (2, 9), (3, 9) },
        // };
        var rooms = new[]
        {
            new byte[] { 8, 9 },
            new byte[] { 10, 11 },
            new byte[] { 12, 13 },
            new byte[] { 14, 15 },
        };
        int GetRoom(int val) => (val - 8) / 2;

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

        var initState = new State2(11, 13, 9, 14, 12, 15, 8, 10);
        var states = new[]
        {
            new State2(11, 13, 9, 14, 12, 15, 8, 10),
            new State2(11, 13, 9, 14, 4,  15, 8, 10),
            new State2(11, 0,  9, 14, 4,  15, 8, 10),
            new State2(11, 0,  9, 14, 13, 15, 8, 10),
            new State2(11, 0,  9, 5,  13, 15, 8, 10),
            new State2(11, 0,  9, 5,  13, 12, 8, 10),
            new State2(11, 0,  9, 5,  13, 12, 8, 15),
            new State2(11, 0,  9, 5,  13, 12, 14, 15),
            new State2(1,  0,  9, 5,  13, 12, 14, 15),
            new State2(1,  0,  9, 11, 13, 12, 14, 15),
            new State2(1,  0, 10, 11, 13, 12, 14, 15),
            new State2(9,  0, 10, 11, 13, 12, 14, 15),
            new State2(9,  8, 10, 11, 13, 12, 14, 15)
        };

        // var initState = new State2(9, 8, 1, 6, 4, 5, 14, 15);
        // var initState = new State2(9, 13, 10, 11, 12, 5, 14, 15);
        var frontier = new PriorityQueue<State2, int>();
        frontier.Enqueue(initState, initState.MinPossible());
        var visited = new Dictionary<State2, int>();
        var enqueued = new Dictionary<State2, int>();
        var minScore = int.MaxValue;
        var minItem = initState;
        // var farthestState = initState;
        // var maxInItsPlace = 1;
        while (frontier.Count > 0)
        {
            frontier.TryDequeue(out var item, out var priority);
            if (item.Equals(states[11]))
            {
                Console.WriteLine("test");
            }
            if (priority >= minScore)
                continue;
            var score = priority - item.MinPossible();
            visited[item] = score;
            // var inItsPlace = item.InItsPlace();
            // if (inItsPlace > maxInItsPlace)
            // {
            //     maxInItsPlace = inItsPlace;
            //     farthestState = item;
            // }

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
                    if (newItem.Equals(states[11]))
                    {
                        Console.WriteLine("test");
                    }
                    if (newItem.Equals(states[12]))
                    {
                        Console.WriteLine("test");
                    }
                }
            }

            // generate possible steps
            var amphipods = item.Amphipods();
            for (var i = 0; i < amphipods.Length; i++)
            {
                var (val, targetRoomIndex, index) = amphipods[i];
                var targetRoom = rooms[targetRoomIndex];
                if (val >= 8 && val % 2 == 1)
                {
                    if (amphipods.Any(x => x.val == val - 1)) // someone above us in the same room
                        continue;
                }
                // 1. straight into a room
                {
                    // skip if in target slot
                    if (val == targetRoom[1] || val == targetRoom[0])
                        goto endLoop;

                    var targetVal = targetRoom[1];
                    if (amphipods.Any(x => x.val == targetRoom[0]))
                        goto endLoop;
                    var inDeep = amphipods.FirstOrDefault(x => x.val == targetRoom[1]);
                    if (inDeep != default)
                    {
                        if (inDeep.targetRoom != targetRoomIndex)
                            goto endLoop;
                        targetVal = targetRoom[0]; // our teammate is already in target room
                    }

                    if (val >= 8)
                    {
                        // we're currently in some other room
                        var ourRoomIndex = GetRoom(val);
                        var stepsCount = (targetVal == targetRoom[1] ? 1 : 0) + (val % 2) + 2;
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

                        stepsCount += targetVal % 2;
                        var newItem = item.SetIndex(i, targetVal);
                        var newScore = score + stepsCount * (int)Pow(10, targetRoomIndex);
                        Enqueue(newItem, newScore);
                    }

                    endLoop: ;
                }
                // 2: into some slot
                {
                    if (val < 8) continue;
                    // skip if in target slot
                    if (val == targetRoom[1])
                        continue;
                    if (val == targetRoom[0] &&
                        amphipods.Single(a => a.targetRoom == targetRoomIndex && a.index != index).val == targetRoom[1])
                        continue;
                    var ourRoomIndex = GetRoom(val);

                    for (byte j = 0; j < 7; j++)
                    {
                        var stepsCount = (val % 2);
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

    private readonly record struct State2(byte A1, byte A2, byte B1, byte B2, byte C1, byte C2, byte D1, byte D2)
    {
        public bool IsFinal() => A1 is 8 or 9 && A2 is 8 or 9
                              && B1 is 10 or 11 && B2 is 10 or 11
                              && C1 is 12 or 13 && C2 is 12 or 13
                              && D1 is 14 or 15 && D2 is 14 or 15;

        public int InItsPlace()
        {
            var res = 0;
            if (A1 is 8 or 9) res++;
            if (A2 is 8 or 9) res++;
            if (B1 is 10 or 11) res++;
            if (B2 is 10 or 11) res++;
            if (C1 is 12 or 13) res++;
            if (C2 is 12 or 13) res++;
            if (D1 is 14 or 15) res++;
            if (D2 is 14 or 15) res++;
            return res;
        }

        public int Util()
        {
            var res = 0;
            if ((A1, A2) is (8, 9) or (9, 8)) res += 2;
            else if (A1 is 9 || A2 is 9) res++;
            if ((B1, B2) is (10, 11) or (11, 10)) res += 20;
            else if (B1 is 10 || B2 is 11) res += 10;
            if ((C1, C2) is (12, 13) or (13, 12)) res += 200;
            else if (C1 is 13 || C2 is 13) res += 100;
            if ((D1, D2) is (15, 14) or (14, 15)) res += 2000;
            else if (D1 is 15 || D2 is 15) res += 1000;
            return res * 8;
        }

        public int MinPossible()
        {
            var res = 0;
            if (A1 is 0 || A2 is 0) res += 3;
            if (A1 is 1 || A2 is 1) res += 2;
            if (A1 is 2 || A2 is 2) res += 2;
            if (A1 is 3 || A2 is 3) res += 4;
            if (A1 is 4 || A2 is 4) res += 6;
            if (A1 is 5 || A2 is 5) res += 8;
            if (A1 is 6 || A2 is 6) res += 9;

            if (B1 is 0 || B2 is 0) res += 50;
            if (B1 is 1 || B2 is 1) res += 40;
            if (B1 is 2 || B2 is 2) res += 20;
            if (B1 is 3 || B2 is 3) res += 20;
            if (B1 is 4 || B2 is 4) res += 40;
            if (B1 is 5 || B2 is 5) res += 60;
            if (B1 is 6 || B2 is 6) res += 70;

            if (C1 is 0 || C2 is 0) res += 700;
            if (C1 is 1 || C2 is 1) res += 600;
            if (C1 is 2 || C2 is 2) res += 400;
            if (C1 is 3 || C2 is 3) res += 200;
            if (C1 is 4 || C2 is 4) res += 200;
            if (C1 is 5 || C2 is 5) res += 400;
            if (C1 is 6 || C2 is 6) res += 500;

            if (D1 is 0 || D2 is 0) res += 9000;
            if (D1 is 1 || D2 is 1) res += 8000;
            if (D1 is 2 || D2 is 2) res += 6000;
            if (D1 is 3 || D2 is 3) res += 4000;
            if (D1 is 4 || D2 is 4) res += 2000;
            if (D1 is 5 || D2 is 5) res += 2000;
            if (D1 is 6 || D2 is 6) res += 3000;

            if (A1 is >= 10 and <= 15) res += A1 - 6;
            if (A2 is >= 10 and <= 15) res += A2 - 6;
            if (B1 is >= 12 and <= 15) res += (B1 - 5)*10;
            if (B2 is >= 12 and <= 15) res += (B2 - 5)*10;
            if (B1 is >= 8 and <= 9) res += (B1 - 4)*10;
            if (B2 is >= 8 and <= 9) res += (B2 - 4)*10;
            if (C1 is >= 14 and <= 15) res += (C1 - 3)*100;
            if (C2 is >= 14 and <= 15) res += (C2 - 3)*100;
            if (C1 is >= 8 and <= 9) res += (C1 - 2)*100;
            if (C2 is >= 8 and <= 9) res += (C2 - 2)*100;
            if (C1 is >= 10 and <= 11) res += (C1 - 6)*100;
            if (C2 is >= 10 and <= 11) res += (C2 - 6)*100;

            if (D1 is >= 8 and <= 13) res += (8 - 2 * ((D1 - 8) / 2) + D1 % 2) * 1000;
            if (D2 is >= 8 and <= 13) res += (8 - 2 * ((D2 - 8) / 2) + D2 % 2) * 1000;
            return res;
        }

        public (byte val, int targetRoom, int index)[] Amphipods()
        {
            return new[]
            {
                (A1, 0, 0),
                (A2, 0, 1),
                (B1, 1, 0),
                (B2, 1, 1),
                (C1, 2, 0),
                (C2, 2, 1),
                (D1, 3, 0),
                (D2, 3, 1),
            };
        }

        public State2 SetIndex(int index, byte val)
        {
            return index switch
            {
                0 => this with { A1 = val },
                1 => this with { A2 = val },
                2 => this with { B1 = val },
                3 => this with { B2 = val },
                4 => this with { C1 = val },
                5 => this with { C2 = val },
                6 => this with { D1 = val },
                7 => this with { D2 = val },
                _ => throw new InvalidOperationException()
            };
        }
    }
}