namespace AdventOfCode._2021;

public class Day15 : Solution
{
    public Day15() : base(15, 2021) { }
    protected override void Solve()
    {
        var grid = ReadLines().Select(row => row.Select(ch => ch.ToInt()).ToArray()).ToArray();

        var grid2 = new int[grid.Length * 5][];
        for (var i = 0; i < grid2.Length; i++) grid2[i] = new int[grid[0].Length * 5];
        for (var i = 0; i < grid2.Length; i++)
        {
            for (var j = 0; j < grid2[0].Length; j++)
            {
                var value = (grid.At((i % grid.Length, j % grid[0].Length)) + i / grid.Length + j / grid[0].Length);
                grid2.At((i, j)) = (value + 8) % 9 + 1;
            }
        }
        // p2:
        grid = grid2;

        var cost = new int[grid.Length, grid[0].Length];
        var frontier = new PriorityQueue<(int R, int C), int>();
        var start = (0, 0);
        var end = (R: grid.Length - 1, C: grid[0].Length - 1);

        int lowerBoundCost((int R, int C) from) =>
            // end.R - from.R + end.C - from.C; // A*
            0; // Dijkstra

        bool updateCost((int R, int C) point, int costBeforePoint)
        {
            var newCost = costBeforePoint + grid.At(point);
            if (cost.At(point) == 0 || newCost < cost.At(point))
            {
                cost.At(point) = newCost;
                return true;
            }

            return false;
        }

        frontier.Enqueue(start, cost.At(start) + lowerBoundCost(start));
        cost.At(end) = int.MaxValue;
        var iter = 0;
        while (frontier.Count > 0)
        {
            iter++;
            frontier.TryDequeue(out var point, out _);

            var costUntilNow = cost.At(point);
            point.Gen4Adjacent()
                .Where(p => p.InBounds(grid))
                .Where(p => updateCost(p, costUntilNow))
                .Select(p => (p, cost.At(p) + lowerBoundCost(p)))
                .Where(x => x.Item2 < cost.At(end))
                .ForEach(x => frontier.Enqueue(x.Item1, x.Item2));
        }

        Console.WriteLine("iterations: " + iter);
        cost.At(end).ToString().Print();
    }
}