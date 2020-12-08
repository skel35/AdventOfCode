using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;
using static AdventOfCode.AoC;

namespace AdventOfCode._2019
{
    public class Day3 : Solution
    {
        public Day3() : base(3, 2019) { }

        protected override void Solve()
        {
            var wires = ReadLines().Select(w => w.Split2()).ToArray();
            var w1 = new List<(int r, int c)> { (0, 0)};
            var w2 = new List<(int r, int c)> { (0, 0)};
            wires[0].Select(ParseVector).Aggregate((R:  0, c: 0), (acc, v) =>
            {
                var newAcc = acc.Plus(v);
                w1.Add(newAcc);
                return newAcc;
            });
            wires[1].Select(ParseVector).Aggregate((R:  0, c: 0), (acc, v) =>
            {
                var newAcc = acc.Plus(v);
                w2.Add(newAcc);
                return newAcc;
            });
            var minDist = int.MaxValue;
            var dist1 = 0;
            for (var i = 1; i < w2.Count; i++)
            {
                var dist2 = 0;
                for (var j = 1; j < w1.Count; j++)
                {
                    var intersects = GridIntersection(w2[0], w2[1], w1[0], w1[1]);
                    if (intersects.HasValue)
                    {
                        var dist = dist1 + dist2 + Math.Abs(w1[j].r - w2[i - 1].r) + Abs(w2[i].c - w1[j - 1].c);
                        if (dist < minDist) minDist = dist;
                    }

                    dist2 += int.Parse(wires[0][j - 1][1..]);
                }
                dist1 += int.Parse(wires[1][i - 1][1..]);
            }

            Console.WriteLine(minDist);
        }

        static (int r, int c) ParseVector(string s)
        {
            var dir = s[0] switch
            {
                'U' => (-1, 0),
                'R' => (0, 1),
                'D' => (1, 0),
                'L' => (0, -1),
                _ => throw new InvalidOperationException()
            };
            var n = int.Parse(s[1..]);
            return dir.Mult(n);
        }
    }
}