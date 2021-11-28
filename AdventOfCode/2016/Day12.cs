namespace AdventOfCode._2016;

public class Day12 : Solution
{
    public Day12() : base(12, 2016) { }
    protected override void Solve()
    {
        var instructions = ReadLines();
        var reg = new Dictionary<string, int>
        {
            ["a"] = 0,
            ["b"] = 0,
            ["c"] = 1,
            ["d"] = 0,
        };
        var line = 0;
        while (true)
        {
            if (line >= instructions.Length)
                break;
            var arg = instructions[line].Split();

            int getValue(string registerOrValue)
            {
                if (!int.TryParse(registerOrValue, out var val))
                    val = reg[registerOrValue];
                return val;
            }
            switch (arg[0])
            {
                case "cpy":
                    reg[arg[2]] = getValue(arg[1]);
                    break;

                case "inc":
                    reg[arg[1]]++;
                    break;

                case "dec":
                    reg[arg[1]]--;
                    break;

                case "jnz":
                    if (getValue(arg[1]) != 0)
                        line += getValue(arg[2]) - 1;
                    break;
            }

            line++;
        }

        Console.WriteLine(reg["a"]);
    }
}