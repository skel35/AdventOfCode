namespace AdventOfCode._2021;

public class Day3 : Solution
{
    public Day3() : base(3, 2021) { }
    protected override void Solve()
    {
        var lines = ReadLines();
        var counts = lines
            .Aggregate(
                Enumerable.Repeat(0, 12).ToArray(),
                (counts, binaryNum) =>
                {
                    binaryNum.Iteri((i, ch) => counts[i] += ch == '1' ? 1 : 0);
                    return counts;
                });
        var gammaRate = counts.Select(c => c > lines.Length / 2).ToArray();
        var gammaRateInt = gammaRate.ToInt();
        var epsilonRateInt = gammaRate.Select(b => !b).ToInt();
        var res = gammaRateInt * epsilonRateInt;
        Console.WriteLine(res); // p1

        var nums = lines.DeepCopy();
        var bitIndex = 0;
        while (nums.Length > 1)
        {
            var ones = nums.Count(s => s[bitIndex] == '1');
            var zeros = nums.Length - ones;
            var chToFilter = ones >= zeros ? '1' : '0';
            nums = nums.Where(s => s[bitIndex] == chToFilter).Distinct().ToArray();
            bitIndex++;
        }

        var oxyRating = nums[0].Select(s => s == '1').ToInt();
        nums = lines.DeepCopy();
        bitIndex = 0;
        while (nums.Length > 1)
        {
            var ones = nums.Count(s => s[bitIndex] == '1');
            var zeros = nums.Length - ones;
            var chToFilter = zeros <= ones ? '0' : '1';
            nums = nums.Where(s => s[bitIndex] == chToFilter).Distinct().ToArray();
            bitIndex++;
        }

        var co2Rating = nums[0].Select(s => s == '1').ToInt();
        res = oxyRating * co2Rating;
        Console.WriteLine(res);
    }
}