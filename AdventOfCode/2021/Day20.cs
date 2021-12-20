using System.Text;

namespace AdventOfCode._2021;

public class Day20 : Solution
{
    public Day20() : base(20, 2021) { }
    protected override void Solve()
    {
        var lines = ReadLines();
        var alg = lines[0];
        var grid = lines.Skip(2).ToArray().ToSb();
        var flipEmpty = alg[0] == '#';
        for (var i = 0; i < 50; i++)
        {
            var g2 = new StringBuilder[grid.Length + 2];
            for (var r = 0; r < grid.Length + 2; r++)
            {
                var sb = new StringBuilder();
                for (var c = 0; c < grid[0].Length + 2; c++)
                {
                    var val = (r - 1, c - 1)
                        .Gen9Adjacent()
                        .Select(p => p.InBounds(grid) ? grid.At(p) == '#' : (i % 2 == 1) ? flipEmpty : false)
                        .ToInt();
                    var processed = alg[val];
                    sb.Append(processed);
                }

                g2[r] = sb;
            }

            grid = g2;
            // grid.Print();

        }

        var res = grid.Select(g => g.ToString().Count(ch => ch == '#')).SumLong();
        res.ToString().Print();
    }
}