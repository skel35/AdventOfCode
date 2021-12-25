namespace AdventOfCode._2021;

public class Day25 : Solution
{
    public Day25() : base(25, 2021) { }
    protected override void Solve()
    {
        var grid = ReadLines().ArrayMap(l => l.ToCharArray());
        var steps = 0;
        while (true)
        {
            var anyMovement = false;
            var grid2 = grid.DeepCopy();
            grid.Iter(
                (i, j, ch) =>
                {
                    if (ch == '>')
                    {
                        var nextC = (j + 1) % grid[0].Length;
                        if (grid[i][nextC] == '.')
                        {
                            grid2[i][j] = '.';
                            grid2[i][nextC] = '>';
                            anyMovement = true;
                        }
                    }
                });
            grid = grid2;
            grid2 = grid.DeepCopy();
            grid.Iter(
                (i, j, ch) =>
                {
                    if (ch == 'v')
                    {
                        var nextR = (i + 1) % grid.Length;
                        if (grid[nextR][j] == '.')
                        {
                            grid2[i][j] = '.';
                            grid2[nextR][j] = 'v';
                            anyMovement = true;
                        }
                    }
                });
            grid = grid2;
            steps++;
            if (!anyMovement)
            {
                break;
            }
        }

        steps.ToString().Print();
    }
}