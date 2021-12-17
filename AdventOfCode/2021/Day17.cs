namespace AdventOfCode._2021;

public class Day17 : Solution
{
    public Day17() : base(17, 2021) { }
    protected override void Solve()
    {
        var (x1, x2, y1, y2) = (102, 157, -146, -90);
        var start = (X: 0, Y: 0);
        var velocity = (X: 15, Y: 145);
        var reached = false;
        var p = start;
        var v = velocity;
        var maxY = 0;
        for (var i = 0; i < 999999; i++)
        {
            p = p.Plus(v);
            if (v.Y == 0)
                maxY = p.Y;
            v = (v.X > 0 ? v.X - 1 : v.X < 0 ? v.X + 1 : 0, v.Y - 1);
            if (p.X >= x1 && p.X <= x2 && p.Y >= y1 && p.Y <= y2)
            {
                reached = true;
                Console.WriteLine("reached in " + p);
                break;
            }

            if (p.Y < y1 && v.Y < 0)
            {
                Console.WriteLine("won't reach, in " + p);
                break;
            }
        }

        maxY.ToString().Print();
    }
}