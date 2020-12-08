using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2020
{
    public class Day3 : Solution
    {
        public Day3() : base(3, 2020) { }
        protected override void Solve()
        {
            var grid = ReadLines();
            var R = grid.R();
            var C = grid.C();

            var res = new[] {(1, 1), (1, 3), (1, 5), (1, 7), (2, 1)}
                .Map(d =>
                    (R: 0, C: 0)
                    .Unfold(pos => pos.R >= R ? null : (pos, (pos.Plus(d).R, pos.Plus(d).C % C)))
                    .Count(pos => grid.At(pos) == '#'))
                .Select(x => (long)x)
                .Product();

            Console.WriteLine(res);
        }
    }
}
