using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2020
{
    public class Day23 : Solution
    {
        public Day23() : base(23, 2020) { }
        protected override void Solve()
        {
            // var input = "476138259";
            var input = "476138259".ToCharArray().Select(c => c.ToInt()).ToList();
            // var input = "389125467".ToCharArray().Select(c => c.ToInt()).ToList();

            var currentIndex = 0;
            for (var i = 0; i < 100; i++)
            {
                var current = input[currentIndex];
                var (take, rest) = input.TakeAndSplit(currentIndex + 1, 3);
                var restList = rest.ToList();
                var destination =
                    restList
                        .MinWithIndex(x => current - x <= 0 ? current - x + 10 : current - x);
                // Console.WriteLine($"destination: {destination.item}");
                restList.InsertRange(destination.index + 1, take);
                currentIndex = (restList.FindIndex(x => x == current) + 1)%restList.Count;
                input = restList;

                // Console.WriteLine(string.Join("", input));
            }

            var indexOf1 = input.FindIndex(x => x == 1);
            var res = string.Join("", input.Skip(indexOf1 + 1).Concat(input.Take(indexOf1)));
            Console.WriteLine(res);
        }
    }
    internal static class ArrayEx
    {
        public static IEnumerable<T> Take<T>(this T[] array, int nToSkip, int nToTake)
        {
            return array.Skip(nToSkip).Take(nToTake).Concat(array.Take(nToSkip + nToTake - array.Length));
        }

        public static (IEnumerable<T>, IEnumerable<T>) TakeAndSplit<T>(this IList<T> array, int nToSkip, int nToTake)
        {
            var take = array.Skip(nToSkip).Take(nToTake).Concat(array.Take(nToSkip + nToTake - array.Count));
            var split = nToSkip + nToTake > array.Count
                ? array.Skip(nToSkip + nToTake - array.Count).Take(array.Count - nToTake)
                : array.Take(nToSkip).Concat(array.Skip(nToSkip + nToTake));
            return (take, split);
        }
    }

}
