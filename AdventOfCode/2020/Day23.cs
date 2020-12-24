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
            var input = "476138259".ToCharArray().Select(c => c.ToInt()).ToList();
            var dict = new Dictionary<int, LinkedListNode<int>>();
            var ll = new LinkedList<int>();

            var maxValue = 1_000_000;

            void AddNode(int x)
            {
                var node = new LinkedListNode<int>(x);
                ll.AddLast(node);
                dict.Add(x, node);
            }
            input.ForEach(AddNode);

            Enumerable.Range(10, maxValue - 9)
                .ForEach(AddNode);
            var currentNode = ll.First!;
            for (var i = 0; i < 10_000_000; i++)
            {
                var skipped1 = ll.NextCircular(currentNode, 1);
                var skipped2 = ll.NextCircular(skipped1, 1);
                var skipped3 = ll.NextCircular(skipped2, 1);
                ll.Remove(skipped1);
                ll.Remove(skipped2);
                ll.Remove(skipped3);
                bool IsSkipped(int x) => x == skipped1.Value ||
                                         x == skipped2.Value ||
                                         x == skipped3.Value;

                int Prev(int x) => x - 1 <= 0 ? maxValue : x - 1;
                var destinationValue = currentNode.Value;
                do
                {
                    destinationValue = Prev(destinationValue);
                } while (IsSkipped(destinationValue));

                var destinationNode = dict[destinationValue];
                ll.AddAfter(destinationNode, skipped1);
                ll.AddAfter(skipped1, skipped2);
                ll.AddAfter(skipped2, skipped3);

                currentNode = ll.NextCircular(currentNode, 1);
            }

            var one = dict[1];
            var oneNext = one.Next;
            var oneNextNext = oneNext.Next;
            var res = (long) oneNext.Value * (long) oneNextNext.Value;
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
