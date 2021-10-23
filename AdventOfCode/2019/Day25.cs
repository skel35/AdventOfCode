namespace AdventOfCode._2019;

public class Day25 : Solution
{
    public Day25() : base(25, 2019) { }
    protected override void Solve()
    {
        var amp = new Amplifier(ReadText());
        var halted = false;
        while (!halted)
        {
            halted = amp.Run();
            while (amp.Output.Count > 0)
            {
                Console.Write(amp.GetAsciiOutputIfInRange());
            }

            amp.Input(Console.ReadLine()!);
        }
    }
}