namespace AdventOfCode._2020;

public class Day10 : Solution
{
    public Day10() : base(10, 2020) { }
    protected override void Solve()
    {
        var a = ReadLines().Select(int.Parse).Prepend(0).OrderBy(x => x).ToList();
        a.Add(a.Max() + 3);
        // var c1 = 0;
        // var c3 = 0;
        // a.Pairwise((a1, a2) =>
        // {
        //     if (a2 - a1 == 1) c1++;
        //     else if (a2 - a1 == 3) c3++;
        //     return a1;
        // }).ToArray();
        // c3++;
        // Console.WriteLine(c1 * c3);

        var current = new List<int>();
        current.Add(0);
        var d = new Dictionary<int, long>();
        d[0] = 1;
        for (var i = 1; i < a.Count; i++)
        {
            while (current[0] < a[i] - 3) current.RemoveAt(0);
            d[a[i]] = current.Sum(c => d[c]);
            current.Add(a[i]);
        }

        Console.WriteLine(d[a.Last()]);

    }
}