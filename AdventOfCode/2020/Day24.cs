using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2020
{
    public class Day24 : Solution
    {
        public Day24() : base(24, 2020) { }
        protected override void Solve()
        {
            var set = new HashSet<(int R, int C)>();
            ReadLines()
                .ForEach(line =>
                {
                    var cur = (R: 0, C: 0);
                    var i = 0;
                    while (i < line.Length)
                    {
                        cur = line[i] switch
                        {
                            'e' => (cur.R, cur.C + 1),
                            'w' => (cur.R, cur.C - 1),
                            'n' when line[i + 1] == 'e' => (cur.R - 1, cur.R % 2 == 0 ? cur.C + 1 : cur.C),
                            'n' when line[i + 1] == 'w' => (cur.R - 1, cur.R % 2 == 0 ? cur.C : cur.C - 1),
                            's' when line[i + 1] == 'e' => (cur.R + 1, cur.R % 2 == 0 ? cur.C + 1 : cur.C),
                            's' when line[i + 1] == 'w' => (cur.R + 1, cur.R % 2 == 0 ? cur.C : cur.C - 1)
                        };
                        i = line[i] switch
                        {
                            'n' or 's' => i + 2,
                            _ => i + 1
                        };
                    }

                    var _ = set.Contains(cur) ? set.Remove(cur) : set.Add(cur);
                });

            // Console.WriteLine(set.Count);

            for (var i = 0; i < 100; i++)
            {
                (int R, int C)[] GetNeighbors((int R, int C) cur)
                {
                    return new[]
                    {
                        (cur.R, cur.C + 1),
                        (cur.R, cur.C - 1),
                        (cur.R - 1, cur.R % 2 == 0 ? cur.C + 1 : cur.C),
                        (cur.R - 1, cur.R % 2 == 0 ? cur.C : cur.C - 1),
                        (cur.R + 1, cur.R % 2 == 0 ? cur.C + 1 : cur.C),
                        (cur.R + 1, cur.R % 2 == 0 ? cur.C : cur.C - 1)
                    };
                }

                var whiteTiles = set.SelectMany(GetNeighbors).ToHashSet();
                whiteTiles.ExceptWith(set);
                var blackToWhite =
                    set.Select(x => (x, GetNeighbors(x).Count(set.Contains)))
                        .Where(t => t.Item2 == 0 || t.Item2 > 2)
                        .Select(t => t.Item1)
                        .ToArray();
                var whiteToBlack = whiteTiles.Select(x => (x, GetNeighbors(x).Count(set.Contains)))
                    .Where(t => t.Item2 == 2)
                    .Select(t => t.Item1)
                    .ToArray();
                set.ExceptWith(blackToWhite);
                set.UnionWith(whiteToBlack);
            }

            Console.WriteLine(set.Count);
        }
    }
}
