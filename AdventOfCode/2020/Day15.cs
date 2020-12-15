using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2020
{
    public class Day15 : Solution
    {
        public Day15() : base(15, 2020) { }
        protected override void Solve()
        {
            var input =
                new[] {8, 0, 17, 4, 1, 12};
            
            var last = new Dictionary<int, int>();
            var nextToLast = new Dictionary<int, int>();
            input.WithIndex().ForEach(t => last.Add(t.Item, t.Index));

            var prev = input.Last();
            var index = input.Length;
            while (true)
            {
                var num =
                    nextToLast.ContainsKey(prev)
                    ? (index - 1) - nextToLast[prev]
                    : 0;

                if (last.ContainsKey(num))
                    nextToLast[num] = last[num];
                last[num] = index;
                if (index == 2020 - 1)
                {
                    Console.WriteLine(num);
                    return;
                }

                index++;
                prev = num;
            }

        }
    }
}
