using static System.Math;

namespace AdventOfCode._2021;

public class Day17 : Solution
{
    public Day17() : base(17, 2021) { }
    protected override void Solve()
    {
        var (x1, x2, y1, y2) = (102, 157, -146, -90);
        // var (x1, x2, y1, y2) = (20, 30, -10, -5);

        (int X, int Y) getPos((int X, int Y) v, int n)
        {
            var x = (v.X + Max(v.X - n + 1, 1)) * Min(n, v.X) / 2;
            var y = (v.Y + (v.Y - n + 1)) * n / 2;
            return (x, y);
        }

        var count = 0;
        for (var x = 14; x <= 157; x++)
        // for (var x = 6; x <= 30; x++)
        {
            for (var y = -146; y <= 145; y++)
            // for (var y = -10; y <= 45; y++)
            {
                for (var n = 1; n <= 292; n++)
                // for (var n = 0; n <= 90; n++)
                {
                    var p = getPos((x, y), n);
                    if (p.X >= x1 && p.X <= x2 && p.Y >= y1 && p.Y <= y2)
                    {
                        count++;
                        // (x, y).ToString().Print();
                        break;
                    }
                }
            }
        }

        count.ToString().Print();
    }
}