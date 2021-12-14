using System.Numerics;
using System.Text;
using RegExtract;

namespace AdventOfCode._2021;

public class Day14 : Solution
{
    public Day14() : base(14, 2021) { }
    protected override void Solve()
    {
        var lines = ReadLines();
        var start = lines[0];
        var transforms = lines[2..]
            .Select(s => s.Extract<(string Left, char Right)>(@"(.+) -> (.)"))
            .ToDictionary(t => t.Left, t => t.Right);

        // p1:
        // var res = new StringBuilder(start);
        // for (var i = 0; i < 40; i++)
        // {
        //     for (var j = res.Length - 2; j >= 0; j--)
        //     {
        //         if (transforms.TryGetValue(res.ToString(j, 2), out var right))
        //             res.Insert(j + 1, right);
        //     }
        // }
        //
        // var groupedBy = res.ToString().AsEnumerable().GroupBy(c => c).ToArray();
        // var maxCount = groupedBy.Max(g => g.Count());
        // var minCount = groupedBy.Min(g => g.Count());
        // (maxCount - minCount).ToString().Print();

        var counts = transforms.ToDictionary(kvp => kvp.Key, _ => new BigInteger());
        start.Pairwise().Select(x => x.first.ToString() + x.second).ForEach(s => counts[s]++);
        var beginningChar = start[0];
        var endingChar = start.Last();
        for (var i = 0; i < 40; i++)
        {
            var newCounts = counts.ToDictionary(kvp => kvp.Key, _ => new BigInteger());
            counts.ForEach(
                kvp =>
                {
                    var (key, value) = kvp;
                    var ch = transforms[key];
                    var s1 = key[0].ToString() + ch;
                    var s2 = ch.ToString() + key[1];
                    newCounts[s1] += value;
                    newCounts[s2] += value;
                });
            counts = newCounts;
        }

        var countsPerChar = new Dictionary<char, BigInteger>();
        counts.ForEach(kvp => countsPerChar.AddOrUpdate(kvp.Key[0], kvp.Value, (_, val) => val + kvp.Value));
        countsPerChar[beginningChar]++;
        countsPerChar[endingChar]++;
        var maxValue = countsPerChar.Values.Max();
        var minValue = countsPerChar.Values.Min();
        (maxValue - minValue).ToString().Print();
    }
}