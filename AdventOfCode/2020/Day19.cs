using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;
using AdventOfCode;

namespace AdventOfCode._2020
{
    using Rule = Either<string,(int[], int[])>;

    public class Day19 : Solution
    {
        public Day19() : base(19, 2020) { }
        protected override void Solve()
        {
            var input =
                ReadText()
                    .Split("\n\n");
            Dictionary<int, Rule> rules =
                input[0]
                    // comment 2 following lines for part 1:
                .Replace("8: 42\n", "8: 42 | 42 8\n")
                .Replace("11: 42 31\n", "11: 42 31 | 42 11 31\n")

                .Split("\n")
                .Select(line => line.SplitBy(':'))
                .Select(split1 => (split1[0].ToInt(),
                    split1[1]
                        .SplitBy(' ')
                        .If(s => s[0][0] == '"',
                            s => new Rule(s[0][1..^1]),
                            s => new Rule((s.TakeWhile(n => n != "|").Select(int.Parse).ToArray(),
                                s.SkipWhile(n => n != "|").Skip(1).Select(int.Parse).ToArray())))))
                .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);

            Dictionary<(string, int), bool> cache = new();

            bool MatchRule(int ruleIndex, string s)
            {
                if (s.Length == 0) return false;

                if (cache.TryGetValue((s, ruleIndex), out var isMatch))
                    return isMatch;

                var rule = rules[ruleIndex];
                var result =
                    rule.IsLeft
                    ? s == rule.Left
                    : MatchRules(rule.Right.Item1, s)
                      ||
                      (rule.Right.Item2.Any() && MatchRules(rule.Right.Item2, s));
                cache[(s, ruleIndex)] = result;
                return result;
            }

            bool MatchRules(int[] ruleIndices, string s)
                => ruleIndices.Length == 1
                    ? MatchRule(ruleIndices[0], s)
                    : (ruleIndices.Length == 2
                        ? Match2Rules(ruleIndices[0], ruleIndices[1], s)
                        : Match3Rules(ruleIndices[0], ruleIndices[1], ruleIndices[2], s));

            bool Match2Rules(int rule1Index, int rule2Index, string s)
            {
                for (var i = 1; i < s.Length; i++)
                {
                    var p1 = s[..i];
                    var p2 = s[i..];
                    if (MatchRule(rule1Index, s[..i]) && MatchRule(rule2Index, s[i..]))
                        return true;
                }

                return false;
            }

            bool Match3Rules(int rule1Index, int rule2Index, int rule3Index, string s)
            {
                for (var i = 1; i < s.Length - 1; i++)
                {
                    var p1 = s[..i];
                    for (var j = i + 1; j < s.Length; j++)
                    {
                        var p2 = s[i..j];
                        var p3 = s[j..];
                        if (MatchRule(rule1Index, p1) && MatchRule(rule2Index, p2) && MatchRule(rule3Index, p3))
                            return true;
                    }
                }

                return false;
            }


            var messages =
                input[1]
                    .Split("\n");
            var count =
                messages
                    .Count(m => MatchRule(0, m));

            Console.WriteLine(count);
        }
    }
}
