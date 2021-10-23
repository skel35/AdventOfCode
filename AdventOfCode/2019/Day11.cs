namespace AdventOfCode._2019;

public class Day11 : Solution
{
    public Day11() : base(11, 2019) { }
    protected override void Solve()
    {
        var N = 200;
        var grid = new bool[N + 1 + N, N + 1 + N];
        var dir = (R:  -1, c: 0);
        var pos = (R:  N, c: N);
        grid.At(pos) = true;
        var halted = false;
        var amp = new Amplifier(ReadText());
        // var painted = new HashSet<(int r, int c)>();
        while (!halted)
        {
            amp.Input.Enqueue(grid.At(pos) ? 1 : 0);
            halted = amp.Run();
            grid.At(pos) = amp.Output.Dequeue() == 1;
            // painted.Add(pos);
            dir = amp.Output.Dequeue() == 1 ? dir.Right() : dir.Left();
            pos = pos.Plus(dir);
        }

        // Console.WriteLine(painted.Count);

        grid.Iter((_, j, b) =>
        {
            if (b) Console.Write('#');
            else Console.Write('.');
            if (j == grid.R() - 1) Console.WriteLine();
        });
    }
}