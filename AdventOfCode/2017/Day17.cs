using System;
using System.Collections.Generic;
using System.IO;
using static System.Math;

namespace AdventOfCode._2017
{
    public class Day17 : Solution
    {
        public Day17() : base(17, 2017) { }
        protected override void Solve()
        { 
            // var input = File.ReadAllText("input.txt");
            // var lines = File.ReadAllLines("input.txt");
            var skip = 371;

            void Part1()
            {
                var list = new List<int>();
                list.Add(0);
                var current = 0;
                for (var i = 0; i < 2017; i++)
                {
                    current = (current + skip) % (i + 1);
                    list.Insert(current, i + 1);
                    current++;
                }

                Console.WriteLine(list[current + 1]);
            }

            Part1();

            void Part2()
            {
                var current = 0;
                var res = -1;
                for (var i = 1; i <= 50_000_000; i++)
                {
                    current = 1 + (current + skip) % i;
                    if (current == 1)
                    {
                        res = i;
                    }
                }

                Console.WriteLine(res);
            }
            
            Part2();
        }
    }
}