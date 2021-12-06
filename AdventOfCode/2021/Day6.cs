using System.Numerics;

namespace AdventOfCode._2021;

public class Day6 : Solution
{
    public Day6() : base(6, 2021) { }
    protected override void Solve()
    {
        var dict = AoC.ParseInts(ReadText()).GroupBy(x => x).ToDictionary(g => g.Key, g => new BigInteger(g.Count()));
        dict.TryAdd(0, 0);
        dict.TryAdd(6, 0);
        dict.TryAdd(7, 0);
        dict.TryAdd(8, 0);

        for (var day = 0; day < 256; day++)
        {
            var newDict = new Dictionary<int, BigInteger>();
            newDict[8] = dict[0];
            newDict[7] = dict[8];
            newDict[6] = dict[7] + dict[0];
            for (var i = 5; i >= 0; i--)
                newDict[i] = dict[i + 1];
            dict = newDict;
        }

        var res = dict.Values.Aggregate(BigInteger.Add);
        Console.WriteLine(res);
    }
}