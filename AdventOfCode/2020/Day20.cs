using static System.Math;

namespace AdventOfCode._2020;

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

        int BorderIndex(int tile, int tileToMatch)
        {
            return
                tileToBorders[tile]
                    .Take(4)
                    .WithIndex()
                    .Single(t => tileToBorders[tileToMatch].Contains(t.Item))
                    .Index;
        }

        char[,] Rotate2(char[,] tile, int clockwise)
        {
            (tile.GetLength(1) == tile.GetLength(0)).Assert();
            var res = new char[tile.GetLength(0), tile.GetLength(1)];
            for (var i = 0; i < tile.GetLength(0); i++)
            {
                for (var j = 0; j < tile.GetLength(1); j++)
                {
                    if (clockwise == 0)
                    {
                        res[i, j] = tile[i,j];
                    }
                    else if (clockwise == 180)
                    {
                        res[tile.GetLength(0) - 1 - i, tile.GetLength(0) - 1 - j] = tile[i,j];
                    }
                    else if (clockwise == 90)
                    {
                        res[j, tile.GetLength(0) - 1 - i] = tile[i,j];
                    }
                    else if (clockwise == 270)
                    {
                        res[tile.GetLength(0) - 1 - j, i] = tile[i,j];
                    }
                }
            }
            return res;
        }


        char[,] Rotate(string[] tile, int clockwise)
        {
            (tile.Length == tile[0].Length).Assert();
            var res = new char[tile.Length, tile[0].Length];
            for (var i = 0; i < tile.Length; i++)
            {
                for (var j = 0; j < tile[0].Length; j++)
                {
                    if (clockwise == 0)
                    {
                        res[i, j] = tile[i][j];
                    }
                    else if (clockwise == 180)
                    {
                        res[tile.Length - 1 - i, tile.Length - 1 - j] = tile[i][j];
                    }
                    else if (clockwise == 90)
                    {
                        res[j, tile.Length - 1 - i] = tile[i][j];
                    }
                    else if (clockwise == 270)
                    {
                        res[tile.Length - 1 - j, i] = tile[i][j];
                    }
                }
            }
            return res;
        }

        void FlipHorizontal(char[,] tile)
        {
            for (var j = 0; j < tile.GetLength(0); j++)
            {
                for (var i = 0; i < tile.GetLength(1) / 2; i++)
                {
                    AoC.Swap(ref tile[j, i], ref tile[j, tile.GetLength(1) - 1 - i]);
                }
            }
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
                resultGrid[i, j] =
                    LinedUp(resultGrid[i, j - 1])
                        .Intersect(
                            LinedUp(resultGrid[i - 1, j]))
                        .Except(resultGrid[i - 1, j - 1])
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

        var tileSize = input.Values.First().Length;
        var sizeTrimmed = tileSize - 2;
        var resultTilesSize = resultGrid.GetLength(0) * sizeTrimmed;
        var resultTiles = new char[resultTilesSize, resultTilesSize];

        for (var i = 0; i < resultGrid.GetLength(0); i++)
        {
            for (var j = 0; j < resultGrid.GetLength(1); j++)
            {
                // rotate&flip to align with neighbors
                var tileNum = resultGrid[i, j];
                var tile = input[tileNum];
                // var borders = tileToBorders[tileNum];
                char[,] rotated;
                if (i == 0)
                {
                    var borderIndex = BorderIndex(tileNum, resultGrid[i + 1, j]);
                    rotated = Rotate(tile, borderIndex switch
                    {
                        0 => 180,
                        1 => 0,
                        2 => 270,
                        3 => 90,
                        _ => throw new InvalidOperationException()
                    });
                }
                else
                {
                    var borderIndex = BorderIndex(tileNum, resultGrid[i - 1, j]);
                    rotated = Rotate(tile, borderIndex switch
                    {
                        0 => 0,
                        1 => 180,
                        2 => 90,
                        3 => 270,
                        _ => throw new InvalidOperationException()
                    });
                }

                // rotated.Print();

                if (j == 0)
                {
                    var rightBorder = (0..tileSize).Linq().Select(x => rotated[x, tileSize - 1]).ToStr();
                    // check if matches with resultGrid[i, j + 1]
                    var isMatch = tileToBorders[resultGrid[i, j + 1]].Contains(rightBorder);
                    // if not -> flip horizontally
                    if (!isMatch)
                    {
                        FlipHorizontal(rotated);
                    }
                }
                else
                {
                    var leftBorder = (0..tileSize).Linq().Select(x => rotated[x, 0]).ToStr();
                    var isMatch = tileToBorders[resultGrid[i, j - 1]].Contains(leftBorder);
                    // if not -> flip horizontally
                    if (!isMatch)
                    {
                        FlipHorizontal(rotated);
                    }
                }

                for (var ii = 1; ii < rotated.GetLength(0) - 1; ii++)
                {
                    for (var jj = 1; jj < rotated.GetLength(1) - 1; jj++)
                    {
                        resultTiles[i * sizeTrimmed + (ii - 1), j * sizeTrimmed + (jj - 1)] = rotated[ii, jj];
                    }
                }
            }
        }
        // FlipHorizontal(resultTiles);
        resultTiles = Rotate2(resultTiles, 90);

        resultTiles.Print();


        var pattern = new int[,]
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
            {1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 1},
            {0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0},
        };

        bool IsPattern(char[,] bigTile, int rowOffset, int colOffset)
        {
            if (rowOffset + pattern.GetLength(0) > bigTile.GetLength(0)) return false;
            if (colOffset + pattern.GetLength(1) > bigTile.GetLength(1)) return false;
            var res = true;
            pattern.Iter((r, c, item) =>
            {
                if (item == 0) return;
                if (bigTile[rowOffset + r, colOffset + c] != '#') res = false;
            });
            return res;
        }

        HashSet<(int, int)> seaMonster = new();

        var count = 0;
        for (var i = 0; i < resultTiles.GetLength(0); i++)
        {
            for (var j = 0; j < resultTiles.GetLength(1); j++)
            {
                if (IsPattern(resultTiles, i, j))
                {
                    var ii = i;
                    var jj = j;
                    pattern.Iter((r, c, item) =>
                    {
                        if (item == 1) seaMonster.Add((ii + r, jj + c));
                    });
                }

                if (resultTiles[i, j] == '#') count++;
            }
        }

        count -= seaMonster.Count;
        Console.WriteLine(count);
    }
}