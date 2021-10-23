using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Math;

namespace AdventOfCode._2016;

public class Day6 : Solution
{
    public Day6() : base(6, 2016) { }
    protected override void Solve()
    {
        // solution 1:
        var dicts = new Dictionary<char, int>[8];
        dicts.Iteri((i, _) => dicts[i] = new Dictionary<char, int>());
        var res = ReadLines()
            .Aggregate(dicts, (dicts, str) =>
            {
                str.Iteri((i, ch) => { dicts[i].AddOrUpdate(ch, 1, (_, v) => v + 1); });
                return dicts;
            })
            .Select(d => d.MinBy(kvp => kvp.Value))
            .Select(kvp => kvp.Key)
            .ToStr();

        Console.WriteLine(res);

        // solution 2:
        var res2 = ReadLines()
            .SelectMany(str => str.WithIndex())
            .GroupBy(tuple => tuple.Index)
            .OrderBy(g => g.Key)
            .Select(g => g.GroupBy(tuple => tuple.Item)
                .Select(gg => (ch: gg.Key, count: gg.Count()))
                .OrderBy(tuple => tuple.count)
                .Select(tuple => tuple.ch)
                .First())
            .ToStr();

        Console.WriteLine(res2);
    }
}