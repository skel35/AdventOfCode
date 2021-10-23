namespace AdventOfCode._2020;

public class Day14 : Solution
{
    public Day14() : base(14, 2020) { }
    protected override void Solve()
    {
        var input =
            ReadLines();
        var dict = new Dictionary<ulong, ulong>();
        string mask = "0X10110X1001000X10X00X01000X01X01101";
        ulong mask0 = 0, mask1 = 0;
        List<int> xpos = new List<int>();
        for (var i = 0; i < input.Length; i++)
        {
            if (input[i].StartsWith("mask"))
            {
                mask = input[i][7..].Reverse().ToStr();
                mask0 = 0;
                mask1 = 0;
                for (var j = 0; j < mask.Length; j++)
                {
                    if (mask[j] == '0')
                        mask0 = mask0 ^ (1ul << j);
                    else if (mask[j] == '1')
                        mask1 = mask1 ^ (1ul << j);
                }

                xpos = mask.WithIndex().Where(t => t.Item == 'X')
                    .Select(t => t.Index).ToList();
                mask0 = ~mask0;

            }
            else
            {
                var s = input[i].SplitBy(' ', '=', '[', ']');
                var index = s[1].ToUlong();
                var val = s[2].ToUlong();
                if (!dict.ContainsKey(index))
                {
                    dict[index] = 0;
                }

                // p1:
                // var valPost = val & mask0;
                // valPost = valPost | mask1;
                // dict[index] = val;

                // p2:
                IEnumerable<ulong> GetPos(ulong index, ulong mask0, ulong mask1, List<int> pos)
                {
                    if (pos.Count == 0)
                    {
                        index = index & (~mask0);
                        index = index | mask1;
                        return new[] {index};
                    }

                    var p = pos[0];
                    var pos1 = GetPos(index, mask0 ^ (1ul << p), mask1, pos.Skip(1).ToList());
                    var pos2 = GetPos(index, mask0, mask1 ^ (1ul << p), pos.Skip(1).ToList());
                    return pos1.Concat(pos2);
                }

                var positions = GetPos(index, 0ul, mask1, xpos);
                positions.ForEach(pos => dict[pos] = val);
            }
        }

        var sum = dict.Select(kvp => kvp.Value).Sum();
        Console.WriteLine(sum);
    }
}