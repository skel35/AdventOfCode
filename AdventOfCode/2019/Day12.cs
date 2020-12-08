using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2019
{
    public class Day12 : Solution
    {
        public Day12() : base(12, 2019) { }
        protected override void Solve()
        {
            var pos = new[]
            {
                (x: 9, y: 13, z: -8),
                (x: -3, y: 16, z: -17),
                (x: -4, y: 11, z: -10),
                (x: 0, y: -2, z: -2)
            };
            var pos2 = new[]
            {
                (x: 9, y: 13, z: -8),
                (x: -3, y: 16, z: -17),
                (x: -4, y: 11, z: -10),
                (x: 0, y: -2, z: -2)
            };

            // var pos = new[]
            // {
            //     (x: -8, y: -10, z: 0),
            //     (x: 5, y: 5, z: 10),
            //     (x: 2, y: -7, z: 3),
            //     (x: 9, y: -8, z: -3)
            // };
            // var pos = new[]
            // {
            //     (x: -1, y: 0, z: 2),
            //     (x: 2, y: -10, z: -7),
            //     (x: 4, y: -8, z: 8),
            //     (x: 3, y: 5, z: -1)
            // };
            var v = Enumerable.Repeat((x: 0, y: 0, z: 0), 4).ToArray();
            for (long s = 0; s < 1_000_000; s++)
            {
                for (var i = 0; i < 3; i++)
                {
                    for (var j = i + 1; j < 4; j++)
                    {
                        if (i == j) continue;
                        var sgn = Math.Sign(pos[i].x - pos[j].x);
                        v[i].x -= sgn;
                        v[j].x += sgn;
                         sgn = Math.Sign(pos[i].y - pos[j].y);
                        v[i].y -= sgn;
                        v[j].y += sgn;
                         sgn = Math.Sign(pos[i].z - pos[j].z);
                        v[i].z -= sgn;
                        v[j].z += sgn;
                    }
                }

                for (var i = 0; i < 4; i++)
                {
                    pos[i] = pos[i].Plus(v[i]);
                }

                bool xEquals((int x, int y, int z)[] a, (int x, int y, int z)[] b)
                {
                    for (var i = 0; i < a.Length; i++)
                    {
                        if (a[i].x != b[i].x) return false;
                    }
                    return true;
                }
                bool yEquals((int x, int y, int z)[] a, (int x, int y, int z)[] b)
                {
                    for (var i = 0; i < a.Length; i++)
                    {
                        if (a[i].y != b[i].y) return false;
                    }
                    return true;
                }

                bool zEquals((int x, int y, int z)[] a, (int x, int y, int z)[] b)
                {
                    for (var i = 0; i < a.Length; i++)
                    {
                        if (a[i].z != b[i].z) return false;
                    }

                    return true;
                }

                if (xEquals(pos, pos2))
                {
                    Console.WriteLine("x period");
                    Console.WriteLine(s + 1);
                    v.Print();
                }

                if (yEquals(pos, pos2))
                {
                    Console.WriteLine("y period");
                    Console.WriteLine(s + 1);
                    v.Print();
                }

                if (zEquals(pos, pos2))
                {
                    Console.WriteLine("z period");
                    Console.WriteLine(s + 1);
                    v.Print();
                }
            }

            // part 1:
            // var tot = 0L;
            // for (var i = 0; i < 4; i++)
            // {
            //     var pot = Abs(pos[i].x) + Abs(pos[i].y) + Abs(pos[i].z);
            //     var kin = Abs(v[i].x) + Abs(v[i].y) + Abs(v[i].z);
            //     tot += pot * kin;
            // }
            //
            // Console.WriteLine(tot);

            // part 2:
            // LCM in an online calculator

        }
    }
}
