using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2019;

public class Day18 : Solution
{
    public Day18() : base(18, 2019) { }
    protected override void Solve()
    {
        var grid = ReadLines();
        var gridSb = grid.ToSb();
        var mid = (grid.R() / 2, grid.C() / 2);
        (grid.At(mid) == '@').Assert();
        (0,0).Gen4Adjacent().Append((0, 0)).Select(dir => mid.Plus(dir))
            .ForEach(p => gridSb.SetAt(p,'#'));
        grid = gridSb.ToStringArray();
        // var initialState = (0u, 0, 0, mid);
        var startingPoints = new[] {mid.Plus((-1, -1)), mid.Plus((-1, 1)), mid.Plus((1, -1)), mid.Plus((1, 1))};
        var initialState2 = (0u, 0, 0,
            new V4(startingPoints[0], startingPoints[1], startingPoints[2], startingPoints[3]));
        var finalLength = int.MaxValue;
        // var statesDict = new Dictionary<((int r, int c) point, uint keys), int>();
        var statesDict2 = new Dictionary<(V4 points, uint keys), int>();
        // var frontier = new PriorityQueue<(uint keys, int length, int countKeys, (int r, int c) point)>();
        var frontier2 = new PriorityQueue<(uint keys, int length, int countKeys, V4 points)>();
        // frontier.Enqueue(initialState, 0); // priority = (double)state.length / state.countKeys
        frontier2.Enqueue(initialState2, 0);
        // statesDict.Add((mid, 0u), 0);
        statesDict2.Add((initialState2.Item4, 0u), 0);
        while (frontier2.Count > 0)
        {
            var state = frontier2.Dequeue();
            var reachableKeys = GetReachableKeys2(state.keys, state.length, grid, state.points);
            if (reachableKeys.Count == 0)
            {
                finalLength = Min(finalLength, state.length);
                continue;
            }

            var fl = finalLength;
            reachableKeys
                .Where(kvp => kvp.Value.length < fl)
                .Select(kvp =>
                {
                    var keys = state.keys.Set(grid.At(kvp.Value.keyPoint) - 'a', true);
                    var length = kvp.Value.length;
                    var countKeys = state.countKeys + 1;
                    return (keys, length, countKeys, points: kvp.Key);
                })
                .ForEach(state =>
                {
                    if (statesDict2.ContainsKey((state.points, state.keys))
                        && statesDict2[(state.points, state.keys)] <= state.length)
                    {
                        return;
                    }
                    frontier2.Enqueue(state, (double) state.length / state.countKeys);
                    statesDict2[(state.points, state.keys)] = state.length;
                });
        }

        Console.WriteLine(finalLength);
    }

    private static bool CanVisit((int r, int c) point, uint foundKeys, string[] grid)
    {
        if (point.OutOfBounds(grid.R(), grid.C())) return false;
        var val = grid.At(point);
        if (val == '.' || val == '@') return true;
        return char.IsLower(val)
               || (char.IsUpper(val)
                   && foundKeys.Get(val - 'A'));
    }
        
    private static Dictionary<V4, ((int r, int c) keyPoint, int length)> GetReachableKeys2(
        uint foundKeys, int currentLength, string[] grid, V4 currentPoints)
    {
        var reachableKeys = new Dictionary<V4, ((int r, int c) keyPoint, int length)>();
        var perPoint = currentPoints.ToPointsArray()
            .Select(p => GetReachableKeys(foundKeys, currentLength, grid, p))
            .ToArray();
        perPoint[0].ForEach(kvp =>
        {
            var v4 = new V4(kvp.Key, currentPoints.P2, currentPoints.P3, currentPoints.P4);
            reachableKeys.Add(v4, (kvp.Key, kvp.Value));
        });
        perPoint[1].ForEach(kvp =>
        {
            var v4 = new V4(currentPoints.P1, kvp.Key, currentPoints.P3, currentPoints.P4);
            reachableKeys.Add(v4, (kvp.Key, kvp.Value));
        });
        perPoint[2].ForEach(kvp =>
        {
            var v4 = new V4(currentPoints.P1, currentPoints.P2, kvp.Key, currentPoints.P4);
            reachableKeys.Add(v4, (kvp.Key, kvp.Value));
        });
        perPoint[3].ForEach(kvp =>
        {
            var v4 = new V4(currentPoints.P1, currentPoints.P2, currentPoints.P3, kvp.Key);
            reachableKeys.Add(v4, (kvp.Key, kvp.Value));
        });

        return reachableKeys;
    }

    private static Dictionary<(int r, int c), int> GetReachableKeys(
        uint foundKeys, int currentLength, string[] grid, (int r, int c) currentPoint)
    {
        var visited = new HashSet<(int r, int c)>();
        var reachableKeys = new Dictionary<(int r, int c), int>();
        var queue = new Queue<((int r, int c), int length)>();
        queue.Enqueue((currentPoint, currentLength));
        while (queue.Count > 0)
        {
            var (item, length) = queue.Dequeue();
            if (visited.Contains(item)) continue;
            // Console.WriteLine(item);
            visited.Add(item);
            var ch = grid.At(item);
            // Console.WriteLine(ch);
            if (char.IsLower(ch) && !foundKeys.Get(ch - 'a'))
            {
                reachableKeys.Add(item, length);
                continue;
            }
            item.Gen4Adjacent()
                .Where(p => CanVisit(p, foundKeys, grid))
                .ForEach(p => queue.Enqueue((p, length + 1)));
        }

        return reachableKeys;
    }
}