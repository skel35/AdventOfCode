namespace AdventOfCode._2021;

public class Day7 : Solution
{
    public Day7() : base(7, 2021) { }
    protected override void Solve()
    {
        var crabs = ReadText().SplitAndDo(int.Parse);
        // var sum = crabs.SumLong();
        // var count = crabs.Length;
        // sum - N*count -> min
        // sum / count
        // 313
        // 344297
        int step(int n) => (n * (n + 1)) / 2;
        var res = new long[1250];
        var minSum = long.MaxValue;
        for (var i = 0; i < 1250; i++)
        {
            var sum = crabs.Select(c => step(Math.Abs(i - c))).SumLong();
            if (sum < minSum)
            {
                minSum = sum;
            }
        }

        Console.WriteLine(minSum);
    }
}