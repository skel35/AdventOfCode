using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2019
{
    public class Day20 : Solution
    {
        public Day20() : base(20, 2019) { }
        protected override void Solve()
        {
            var lines = ReadLines();
            if (lines[^1].Length != lines[0].Length)
            {
                lines[^1] = lines[^1] + Enumerable.Repeat(' ', lines[0].Length - lines[^1].Length).ToStr();
            }
            var R = lines.Length - 4;
            var C = lines[0].Length - 4;
            const int CC = 27;
            const int RC = 57;
            char At((int r, int c) p) => lines.At(p.Plus((2, 2)));
            bool IsEdge((int r, int c) p)
            {
                if (At(p) != '.') return false;
                var (r, c) = p;
                return r == 0 || c == 0 || r == R - 1 || c == C - 1
                    || (r == ((R - RC) / 2 - 1) && (c.IsBetween(CC - 1, C - CC)))
                    || (r == ((R + RC) / 2) && (c.IsBetween(CC - 1, C - CC)))
                    || (c == CC - 1 && r.IsBetween((R - RC) / 2 - 1, (R + RC) / 2))
                    || (c == C - CC && r.IsBetween((R - RC) / 2 - 1, (R + RC) / 2));
            }

            bool IsOuter((int r, int c) p)
            {
                var (r, c) = p;
                return r == 0 || c == 0 || r == R - 1 || c == C - 1;
            }

            string GetLabel((int r, int c) edge)
            {
                var (r, c) = edge;
                if (r == 0) return At((r - 2, c)).ToString() + At((r - 1, c));
                if (r == R - 1) return At((r + 1, c)).ToString() + At((r + 2, c));
                if (c == 0) return At((r, c - 2)).ToString() + At((r, c - 1));
                if (c == C - 1) return At((r, c + 1)).ToString() + At((r, c + 2));
                if (r == (R - RC)/2 - 1) return At((r + 1, c)).ToString() + At((r + 2, c));
                if (r == (R + RC)/2) return At((r - 2, c)).ToString() + At((r - 1, c));
                if (c == CC - 1) return At((r, c + 1)).ToString() + At((r, c + 2));
                if (c == C - CC)return At((r, c - 2)).ToString() + At((r, c - 1));
                return "";
            }

            var labels = new Dictionary<string, List<(int r, int c)>>();

            void AddLabel((int r, int c) p)
            {
                var lbl = GetLabel(p);
                if (!labels.ContainsKey(lbl)) labels[lbl] = new List<(int r, int c)>();
                labels[lbl].Add(p);
            }

            Enumerable.Range(0, C).SelectMany(c => new[]{(R - 1, c), (0, c)}).Where(IsEdge)
                .ForEach(AddLabel);
            Enumerable.Range(0, R).SelectMany(r => new[]{(r, C - 1), (r, 0)}).Where(IsEdge)
                .ForEach(AddLabel);
            Enumerable.Range((R-RC)/2, RC).SelectMany(r => new[]{(r, CC - 1),(r,C-CC)}).Where(IsEdge)
                .ForEach(AddLabel);
            Enumerable.Range(CC, C - CC*2).SelectMany(c => new[]{((R-RC)/2-1, c),((R+RC)/2,c)}).Where(IsEdge)
                .ForEach(AddLabel);

            var start = labels["AA"][0];

            var queue = new Queue<(((int r, int c) point, int level) location, int length)>();
            var visited = new Dictionary<((int r, int c) point, int level), int>();
            queue.Enqueue(((start, 0), 0));
            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                if (visited.ContainsKey(item.location))
                {
                    continue;
                }
                visited.Add(item.location, item.length);

                if (IsEdge(item.location.point))
                {
                    var label = GetLabel(item.location.point);
                    if (label == "ZZ" && item.location.level == 0)
                    {
                        Console.WriteLine(item.length);
                        return;
                    }

                    var isOuter = IsOuter(item.location.point);
                    labels[label].Without(item.location.point)
                        .Select(p => (p, level: isOuter ? item.location.level - 1 : item.location.level + 1))
                        .Where(loc => loc.level >= 0)
                        .ForEach(p => queue.Enqueue((p, item.length + 1)));
                }

                item.location.point.Gen4Adjacent()
                    .Where(p => !p.OutOfBounds(R, C))
                    .Where(p => !p.InBounds(((R - RC) / 2 - 1), ((R + RC) / 2), CC - 1, C - CC))
                    .Where(p => At(p) == '.')
                    .Select(p => (p, item.location.level))
                    .Where(p => !visited.ContainsKey(p))
                    .ForEach(p => queue.Enqueue((p, item.length + 1)));
            }
        }

    }
}
