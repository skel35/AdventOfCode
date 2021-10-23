using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Math;

namespace AdventOfCode._2019;

public class Day24 : Solution
{
    public Day24() : base(24, 2019) { }

    static BitArray FromSb(StringBuilder sb)
    {
        return new(sb.ToString().Select(ch => ch == '#').ToArray());
    }
    protected override void Solve()
    {
        var grid = ReadLines().ToSb().Select(FromSb).ToArray();
        var N = 202;
        var grids = new BitArray[2*N + 1][];
        for (var i = 0; i < 2 * N + 1; i++)
        {
            grids[i] = new BitArray[grid.Length];
            for (var j = 0; j < grid.Length; j++)
                grids[i][j] = new BitArray(grid[j].Length);
        }
        grids[N] = grid;
        // var set = new HashSet<uint>();
        for (var mins = 0; mins < 200; mins++)
        {
            // uint h = 0;
            var gs = new List<(int gInd, BitArray[] gg)>();
            for (var gridIndex = 0; gridIndex <= mins + 1; gridIndex++)
            {
                // N+gridIndex
                var g = grids[N + gridIndex];
                var g2 = g.ArrayMap(sb => new BitArray(sb));

                for (var i = 0; i < g.Length; i++)
                {
                    for (var j = 0; j < g[i].Length; j++)
                    {
                        if (i == 2 && j == 2)
                        {
                            continue;
                        }

                        var bugs = GenAdjacent((N + gridIndex, (i, j)))
                            .Count(point => grids[point.gridInd].At(point.p));
                        if (g[i][j] && bugs != 1) g2[i][j] = false;
                        if (!g[i][j] && (bugs == 1 || bugs == 2)) g2[i][j] = true;
                    }
                }
                gs.Add((N+gridIndex, g2));


                // N-gridIndex
                if (gridIndex == 0) continue;

                // same
                g = grids[N - gridIndex];
                g2 = g.ArrayMap(sb => new BitArray(sb));

                for (var i = 0; i < g.Length; i++)
                {
                    for (var j = 0; j < g[i].Length; j++)
                    {
                        if (i == 2 && j == 2)
                        {
                            continue;
                        }

                        var adj = GenAdjacent((N - gridIndex, (i, j)));
                        var bugs = adj
                            .Count(point => grids[point.gridInd].At(point.p));
                        if (g[i][j] && bugs != 1) g2[i][j] = false;
                        if (!g[i][j] && (bugs == 1 || bugs == 2)) g2[i][j] = true;
                    }
                }
                gs.Add((N-gridIndex, g2));

            }

            foreach (var (gInd, gg) in gs)
            {
                grids[gInd] = gg;
            }

            // var grid2 = grid.ArrayMap(sb => new BitArray(sb));
            //
            // for (var i = grid.Length - 1; i >= 0; i--)
            // {
            //     for (var j = grid[i].Length - 1; j >= 0; j--)
            //     {
            //         if (grid[i][j]) h++;
            //         if (i != 0 || j != 0) h *= 2;
            //
            //         var bugs = (i, j).Gen4Adjacent()
            //             .Where(p => !p.OutOfBounds(5, 5))
            //             .Count(p => grid.At(p));
            //         if (grid[i][j] && bugs != 1) grid2[i][j] = false;
            //         if (!grid[i][j] && (bugs == 1 || bugs == 2)) grid2[i][j] = true;
            //     }
            // }
            //
            // grid = grid2;

            // if (!set.Add(h))
            // {
            //     Console.WriteLine(h);
            //     return;
            // }
        }

        var sum = grids.Sum(grid => grid.Sum(CountOnes));
        Console.WriteLine(sum);
    }

    IList<(int gridInd, (int r, int c) p)> GenAdjacent((int gridInd, (int r, int c) p) point)
    {
        var (gridInd, p) = point;
        var sameGrid = p.Gen4Adjacent()
            .Where(pp => !pp.OutOfBounds(5, 5))
            .Select(pp => (gridInd, pp))
            .ToList();
        if (p.r == 0) sameGrid.Add((gridInd - 1, (1, 2)));
        if (p.r == 4) sameGrid.Add((gridInd - 1, (3, 2)));
        if (p.c == 0) sameGrid.Add((gridInd - 1, (2, 1)));
        if (p.c == 4) sameGrid.Add((gridInd - 1, (2, 3)));
        if (p == (1, 2))
        {
            var range = Enumerable.Range(0, 5).Select(num => (gridInd + 1, (0, num))).ToArray();
            sameGrid.AddRange(range);
        }
        if (p == (3, 2))
        {
            var range = Enumerable.Range(0, 5).Select(num => (gridInd + 1, (4, num))).ToArray();
            sameGrid.AddRange(range);
        }
        if (p == (2, 1))
        {
            var range = Enumerable.Range(0, 5).Select(num => (gridInd + 1, (num, 0))).ToArray();
            sameGrid.AddRange(range);
        }
        if (p == (2, 3))
        {
            var range = Enumerable.Range(0, 5).Select(num => (gridInd + 1, (num, 4))).ToArray();
            sameGrid.AddRange(range);
        }

        return sameGrid;
        // .SelectMany(pp =>
        // {
        //     // if (pp == (2, 2))
        //     // {
        //     //     // return new[]{(gridInd + 1}
        //     //
        //     // }
        // })

        // return new (int gridInd, (int r, int c) p)[0];
    }

    static int CountOnes(BitArray ba)
    {
        var count = 0;
        for (var i = 0; i < ba.Length; i++)
        {
            if (ba[i])
                count++;
        }
        return count;
    }
}