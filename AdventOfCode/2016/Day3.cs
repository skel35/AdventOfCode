namespace AdventOfCode._2016;

public class Day3 : Solution
{
    public Day3() : base(3, 2016) { }
    protected override void Solve()
    {
        var res = ReadLines().Select(line => line.ParseInts()).Window(3)
            .Sum(trg => (IsValid(trg[0][0], trg[1][0], trg[2][0]) ? 1 : 0)
                        + (IsValid(trg[0][1], trg[1][1], trg[2][1]) ? 1 : 0)
                        + (IsValid(trg[0][2], trg[1][2], trg[2][2]) ? 1 : 0));
        Console.WriteLine(res);
    }

    bool IsValid(int t1, int t2, int t3)
    {
        return t1 + t2 > t3 && t1 + t3 > t2 && t2 + t3 > t1;
    }
}