using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2016
{
    public class Day7 : Solution
    {
        public Day7() : base(7, 2016) { }
        protected override void Solve()
        {
            var res = ReadLines().Select(line =>
            {
                var isValid = false;
                var strings = line.SplitBy('[', ']');

                // bool HasAbba(string s)
                // {
                //     return s[..^3]
                //         .Select((t, i) => s[i] != s[i + 1] && s[i] == s[i + 3] && s[i + 1] == s[i + 2])
                //         .Any(x => x);
                // }

                IEnumerable<string> GetAbas(string s)
                {
                    return s[..^2]
                        .WithIndex()
                        .Where(t => s[t.Index] != s[t.Index + 1] && s[t.Index] == s[t.Index + 2])
                        .Select(t => s[t.Index..(t.Index + 3)]);
                }

                // p1:
                // isValid = strings.Where((s, i) => i % 2 == 0).Any(HasAbba)
                //           && !strings.Where((s, i) => i % 2 == 1).Any(HasAbba);

                var babs = strings.Where((_, i) => i % 2 == 0)
                    .SelectMany(GetAbas)
                    .Select(aba => aba[1].ToString() + aba[0] + aba[1]);

                isValid = strings.Where((_, i) => i % 2 == 1)
                    .Any(s => babs.Any(s.Contains));

                return isValid;
            }).Count(x => x);
            Console.WriteLine(res);
        }
    }
}
