using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2019;

public class Day10 : Solution
{
    public Day10() : base(10, 2019) { }
    protected override void Solve()
    {
        var grid = ReadLines().ToSb();
        var maxAsteroids = 0;
        var point = (R:  0, C: 0);
        grid.Iter((i, j, ch) =>
        {
            if (ch == '#')
            {
                var d = new Dictionary<int, (int, int)>();
                grid.Iter((r, c, ch2) =>
                {
                    if (r == i && c == j) return;
                    if (ch2 == '#')
                    {
                        var degree = Degree(i - r, j - c);
                        d[degree] = (r, c);
                    }
                });

                if (d.Count > maxAsteroids)
                {
                    maxAsteroids = d.Count;
                    point = (i, j);
                }
            }
        });

        Console.WriteLine(maxAsteroids);

        int Degree(int rDiff, int cDiff)
        {
            var atan = Atan2(-cDiff, rDiff);
            if (atan < 0) atan += 2.0*PI;
            return (int)(18000.0 * atan/PI);
        }

        var ind = 0;
        while (ind <= 200)
        {
            var dict = new Dictionary<int, (int, int)>();
            grid.Iter((r, c, ch) =>
            {
                if (r == point.R && c == point.C) return;
                if (ch == '#')
                {
                    var deg = Degree(point.R - r, point.C - c);
                    if (!dict.ContainsKey(deg)
                        || point.Manhattan((r, c)) < point.Manhattan(dict[deg]))
                    {
                        dict[deg] = (r, c);
                    }
                }
            });
            var ordered = dict.OrderBy(kvp => kvp.Key).ToArray();
            ordered.ForEach(kvp =>
            {
                var pp = kvp.Value;
                grid[pp.Item1][pp.Item2] = '.';
                ind++;
                if (ind == 200)
                {
                    Console.WriteLine((pp.Item2 ) * 100 + (pp.Item1 ));
                }
            });
        }
    }
}