using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2020;

public class Day2 : Solution
{
    public Day2() : base(2, 2020) { }
    protected override void Solve()
    {
        // ReadLines();
        // ReadText().ParseInts();
        var lines = ReadLines();
        var p1 = 0;
        var p2 = 0;
        foreach (var line in lines)
        {
            var s = line.SplitBy(' ', ':');
            var a = s[0].SplitBy('-').Select(int.Parse).ToArray();
            var count = s[2].Count(c => c == s[1][0]);

            if (count >= a[0] && count <= a[1]) p1++;
            if ((s[2][a[0] - 1] == s[1][0]) ^ (s[2][a[1] - 1] == s[1][0])) p2++;
        }

        Console.WriteLine(p1);
        Console.WriteLine(p2);

    }
}