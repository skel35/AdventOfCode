namespace AdventOfCode._2021;

public class Day1 : Solution
{
    public Day1() : base(1, 2021) { }
    protected override void Solve()
    {
        var res = ReadLines().Select(int.Parse).Triplewise((a1, a2, a3) => a1 + a2 + a3).Pairwise().Count(t => t.second > t.first);
        Console.WriteLine(res);
    }
}