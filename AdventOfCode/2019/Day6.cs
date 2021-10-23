using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2019;

public class Day6 : Solution
{
    public Day6() : base(6, 2019) { }
    protected override void Solve()
    {
        var dict = new Dictionary<string, string>();
        var count = new Dictionary<string, int>();
        ReadLines().ArrayMap(line => line.SplitBy(')'))
            .ForEach(array => { dict[array[1]] = array[0]; });

        int Orbits(string s)
        {
            var res = 0;
            if (count.ContainsKey(s)) return count[s];
            if (dict.ContainsKey(s))
            {
                res += Orbits(dict[s]);
                res += 1;
            }

            count[s] = res;
            return res;
        }

        // part 1
        var sum = dict.Sum(kvp => Orbits(kvp.Key));
        Console.WriteLine(sum);

        var set = new Dictionary<string, int>();
        var steps = 0;
        var l = dict["YOU"];
        var r = dict["SAN"];
        while (true)
        {
            if (l == r)
            {
                Console.WriteLine(steps * 2);
                return;
            }

            if (!set.ContainsKey(l))
                set.Add(l, steps);
            if (!set.ContainsKey(r))
                set.Add(r, steps);
            steps++;

            if (l != "COM")
            {
                l = dict[l];
                if (set.ContainsKey(l))
                {
                    Console.WriteLine(steps + set[l]);
                    return;
                }

            }

            if (r != "COM")
            {
                r = dict[r];
                if (set.ContainsKey(r))
                {
                    Console.WriteLine(steps + set[r]);
                    return;
                }
            }
        }
    }
}