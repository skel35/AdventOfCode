using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2020;

public class Day1 : Solution
{
    public Day1() : base(1, 2020) { }
    protected override void Solve()
    {
        // var lines = ReadLines();
        var a = ReadText().ParseInts();
        for (var i = 0; i < a.Length; i++)
        {
            for (var j = i + 1; j < a.Length; j++)
            {
                if (a[i] + a[j] == 2020) Console.WriteLine("Part 1: " + a[i]*a[j]);
                for (var k = j + 1; k < a.Length; k++)
                {
                    if (a[i] + a[j] + a[k] == 2020) Console.WriteLine("Part 2: " + a[i]*a[j]*a[k]);
                }
            }
        }

        // alternative solution with helper methods:
        var p1 = a.Pairs().First(t => t.first + t.second == 2020);
        Console.WriteLine(p1.first * p1.second);
        var p2 = a.Triples().First(t => t.first + t.second + t.third == 2020);
        Console.WriteLine(p2.first * p2.second * p2.third);

        // faster solution with HashSets:
        var set = a.Select(x => 2020 - x).ToHashSet();
        var y = a.First(set.Contains);
        Console.WriteLine(y * (2020-y));
        var set2 = new HashSet<int>();
        a.IterSqr2((x, y) => set2.Add(2020 - x - y));
        var z = a.First(set2.Contains);
        var set3 = a.Select(x => 2020 - z - x).ToHashSet();
        var zz = a.First(set3.Contains);
        var res = z * zz * (2020 - z - zz);
        Console.WriteLine(res);
    }
}