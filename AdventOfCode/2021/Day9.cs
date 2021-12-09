namespace AdventOfCode._2021;

public class Day9 : Solution
{
    public Day9() : base(9, 2021) { }
    protected override void Solve()
    {
        var grid = ReadLines();
        // p1:
        var topBasins = new[] { 0, 0, 0 };
        void addBasin(int basinSize)
        {
            if (basinSize < topBasins[2]) return;
            topBasins[2] = basinSize;
            if (topBasins[1] < topBasins[2])
            {
                (topBasins[1], topBasins[2]) = (topBasins[2], topBasins[1]);
                if (topBasins[0] < topBasins[1])
                {
                    (topBasins[1], topBasins[0]) = (topBasins[0], topBasins[1]);
                }
            }
        }
        var sum = 0;
        grid.Iter((r, c, ch) =>
        {
            var adjacent = (r, c).Gen4Adjacent().Where(p => p.InBounds(grid)).ToArray();
            var isLowPoint = adjacent.All(p => ch < grid.At(p));
            if (isLowPoint)
            {
                sum += ch.ToInt() + 1;
                var basinSize = 1;
                var visited = new HashSet<(int R, int C)> { (r, c) };
                var frontier = new PriorityQueue<(int R, int C), int>(adjacent.Select(p => (p, grid.At(p).ToInt())).Where(x => x.Item2 < 9));
                while (frontier.Count > 0)
                {
                    var point = frontier.Dequeue();
                    if (visited.Contains(point)) continue;
                    visited.Add(point);
                    basinSize++;
                    var adj = point.Gen4Adjacent().Where(p => p.InBounds(grid) && !visited.Contains(p) && grid.At(p) != '9').ToArray();
                    adj.ForEach(p => frontier.Enqueue(p, grid.At(p).ToInt()));
                }
                addBasin(basinSize);
            }
        });
        // p1:
        // Console.WriteLine(sum);
        // p2:
        Console.WriteLine(topBasins[0] * topBasins[1] * topBasins[2]);

    }
}