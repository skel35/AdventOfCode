using System.Text;

namespace AdventOfCode._2016;

public class Day9 : Solution
{
    public Day9() : base(9, 2016) { }
    protected override void Solve()
    {
        var input =
            ReadLines()
                .Select(TransformLength)
                // .Select(s => s.Length)
                .Sum();
        Console.WriteLine(input);

        long TransformLength(string s)
        {
            var length = 0L;
            for (var i = 0; i < s.Length; i++)
            {
                if (s[i] == '(')
                {
                    var indexOfClosingBracket = s.IndexOf(')', i + 1);
                    var n = s[(i + 1)..indexOfClosingBracket].SplitBy('x').Select(int.Parse).ToArray();
                    length += n[1] * TransformLength(
                        s[(indexOfClosingBracket + 1)..(indexOfClosingBracket + 1 + n[0])]);
                    i = indexOfClosingBracket + n[0];
                }
                else
                {
                    length++;
                }
            }

            return length;
        }

        // p1:
        // var res = ReadLines()
        // .Select(Transform)
        // .Select(s => s.Length)
        // .Sum();
        string Transform(string s)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < s.Length; i++)
            {
                if (s[i] == '(')
                {
                    var indexOfClosingBracket = s.IndexOf(')', i + 1);
                    var n = s[(i + 1)..indexOfClosingBracket].SplitBy('x').Select(int.Parse).ToArray();
                    for (var j = 0; j < n[1]; j++)
                        sb.Append(Transform(s[(indexOfClosingBracket + 1)..(indexOfClosingBracket + 1 + n[0])]));
                    i = indexOfClosingBracket + n[0];
                }
                else
                {
                    sb.Append(s[i]);
                }
            }

            return sb.ToString();
        }
    }
}