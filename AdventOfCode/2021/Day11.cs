namespace AdventOfCode._2021;

public class Day11 : Solution
{
    public Day11() : base(11, 2021) { }
    protected override void Solve()
    {
        var grid = ReadLines().ToSb().Map((r, c, ch) => ch.ToInt());
        var totalFlashes = 0;
        for (var i = 0; i < 100000; i++)
        {
            var copy = (int[,]) grid.Clone();
            copy.Iter((r, c, ch) => copy[r, c] = ch + 1);
            var stepFlashes = 0;
            var flashes = 0;
            do
            {
                stepFlashes += flashes;
                flashes = 0;
                var copy2 = (int[,]) copy.Clone();
                copy.Iter((r, c, n) =>
                {
                    if (n >= 10)
                    {
                        copy2[r, c] = 0;
                        (r, c).Gen8Adjacent()
                            .Where(p => p.InBounds(grid))
                            .Where(p => copy2.At(p) != 0)
                            .ForEach(p => copy2.At(p) = copy2.At(p) + 1);
                        flashes++;
                    }
                });
                copy = copy2;
            } while (flashes > 0);

            if (stepFlashes == 100)
            {
                Console.WriteLine(i + 1);
                return;
            }
            totalFlashes += stepFlashes;
            grid = copy;
        }

        // p1:
        // Console.WriteLine(totalFlashes);
    }
}