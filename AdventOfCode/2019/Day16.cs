namespace AdventOfCode._2019;

public class Day16 : Solution
{
    public Day16() : base(16, 2019) { }
    protected override void Solve()
    {
        var s = ReadText();
        var offset = int.Parse(s[..7]);
        var ce = (IEnumerable<char>) s;
        int[] se = Enumerable.Repeat(ce, 10000)
            .SelectMany(x => x.Select(c => c.ToInt()))
            .Skip(offset)
            .ToArray();

        var sb = new int[se.Length];
        for (var j = 0; j < 100; j++)
        {
            var sum = 0;
            for (var i = se.Length - 1; i >= 0; i--)
            {
                sum += se[i];
                sb[i] = sum % 10;
            }
            (se, sb) = (sb, se);
        }

        se[..8].Print("");

        Console.WriteLine();
    }
}