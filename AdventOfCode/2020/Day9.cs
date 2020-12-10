using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2020
{
    public class Day9 : Solution
    {
        public Day9() : base(9, 2020) { }
        protected override void Solve()
        {
            var nums = ReadLines().Select(long.Parse).ToArray();
            var P = 25;
            // for (var i = P; i < nums.Length; i++)
            // {
            //     bool good = false;
            //     for (var j = i - P; j < i; j++)
            //     {
            //         for (var k = j + 1; k < i; k++)
            //         {
            //             if (nums[i] == nums[j] + nums[k])
            //             {
            //                 good = true;
            //                 break;
            //             }
            //         }
            //
            //         if (good) break;
            //     }
            //
            //     if (!good)
            //     {
            //         Console.WriteLine(nums[i]);
            //         return;
            //     }
            // }
            var N = 69316178;
            var sum = 0L;
            var iStart = 0;
            sum += nums[iStart];
            for (var iEnd = 1; iEnd < nums.Length; iEnd++)
            {
                sum += nums[iEnd];
                if (sum == N)
                {
                    Console.WriteLine(nums[iStart] + nums[iEnd]);
                    break;
                }
                else
                {
                    while (sum > N && iStart < iEnd - 1)
                    {
                        sum -= nums[iStart];
                        iStart++;
                    }
                    if (sum == N)
                    {
                        // Console.WriteLine(nums[iStart] + nums[iEnd]);
                        // var sum2 = nums.Skip(iStart).Take(iEnd - iStart + 1).Sum();
                        // Console.WriteLine(sum2);
                        var array = nums.Skip(iStart).Take(iEnd - iStart + 1).ToArray();
                        Console.WriteLine(array.Min() + array.Max());
                        break;
                    }
                }
            }

            // not 8427069
        }
    }
}
