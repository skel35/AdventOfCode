using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2020
{
    public class Day11 : Solution
    {
        public Day11() : base(11, 2020) { }
        protected override void Solve()
        {
            var input =
                ReadLines()
                .Select(line => line.Split())
                .ToArray();

        }
    }
}
