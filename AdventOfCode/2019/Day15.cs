using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2019;

public class Day15 : Solution
{
    public Day15() : base(15, 2019) { }
    protected override void Solve()
    {
        var amp = new Amplifier(ReadText());
        // var dirs = new[] {"N", "S", "W", "E"};
        (int r, int c) start = (500, 500);
        var grid = new int[1000, 1000];
        grid.At(start) = 1; // 2 - wall, 1 - empty
        (int r, int c) oxy = (512, 480);
        Explore(grid, start, amp);

        var frontier = new Queue<((int r, int c) p, int length)>();
        frontier.Enqueue((oxy, 0));
        var oxygenated = new HashSet<(int r, int c)>();
        // oxygenated.Add(oxy);
        var maxLength = 0;
        while (frontier.Count > 0)
        {
            var item = frontier.Dequeue();
            oxygenated.Add(item.p);
            maxLength = Max(maxLength, item.length);
            item.p.Gen4Adjacent()
                .Where(p => grid.At(p) == 1)
                .Where(p => !oxygenated.Contains(p))
                .Select(p => (p, item.length + 1))
                .ForEach(frontier.Enqueue);
        }

        Console.WriteLine(maxLength);
    }

    private static readonly Dictionary<(int r, int c), int> Map2 = new()
    {
        {(-1, 0), 1},{(1, 0), 2}, {(0, -1), 3},{(0, 1), 4}
    };

    static void Explore(int[,] grid, (int r, int c) start, Amplifier amp)
    {
        var frontier = new Stack<((int r, int c), ImmutableList<(int r, int c)>)>();
        frontier.Push((start, ImmutableList<(int r, int c)>.Empty));
        var prevStr = ImmutableList<(int r, int c)>.Empty;
        while (frontier.Count > 0)
        {
            var (point, str) = frontier.Pop();
            // go to point
            // walk back the prevStr.Length - (str.Length - 1)
            for (var i = 0; i < str.Count - 1; i++)
            {
                if (str[i] != prevStr[i])
                {
                    throw new InvalidOperationException("previous path is too weird");
                }
            }
            for (var i = prevStr.Count - 1; i >= str.Count - 1 && i >= 0; i--)
            {
                var step = Map2[prevStr[i].Opposite()];
                amp.Input.Enqueue(step);
                amp.Run();
                var outp = amp.Output.Dequeue();
                if (outp == 0)
                {
                    throw new InvalidOperationException("wall on the way back");
                }
            }
            // the last step is the last char in str
            if (str.Count > 0)
            {
                var lastStep = Map2[str.Last()];
                amp.Input.Enqueue(lastStep);
                amp.Run();
                var output = amp.Output.Dequeue();
                if (output == 0)
                {
                    grid.At(point) = 2;
                    prevStr = str.RemoveAt(str.Count - 1);
                    continue;
                }
                else
                {
                    grid.At(point) = 1;
                    prevStr = str;
                }
            }
                
            // visited.Add(point, str);
            point.Gen4Adjacent()
                .Where(p => grid.At(p) == 0)
                .Select(p => (p, str.Add(p.Minus(point))))
                .ForEach(frontier.Push);
        }
    }
}