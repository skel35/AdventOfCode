using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2020
{
    public class Day12 : Solution
    {
        public Day12() : base(12, 2020) { }
        protected override void Solve()
        {
            var input =
                ReadLines();
            var start = (R: 0, C: 0);
            var cur = start;
            // p1:
            // var dir = (R: 0, C: 1);
            var dir = (R: -1, C: 10);
            foreach (var step in input)
            {
                var n = step[1..].ToInt();
                var s = step[0];
                (cur, dir) = s switch
                {
                    'F' => (cur.Plus(dir.Mult(n)), dir),
                    // p1:
                    // 'N' => (cur.Plus((-1, 0).Mult(n)), dir),
                    // 'S' => (cur.Plus((1, 0) .Mult(n)), dir),
                    // 'E' => (cur.Plus((0, 1) .Mult(n)), dir),
                    // 'W' => (cur.Plus((0, -1).Mult(n)), dir),
                    'N' => (cur, dir.Plus((-1, 0).Mult(n))),
                    'S' => (cur, dir.Plus((1, 0).Mult(n))),
                    'E' => (cur, dir.Plus((0, 1).Mult(n))),
                    'W' => (cur, dir.Plus((0, -1).Mult(n))),
                    'L' => (cur, n switch
                    {
                        90 => dir.Left(),
                        180 => dir.Opposite(),
                        _ => dir.Right()
                    }),
                    'R' => (cur, n switch
                    {
                        90 => dir.Right(),
                        180 => dir.Opposite(),
                        _ => dir.Left()
                    }),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            Console.WriteLine(start.Manhattan(cur));
        }
    }
}
