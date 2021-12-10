using System.Numerics;

namespace AdventOfCode._2021;

public class Day10 : Solution
{
    public Day10() : base(10, 2021) { }
    protected override void Solve()
    {
        var lines = ReadLines();
        var opening = "([{<";
        var closing = new Dictionary<char, char>()
        {
            ['('] = ')',
            ['['] = ']',
            ['{'] = '}',
            ['<'] = '>',
        };
        var points = new Dictionary<char, int>
        {
            [')'] = 3,
            [']'] = 57,
            ['}'] = 1197,
            ['>'] = 25137,
        };
        // var points2 = new Dictionary<char, int>
        // {
        //     [')'] = 1,
        //     [']'] = 2,
        //     ['}'] = 3,
        //     ['>'] = 4,
        // };

        var illegal = new List<char>();
        var scores = new List<BigInteger>();
        lines.ForEach(
            line =>
            {
                var stack = new Stack<char>();
                foreach (var ch in line)
                {
                    if (opening.Contains(ch))
                    {
                        stack.Push(ch);
                    }
                    else if (stack.Count == 0
                             || ch != closing[stack.Pop()])
                    {
                        illegal.Add(ch);
                        return;
                    }
                }

                var score = stack.Aggregate(new BigInteger(0), (score, ch) => score * 5 + opening.IndexOf(ch) + 1);
                scores.Add(score);
            });
        // p1:
        // var res = illegal.SumLong(ch => points[ch]);
        // Console.WriteLine(res);

        // p2:
        scores.Sort();
        Console.WriteLine(scores[scores.Count / 2]);
    }
}