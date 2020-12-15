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
                "8,0,17,4,1,12".SplitBy(',').Select(int.Parse).ToArray();
            var dict = new Dictionary<int, int>();
            var dict2 = new Dictionary<int, int>();
            for (var i = 0; i < input.Length; i++)
            {
                dict.Add(input[i], i);
            }

            int prev = input.Last();

            var index = input.Length;
            while (true)
            {
                int num;
                if (!dict2.ContainsKey(prev))
                {
                    num = 0;
                }
                else
                {
                    num = dict[prev] - dict2[prev];
                }

                if (dict.ContainsKey(num))
                    dict2[num] = dict[num];
                dict[num] = index;
                if (index == 30000000 - 1)
                {
                    Console.WriteLine(num);
                    return;
                }
                // Console.WriteLine(num);

                index++;
                prev = num;
            }

        }
    }
}
