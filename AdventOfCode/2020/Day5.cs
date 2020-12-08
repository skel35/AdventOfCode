using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2020
{
    public class Day5 : Solution
    {
        public Day5() : base(5, 2020) { }
        protected override void Solve()
        {
            var nums = ReadLines()
                .Select(line => line
                    .Select(c => c switch {'F' => 0, 'B' => 1, 'R' => 1, 'L' => 0, _ => 0})
                    .Select(x => x == 1)
                    .ToInt());
            // var max = nums.Max();
            // Console.WriteLine(max);

            var valueTuples = nums.OrderBy(x => x).Pairwise((x, y) => (x, x == y - 2)).ToArray();
            var seat = valueTuples.First(t => t.Item2).Item1 + 1;
            Console.WriteLine(seat);
        }
    }
}
