using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2020;

public class Day6 : Solution
{
    public Day6() : base(6, 2020) { }
    protected override void Solve()
    {
        // p1:
        var sum = ReadText().Split("\n\n")
            .Sum(t => t.Except('\n').ToHashSet().Count);

        // p2:
        sum = ReadText().Split("\n\n")
            .Sum(t => t
                .Split('\n')
                .Aggregate((s, s1) => s.Intersect(s1).ToStr())
                .Length);
        Console.WriteLine(sum);
    }

    public void SolveNoDependencies()
    {
        // p1:
        var sum = ReadText().Split("\n\n")
            .Sum(t => t.Except(new[]{'\n'}).ToHashSet().Count);

        // p2:
        sum = ReadText().Split("\n\n")
            .Sum(t => t
                .Split('\n')
                .Aggregate((s, s1) => string.Concat(s.Intersect(s1)))
                .Length);
        Console.WriteLine(sum);
    }
}