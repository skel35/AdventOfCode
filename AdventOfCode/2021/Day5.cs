using RegExtract;
using static System.Math;

namespace AdventOfCode._2021;

public class Day5 : Solution
{
    record Line(int X1, int Y1, int X2, int Y2);
    public Day5() : base(5, 2021) { }
    protected override void Solve()
    {
        var lines = ReadLines().ArrayMap(s => s.Extract<Line>(@"(\d+),(\d+) -> (\d+),(\d+)")!);
        // lines = lines.Where(l => l.X1 == l.X2 || l.Y1 == l.Y2).ToArray(); // p1
        // solution 1:
        var overlaps = new HashSet<(int X, int Y)>();
        for (var i = 0; i < lines.Length; i++)
        {
            var line1 = lines[i];
            for (var j = i + 1; j < lines.Length; j++)
            {
                var line2 = lines[j];

                var (x1, y1, x2, y2) = line1;
                var (x3, y3, x4, y4) = line2;

                if (x2 == x1)
                {
                    if (x4 == x3)
                    {
                        if (x4 != x1) continue;

                        var miny1 = Min(y1, y2);
                        var miny2 = Min(y3, y4);
                        var maxy1 = Max(y1, y2);
                        var maxy2 = Max(y3, y4);
                        var fromY = Max(miny1, miny2);
                        var toY = Min(maxy1, maxy2);
                        for (var y = fromY; y <= toY; y++)
                            overlaps.Add((x1, y));
                    }
                    else if (x1.IsBetweenNonStrict(x3, x4))
                    {
                        var a = (y4 - y3) / (x4 - x3);
                        var b = y3 - a * x3;
                        var yOverlap = a * x1 + b;
                        if (yOverlap.IsBetweenNonStrict(y1, y2)
                            && yOverlap.IsBetweenNonStrict(y3, y4))
                            overlaps.Add((x1, yOverlap));
                    }
                }
                else if (y2 == y1)
                {
                    if (y4 == y3)
                    {
                        if (y4 != y1) continue;

                        var minx1 = Min(x1, x2);
                        var minx2 = Min(x3, x4);
                        var maxx1 = Max(x1, x2);
                        var maxx2 = Max(x3, x4);
                        var fromX = Max(minx1, minx2);
                        var toX = Min(maxx1, maxx2);
                        for (var x = fromX; x <= toX; x++)
                            overlaps.Add((x, y1));
                    }
                    else if (x4 == x3)
                    {
                        if (x3.IsBetweenNonStrict(x1, x2)
                            && y1.IsBetweenNonStrict(y3, y4))
                            overlaps.Add((x3, y1));
                    }
                    else
                    {
                        var a = (y4 - y3) / (x4 - x3);
                        var b = y3 - a * x3;
                        var xOverlap = (y1 - b) / a;
                        if (xOverlap.IsBetweenNonStrict(x1, x2)
                            && xOverlap.IsBetweenNonStrict(x3, x4))
                            overlaps.Add((xOverlap, y1));
                    }
                }
                else
                {
                    var a = (y2 - y1) / (x2 - x1);
                    (a is 1 or -1).Assert();

                    var b = y1 - a * x1;
                    (b == y2 - a * x2).Assert();

                    if (x4 == x3)
                    {
                        if (x3.IsBetweenNonStrict(x1, x2) == false)
                            continue;
                        var yOverlap = a * x3 + b;
                        if (yOverlap.IsBetweenNonStrict(y3, y4)
                            && yOverlap.IsBetweenNonStrict(y1, y2))
                            overlaps.Add((x3, yOverlap));
                    }
                    else if (y4 == y3)
                    {
                        if (y3.IsBetweenNonStrict(y1, y2) == false)
                            continue;
                        // y3 = a * x + b
                        // x = (y3 - b) / a
                        var xOverlap = (y3 - b) / a;
                        if (xOverlap.IsBetweenNonStrict(x1, x2)
                            && xOverlap.IsBetweenNonStrict(x3, x4))
                            overlaps.Add((xOverlap, y3));
                    }
                    else
                    {
                        var c = (y4 - y3) / (x4 - x3);
                        var d = y3 - c * x3;
                        if (a == c)
                        {
                            if (b != d) continue;
                            var fromX = Max(Min(x1, x2), Min(x3, x4));
                            var toX = Min(Max(x1, x2), Max(x3, x4));
                            for (var x = fromX; x <= toX; x++)
                            {
                                var y = a * x + b;
                                overlaps.Add((x, y));
                            }
                        }
                        else
                        {
                            // y = a*x + b
                            // y = c*x + d
                            var mod = (d - b) % (a - c);
                            if (mod != 0)
                                continue;
                            var xOverlap = (d - b) / (a - c);
                            if (xOverlap.IsBetweenNonStrict(x1, x2)
                                && xOverlap.IsBetweenNonStrict(x3, x4))
                                overlaps.Add((xOverlap, a * xOverlap + b));
                        }
                    }
                }
            }
        }

        Console.WriteLine(overlaps.Count);

        // solution 2:
        var grid = new int[1000, 1000];
        foreach (var (x1, y1, x2, y2) in lines)
        {
            var xD = Sign(x2 - x1);
            var yD = Sign(y2 - y1);
            var n = Max(Abs(x2 - x1), Abs(y2 - y1));
            var x = x1;
            var y = y1;
            for (var i = 0; i <= n; i++)
            {
                grid[x, y]++;
                x += xD;
                y += yD;
            }
        }
        var count = grid.Cast<int>().Count(x => x > 1);
        Console.WriteLine(count);

        // code for testing solution 1 vs solution 2 diff:
        // var cells = getCoordinates(grid);
        // var countedButIncorrect = overlaps.Except(cells).ToHashSet();
    }

    HashSet<(int X, int Y)> getCoordinates(int[,] grid)
    {
        var res = new HashSet<(int X, int Y)>();
        for (var i = 0; i < grid.R(); i++)
            for (var j = 0; j < grid.C(); j++)
                if (grid[i, j] > 1)
                    res.Add((i, j));
        return res;
    }
}