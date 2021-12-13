using RegExtract;

namespace AdventOfCode._2021;

public class Day13 : Solution
{
    public Day13() : base(13, 2021) { }
    protected override void Solve()
    {
        var input = ReadText().Split("\n\n");
        var dots = input[0].SplitBy('\n').Select(s => s.SplitBy(',').Select(int.Parse).ToArray()).Select(d => (X: d[0], Y: d[1])).ToArray();
        var fold = input[1].SplitBy('\n').Select(s => s.Extract<(char C, int N)>(@"fold along (.)=(\d+)")).ToArray();
        foreach (var f in fold)
        {
            if (f.C == 'x')
                dots = dots.Select(d => (d.X <= f.N ? d.X : f.N - (d.X - f.N), d.Y)).ToArray();
            else
                dots = dots.Select(d => (d.X, d.Y <= f.N ? d.Y : f.N - (d.Y - f.N))).ToArray();
        }

        var grid = new char[40, 140];
        foreach (var d in dots) grid.At((d.Y, d.X)) = '#';
        grid.Print();
        // dots2.Length.ToString().Print();
        // dots2.ToHashSet().Count.ToString().Print();
    }
}