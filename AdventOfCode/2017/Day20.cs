using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;
using E = AdventOfCode.AoC;

namespace AdventOfCode._2017
{
    public class Day20 : Solution
    {
        public Day20() : base(20, 2017) { }
        protected override void Solve()
        {
            var lines = ReadLines();

            T3 ParseTuple(char ch, string line)
            {
                var s = line.Ss(ch + "=<", ">").ParseInts();
                return (s[0], s[1], s[2]);
            }

            var list = new List<(T3 p, T3 v, T3 acc)>();

            for (var index = 0; index < lines.Length; index++)
            {
                var line = lines[index];
                var acc = ParseTuple('a', line);
                var p = ParseTuple('p', line);
                var v = ParseTuple('v', line);
                list.Add((p, v, acc));
            }

            (T3 p, T3 v, T3 acc) Next((T3 p, T3 v, T3 acc) current)
            {
                (T3 p, T3 v, T3 acc) res;
                res.v = E.Plus(current.v, current.acc);
                res.p = E.Plus(current.p, current.v);
                res.acc = current.acc;
                return res;
            }

            var N = 15000;
            for (var i = 0; i < N; i++)
            {
                var dict = new Dictionary<T3, List<int>>();
                for (var j = 0; j < list.Count; j++)
                {
                    list[j] = Next(list[j]);
                    if (!dict.ContainsKey(list[j].p))
                    {
                        dict.Add(list[j].p, new List<int>(){j});
                    }
                    else dict[list[j].p].Add(j);
                }
                dict.Where(kvp => kvp.Value.Count > 1).SelectMany(kvp => kvp.Value).OrderByDescending(ind => ind).ForEach(ind => list.RemoveAt(ind));
            }

            Console.WriteLine(list.Count);
        }
    }
}