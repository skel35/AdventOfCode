using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using static System.Math;
using static AdventOfCode.AoC;

namespace AdventOfCode._2017
{
    public class Day24 : Solution
    {
        const int N = 57;
        public Day24() : base(24, 2017) { }
        protected override void Solve()
        {
            var inp = ReadLines().Select(line => line.Split('/').Select(int.Parse).OrderBy(v => v).ToArray()).ToArray();
            var dict = new Dictionary<int, List<int>>();
            dict[0] = new List<int>();
            for (var i = 0; i < inp.Length; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    if (inp[i][j] == 0) continue;
                    if (!dict.ContainsKey(inp[i][j])) dict.Add(inp[i][j], new List<int>());
                    dict[inp[i][j]].Add(i);
                }
            }

            var maxLength = 0;
            var maxStrength = 0;
            var totalCount = 0;
            var bridgeHashMap = new Dictionary<ulong, Bridge>();
            var queue = new Queue<ulong>();
            void AddBridge(Bridge bridge)
            {
                var hash = bridge.Hash();
                if (!bridgeHashMap.ContainsKey(hash))
                {
                    bridgeHashMap.Add(hash, bridge);
                    queue.Enqueue(hash);
                }
            }
            for (var i = 0; i < inp.Length; i++)
            {
                if (inp[i][0] == 0)
                {
                    var bridge = new Bridge();
                    bridge.Left = inp[i][0];
                    bridge.Right = inp[i][1];
                    bridge.AddIn(i, inp[i]);
                    AddBridge(bridge);
                }
            }

            while (queue.Count > 0)
            {
                var bridge = bridgeHashMap[queue.Dequeue()];
                totalCount++;
                // try attach another connector
                // if not possible = calculate strength and compare to max strength
                var cLeft = dict[bridge.Left].Where(c => (bridge.Used >> c) % 2 == 0).ToArray();
                var cRight = dict[bridge.Right].Where(c => (bridge.Used >> c) % 2 == 0).ToArray();
                if (cLeft.Length == 0 && cRight.Length == 0)
                {
                    var length = bridge.Length;
                    var strength = bridge.Strength;
                    if (length > maxLength)
                    {
                        maxLength = length;
                        maxStrength = strength;
                    }
                    else if (length == maxLength)
                    {
                        if (strength > maxStrength)
                        {
                            maxStrength = strength;
                        }
                    }
                }
                else
                {
                    for (var i = 0; i < cLeft.Length; i++)
                    {
                        var newBridge = bridge;
                        newBridge.AttachLeft(cLeft[i], inp[cLeft[i]]);
                        AddBridge(newBridge);
                    }
                    for (var i = 0; i < cRight.Length; i++)
                    {
                        var newBridge = bridge;
                        newBridge.AttachRight(cRight[i], inp[cRight[i]]);
                        AddBridge(newBridge);
                    }
                }
            }

            Console.WriteLine(maxStrength);
            Console.WriteLine("total count = " + totalCount);
        }

        struct Bridge
        {
            public ulong Used;
            public int Left;
            public int Right;
            public int Strength;
            public int Length;
            public ulong Hash()
            {
                var sevenBits = ((ulong) Left + (ulong)Right) % (1ul << 7);
                return (Used << (64 - N)) ^ sevenBits;
            }
            
            public void AddIn(int ind, int[] val)
            {
                Used |= (1ul << ind);
                Strength += val[0] + val[1];
                Length++;
                SortSides();
            }

            private void SortSides()
            {
                if (Left > Right)
                {
                    Swap(ref Left, ref Right);
                }
            }
            public void AttachLeft(int ind, int[] val)
            {
                if (val[0] == Left) Left = val[1];
                else if (val[1] == Left) Left = val[0];
                else throw new ArgumentException();
                AddIn(ind, val);
            }

            public void AttachRight(int ind, int[] val)
            {
                if (val[0] == Right) Right = val[1];
                else if (val[1] == Right) Right = val[0];
                else throw new ArgumentException();
                AddIn(ind, val);
            }

            // public Bridge Clone()
            // {
            //     var clone = new Bridge();
            //     clone.Used = Used;
            //     clone.Left = Left;
            //     clone.Right = Right;
            //     clone.Strength = Strength;
            //     return clone;
            // }
        }

    }

}