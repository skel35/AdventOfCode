namespace AdventOfCode._2020;

public class Day8 : Solution
{
    public Day8() : base(8, 2020) { }
    protected override void Solve()
    {
        var lines = ReadLines().Select(line => line.Split())
            .Select(array => (Op: array[0], Arg: array[1].ToInt()))
            .ToArray();
        for (var i = 0; i < lines.Length; i++)
        {
            var copy = lines.DeepCopy();
            copy[i].Op = lines[i].Op switch
            {
                "jmp" => "nop",
                "nop" => "jmp",
                {} s => s
            };

            var set = new HashSet<int>();
            var acc = 0;
            var it = 0;
            do
            {
                if (it == copy.Length)
                {
                    Console.WriteLine(acc);
                    return;
                }

                if (!set.Add(it))
                {
                    break;
                }

                (acc, it) = copy[it] switch
                {
                    ("nop", _) => (acc, it + 1),
                    ("acc", {} n) => (acc + n, it + 1),
                    ("jmp", {} n) => (acc, it + n),
                    _ => (acc, it)
                };
            } while (true);
        }
    }
}