namespace AdventOfCode._2019;

public class Day4 : Solution
{
    public Day4() : base(4, 2019) { }
    protected override void Solve()
    {
        var start = 245182;
        var stop = 790572;
        var count = Enumerable.Range(start, stop - start + 1).Count(IsValid);
        Console.WriteLine(count);
    }

    bool IsValid(int n)
    {
        var s = n.ToString();
        return s.Where((_, i) => i > 0 && s[i] < s[i - 1]).Any() == false
               && s.GroupBy(c => c).Any(g => g.Count() == 2);
    }
}