namespace AdventOfCode._2016;

public class Day4 : Solution
{
    public Day4() : base(4, 2016) { }
    protected override void Solve()
    {
        var res = ReadLines()
            .Sum(line =>
            {
                var bracketIndex = line.IndexOf('[');
                var dashIndex = line.LastIndexOf('-', bracketIndex);
                var n = line[(dashIndex + 1)..bracketIndex].ToInt();
                var name = line[..dashIndex];
                var grouped = name.Without('-').GroupBy(c => c)
                    .OrderByDescending(g => g.Count()).ThenBy(g => g.Key).Take(5).Select(g => g.Key).ToStr();
                var checksum = line[(bracketIndex + 1)..^1];
                if (grouped == checksum)
                {
                    var r = name.Select(c =>
                    {
                        if (c == '-') return ' ';
                        return (char) ('a' + ((c - 'a') + n) % ('z' - 'a' + 1));
                    }).ToStr();
                    Console.WriteLine(r + ": " + n);
                    return n;
                }
                return 0;
            });
        Console.WriteLine(res);
    }
}