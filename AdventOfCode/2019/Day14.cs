using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2019
{
    public class Day14 : Solution
    {
        public Day14() : base(14, 2019) { }
        protected override void Solve()
        {
            var dict = ReadLines().ArrayMap(line => line.Split("=>"))
                .ToDictionary(r => ParseElement(r[1]).Item2,
                    r => r[0].SplitBy(',').Select(ParseElement).ToArray());
            dict["ORE"] = new (long, string)[0];
            
            var dict2 = ReadLines().ArrayMap(line => line.Split("=>"))
                .ToDictionary(r => ParseElement(r.Last()).Item2,
                    r => ParseElement(r.Last()).Item1);
            dict2["ORE"] = 1;
            var oreCount = 0L;
            long trillion = 1000000000000L;
            var fuelStart = 90000;
            var l = fuelStart;
            var r = fuelStart * 40;
            while (r > l)
            {
                var mid = (l + r) / 2;
                oreCount = OreCount(dict, dict2, mid);
                if (oreCount <= trillion)
                    l = mid + 1;
                else r = mid - 1;
            }

            if (OreCount(dict, dict2, l) <= trillion)
            {
                Console.WriteLine(l);
            }
            else
            {
                Console.WriteLine(l - 1);
            }

            // long fuel = trillion / (long) oreCount;
            // Console.WriteLine(oreCount);
            // Console.WriteLine(fuelStart);
        }

        private static long OreCount(Dictionary<string, (long, string)[]> dict, Dictionary<string, long> dict2,
            long fuel)
        {
            var needed = new Dictionary<string, long> {{"FUEL", fuel}};
            var used = new Dictionary<string, long>();
            var oreCount = 0L;
            while (needed.Count > 0)
            {
                var key = needed.First().Key;
                var howMuch = needed[key];
                needed.Remove(key);
                if (key == "ORE")
                {
                    oreCount += howMuch;
                    if (oreCount < 0)
                    {
                        Console.Write("neg");
                    }
                    continue;
                }

                if (used.ContainsKey(key))
                {
                    var found = used[key];
                    if (found == howMuch)
                    {
                        used.Remove(key);
                        continue;
                    }

                    if (found < howMuch)
                    {
                        used.Remove(key);
                        howMuch -= found;
                    }
                    else if (found > howMuch)
                    {
                        used[key] = found - howMuch;
                        continue;
                    }
                }

                var reaction = dict[key];
                // var numReactions = (int)Ceiling((double) dict2[key] / howMuch);
                var numReactions = (long) Ceiling((double) howMuch / dict2[key]);
                var numExtra = dict2[key] * numReactions - howMuch;
                reaction.ForEach(r =>
                {
                    if (needed.ContainsKey(r.Item2))
                        needed[r.Item2] = needed[r.Item2] + (r.Item1 * numReactions);
                    else needed[r.Item2] = (r.Item1 * numReactions);
                });
                if (used.ContainsKey(key)) used[key] = used[key] + numExtra;
                else used[key] = numExtra;
            }

            return oreCount;
        }

        (long, string) ParseElement(string el)
        {
            var s = el.SplitBy(' ');
            return (int.Parse(s[0]), s[1]);
        }
    }
}
