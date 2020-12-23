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

            // p1:
            // borderToTiles
            //     .Where(kvp => kvp.Value.Count == 1)
            //     .Select(kvp => kvp.Value[0])
            //     .GroupBy(x => x)
            //     .Select(g => (g.Key, g.Count()))
            //     .OrderBy(t => t.Item2)
            //     .Where(t => t.Item2 == 4)
            //     .Select(t => (long)t.Item1)
            //     .Product()
            //     .ToString()
            //     .Print();

            var tileToCountOfNonMatchedBorders =
                borderToTiles
                    .Where(kvp => kvp.Value.Count == 1)
                    .Select(kvp => kvp.Value[0])
                    .GroupBy(x => x)
                    .ToDictionary(g => g.Key, g => g.Count());
            var corners =
                tileToCountOfNonMatchedBorders
                    .Where(kvp => kvp.Value == 4)
                    .Select(kvp => kvp.Key)
                    .ToArray();
            var otherBorders = tileToCountOfNonMatchedBorders
                .Where(kvp => kvp.Value == 2)
                .Select(kvp => kvp.Key)
                .ToArray();

            IEnumerable<int> LinedUp(int tileNum)
            {
                return tileToBorders[tileNum]
                    .SelectMany(b => borderToTiles[b])
                    .Except(tileNum);
            }

            bool IsLinedUp(int tileNum1, int tileNum2)
            {
                return tileToBorders[tileNum1].Any(b => tileToBorders[tileNum2].Contains(b));
            }

            var size = (int) Round(Sqrt(input.Count));
            var resultGrid = new int[size, size];
            resultGrid[0, 0] = corners[0];
            var unusedBorders = otherBorders.ToHashSet();
            for (var i = 1; i < size - 1; i++)
            {
                resultGrid[i, 0] =
                    unusedBorders
                        .First(t => IsLinedUp(resultGrid[i - 1, 0], t));
                unusedBorders.Remove(resultGrid[i, 0]);
                resultGrid[0, i] =
                    unusedBorders
                        .First(t => IsLinedUp(resultGrid[0, i - 1], t));
                unusedBorders.Remove(resultGrid[0, i]);
                for (var j = 1; j < i; j++)
                {
                    // var lu1 = LinedUp(resultGrid[i, j - 1]).ToHashSet();
                    // var lu2 = LinedUp(resultGrid[i - 1, j]).ToHashSet();
                    // lu1.IntersectWith(lu2);
                    resultGrid[i, j] =
                        LinedUp(resultGrid[i, j - 1])
                            .Intersect(
                                LinedUp(resultGrid[i - 1, j]))
                            .Except(resultGrid[i - 1, j - 1])
                        // lu1
                            .Single();

                    resultGrid[j, i] =
                        LinedUp(resultGrid[j - 1, i])
                            .Intersect(
                                LinedUp(resultGrid[j, i - 1]))
                            .Except(resultGrid[j - 1, i - 1])
                            .Single();
                }

                resultGrid[i, i] =
                    LinedUp(resultGrid[i - 1, i])
                        .Intersect(
                            LinedUp(resultGrid[i, i - 1]))
                        .Except(resultGrid[i - 1, i - 1])
                        .Single();
            }

            resultGrid[size - 1, 0] = corners.Skip(1).First(c => IsLinedUp(c, resultGrid[size - 2, 0]));
            resultGrid[0, size - 1] = corners.Skip(1).First(c => IsLinedUp(c, resultGrid[0, size - 2]));
            for (var j = 1; j < size - 1; j++)
            {
                resultGrid[size - 1, j] =
                    unusedBorders
                        .First(t => IsLinedUp(resultGrid[size - 1, j - 1], t));
                unusedBorders.Remove(resultGrid[size - 1, j]);

                resultGrid[j, size - 1] =
                    unusedBorders
                        .First(t => IsLinedUp(resultGrid[j - 1, size - 1], t));
                unusedBorders.Remove(resultGrid[j, size - 1]);
            }

            resultGrid[size - 1, size - 1] =
                corners.Except(resultGrid[0, 0], resultGrid[size - 1, 0], resultGrid[0, size - 1]).Single();

            resultGrid.Print();

            var resultTilesSize = resultGrid.Length * (input[0].Length - 2);
            var resultTiles = new char[resultTilesSize, resultTilesSize];

            for (var i = 0; i < resultGrid.GetLength(0); i++)
            {
                for (var j = 0; j < resultGrid.GetLength(1); j++)
                {
                    var tile = input[resultGrid[i, j]];
                    for (var ii = 1; ii < tile.Length - 1; ii++)
                    {
                        for (var jj = 1; jj < tile[ii].Length - 1; jj++)
                        {
                            // resultTiles[i * (input[0].Length - 2) + (ii - 1), j * (input[0].Length - 2) + (jj - 1)]
                        }
                    }
                }
            }
        }
    }
}
