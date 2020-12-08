using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;
using static AdventOfCode.AoC;

namespace AdventOfCode._2019
{
    public class Day2 : Solution
    {
        public Day2() : base(2, 2019) { }
        protected override void Solve()
        {
            var n = ReadText().ParseInts();
            var ncopy = new int[n.Length];
            Array.Copy(n, ncopy, n.Length);
            const int expected = 19690720;
            // var n = new[] {2,4,4,5,99,0};
            // n[1] = 12;
            // n[2] = 2;
            for (var i1 = 0; i1 < 100; i1++)
            {
                for (var i2 = 0; i2 < 100; i2++)
                {
                    Array.Copy(ncopy, n, n.Length);
                    n[1] = i1;
                    n[2] = i2;
                    var res = Process(n);
                    if (res == expected)
                    {
                        Console.WriteLine(i1 * 100 + i2);
                        return;
                    }
                    else
                    {
                        Console.WriteLine($"unsuccessful: {i1} {i2} res: {res}");
                    }
                }
            }

            Console.WriteLine("not found");
        }

        int Process(int[] n)
        {
            for (var i = 0; i < n.Length; i += 4)
            {
                var opcode = n[i];
                if (opcode == 99)
                {
                    return n[0];
                }

                if (opcode == 1)
                {
                    n[n[i + 3]] = n[n[i + 1]] + n[n[i + 2]];
                }

                if (opcode == 2)
                {
                    n[n[i + 3]] = n[n[i + 1]] * n[n[i + 2]];
                }
            }

            return -1;
        }
    }
}