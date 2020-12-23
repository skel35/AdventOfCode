using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2020
{
    public class Day20 : Solution
    {
        public Day20() : base(20, 2020) { }
        protected override void Solve()
        {
            var input =
                ReadText()
                    .Split("\n\n")
                    .Select(s => s.SplitBy('\n'))
                    .Select(lines => (lines[0].Ss(" ", ":").ToInt(), lines.Skip(1).ToArray()))
                    .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);

            string[] Get4Borders(string[] grid)
            {
                return new[] {grid[0], grid.Last(), grid.Select(s => s[0]).ToStr(), grid.Select(s => s.Last()).ToStr()};
            }
            string[] Get8Borders(string[] grid)
            {
                var fourBorders = new[] {grid[0], grid.Last(), grid.Select(s => s[0]).ToStr(), grid.Select(s => s.Last()).ToStr()};
                return fourBorders.Concat(fourBorders.Select(s => s.Reverse().ToStr())).ToArray();
            }

            var tileToBorders =
                input
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => Get8Borders(kvp.Value));

            var borderToTiles = new Dictionary<string, List<int>>();
            input.ForEach(kvp =>
                Get8Borders(kvp.Value).ForEach(b => borderToTiles.AddOrUpdate(b, new List<int> { kvp.Key }, (s, ints) =>
                {
                    ints.Add(kvp.Key);
                    return ints;
                })));

            borderToTiles
                .Where(kvp => kvp.Value.Count == 1)
                .Select(kvp => kvp.Value[0])
                .GroupBy(x => x)
                .Select(g => (g.Key, g.Count()))
                .OrderBy(t => t.Item2)
                .Where(t => t.Item2 == 4)
                .Select(t => (long)t.Item1)
                .Product()
                .ToString()
                .Print();
        }
    }
}
