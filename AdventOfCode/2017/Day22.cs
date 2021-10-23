using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2017;

public class Day22 : Solution
{
    public Day22() : base(22, 2017) { }
    protected override void Solve()
    {
        var lines = ReadLines();
        const int n = 5000;
        var smallGrid = lines.Select(str => str.Select(ch => ch == '#' ? 2 : 0).ToArray()).ToArray();
        var centerSmall = (R:  lines.Length / 2, c: lines.Length / 2);
        var centerBig = (n / 2, n / 2).Plus(centerSmall);
        var grid = new int[n, n];
        lines.Length.IterSqr((i, j) => { grid[n / 2 + i, n / 2 + j] = smallGrid[i][j]; });
        var cur = centerBig;
        (int r, int c) dir = (-1, 0);

        var infected = 0;
        for (var i = 0; i < 10000000; i++)
        {
            switch (grid.At(cur))
            {
                case 0:
                    dir = dir.Left();
                    break;
                case 1:
                    break;
                case 2:
                    dir = dir.Right();
                    break;
                case 3:
                    dir = dir.Opposite();
                    break;
            }

            grid.At(cur) = (grid.At(cur) + 1) % 4;
            if (grid.At(cur) == 2)
            {
                infected++;
            }
            cur = cur.Plus(dir);
        }

        Console.WriteLine(infected);
    }
}