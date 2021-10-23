using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2016;

public class Day1 : Solution
{
    public Day1() : base(1, 2016) {}

    protected override void Solve()
    {
        var p = (0, 0);
        var dir = (-1, 0);
        var moves = ReadText().Split2();
        var set = new HashSet<(int, int)> {p};
        foreach (var m in moves)
        {
            dir = m[0] switch
            {
                'L' => dir.Left(),
                'R' => dir.Right(),
                _ => dir
            };

            for (var i = 0; i < m[1..].ToInt(); i++)
            {
                p = p.Plus(dir);
                if (!set.Add(p))
                {
                    Console.WriteLine(p.Manhattan());
                    return;
                }
            }
        }

        // Console.WriteLine(p.Manhattan());
    }
}