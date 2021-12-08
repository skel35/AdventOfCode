namespace AdventOfCode._2021;

public class Day8 : Solution
{
    public Day8() : base(8, 2021) { }
    protected override void Solve()
    {
        var input = ReadLines()
            .Select(s => s.Split('|'))
            .Select(s => (Signals: s[0].Split2(), Output: s[1].Split2()))
            .ToArray();
        // var signals = input.Select(x => x.Signals).ToArray();
        // var output = input.Select(x => x.Output).ToArray();

        // var totalCount = output.SelectMany(x => x).Count(x => x.Length is 2 or 3 or 4 or 7);
        // Console.WriteLine(totalCount);

        long outputSum = 0;

        foreach (var (signal, output) in input)
        {
            var seven = signal.First(x => x.Length == 3);
            var one = signal.First(x => x.Length == 2);
            var four = signal.First(x => x.Length == 4);
            var eight = signal.First(x => x.Length == 7);
            var map = new Dictionary<char, int>();
            var char0 = seven.Except(one).Single();
            map[char0] = 0;
            var count2Or5 = signal.Count(x => x.Contains(one[0]));
            var char2 = count2Or5 == 8 ? one[0] : one[1];
            var char5 = char2 == one[0] ? one[1] : one[0];
            map[char2] = 2;
            map[char5] = 5;
            var char1 = four.Except(one).Single(ch => signal.Count(x => x.Contains(ch)) == 6);
            var char3 = four.Except(one.Concat(char1)).Single();
            map[char1] = 1;
            map[char3] = 3;
            var char4Or6 = eight.Except(four).Except(seven).ToArray();
            var char4 = char4Or6.Single(ch => signal.Count(x => x.Contains(ch)) == 4);
            var char6 = char4Or6.Except(char4).Single();
            map[char4] = 4;
            map[char6] = 6;

            var someRes = output
                .Select(encodedNumber =>
                {
                    var intermediate = encodedNumber.Select(ch => map[ch]).ToArray();
                    return create(intermediate);
                })
                .Select(ba => Patterns[toInt(ba)])
                .ToArray();

            var outputNumber =
                someRes
                .Aggregate(0, (cur, next) => cur * 10 + next);

            outputSum += outputNumber;
        }

        Console.WriteLine(outputSum);
    }

    private static readonly Dictionary<int, int> Patterns = new()
    {
        [createBitArray(1, 1, 1, 0, 1, 1, 1)] = 0,
        [createBitArray(0, 0, 1, 0, 0, 1, 0)] = 1,
        [createBitArray(1, 0, 1, 1, 1, 0, 1)] = 2,
        [createBitArray(1, 0, 1, 1, 0, 1, 1)] = 3,
        [createBitArray(0, 1, 1, 1, 0, 1, 0)] = 4,
        [createBitArray(1, 1, 0, 1, 0, 1, 1)] = 5,
        [createBitArray(1, 1, 0, 1, 1, 1, 1)] = 6,
        [createBitArray(1, 0, 1, 0, 0, 1, 0)] = 7,
        [createBitArray(1, 1, 1, 1, 1, 1, 1)] = 8,
        [createBitArray(1, 1, 1, 1, 0, 1, 1)] = 9,
    };

    private static int createBitArray(params int[] bits) => toInt(new BitArray(bits.Select(x => x > 0).ToArray()));

    private static BitArray create(int[] positions)
    {
        var bits = new bool[7];
        positions.ForEach(p => bits[p] = true);
        return new BitArray(bits);
    }

    private static int toInt(BitArray bitArray)
    {

        if (bitArray.Length > 32)
            throw new ArgumentException("Argument length shall be at most 32 bits.");

        int[] array = new int[1];
        bitArray.CopyTo(array, 0);
        return array[0];

    }
}