using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2020;

public class Day13 : Solution
{
    public Day13() : base(13, 2020) { }
    protected override void Solve()
    {
        // p1:
        // var lines = ReadLines();
        // var start = lines[0].ToInt();
        // var ts = lines[0].ToInt();
        // var buses = lines[1].SplitBy(',').Where(x => x != "x")
        //     .Select(int.Parse).ToArray();
        // while (true)
        // {
        //     foreach (var bus in buses)
        //     {
        //         if (ts % bus == 0)
        //         {
        //             var res = (ts - start) * bus;
        //             Console.WriteLine(res);
        //             return;
        //         }
        //     }
        //
        //     ts++;
        // }

        var lines = ReadLines();
        var buses = lines[1].SplitBy(',')
            //.Where(x => x != "x")
            .Select(x => int.TryParse(x, out var y) ? y : -1)
            .ToArray();
        var offsets = buses.WithIndex().Where(x => x.Item != -1)
            .OrderByDescending(x => x.Item)
            .ToArray();
        // solved via Wolfram Alpha
        // var num =
        //     525 * 1839692279113 * 438 +
        //     396 * 2519486192873 * 324 +
        //     14 * 26608232232049 * 29 +
        //     21 * 37618535224621 * 2 +
        //     1 * 47432066152783 * 6 +
        //     1 * 57417764290211 * 8 +
        //     14 * 64172795383177 * 15 +
        //     10 * 83918270885693 * 4;
        // var lcm = 1090937521514009; // Wolfram Alpha
        // var num = 776257269215190994 % lcm;
        var num = 600691418730595;
        for (var ts = num;;)
        {
            var all = true;
            for (var i = 0; i < offsets.Length; i++)
            {
                var offset = (ts + offsets[i].Index) % offsets[i].Item;
                if (offset != 0)
                {
                    ts += offsets[i].Item - offset;
                    if (i >= 1)
                    {
                        all = false;
                        break;
                    }
                }
            }

            if (all)
            {
                Console.WriteLine(ts);
                return;
            }
        }
    }
}