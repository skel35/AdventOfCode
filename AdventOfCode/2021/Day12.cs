using RegExtract;
using static System.String;

namespace AdventOfCode._2021;

public class Day12 : Solution
{
    public Day12() : base(12, 2021) { }
    protected override void Solve()
    {
        var caves = new Dictionary<string, int>
        {
            { "start", 0 },
            { "end", 1 },
        };
        // var names = new List<string> { "start", "end" };

        var map = new bool[20,20];
        var map2 = new List<int>[20];
        for (var i = 0; i < 20; i++) map2[i] = new List<int>();
        ReadLines().Select(s => s.Split('-'))
            .ForEach(s =>
            {
                if (!caves.ContainsKey(s[0])) caves.Add(s[0], caves.Count);
                if (!caves.ContainsKey(s[1])) caves.Add(s[1], caves.Count);
                map[caves[s[0]], caves[s[1]]] = true;
                map[caves[s[1]], caves[s[0]]] = true;
                map2[caves[s[0]]].Add(caves[s[1]]);
                map2[caves[s[1]]].Add(caves[s[0]]);
            });
        var names = caves.OrderBy(kvp => kvp.Value).Select(kvp => kvp.Key).ToArray();
        var paths = new Stack<Path>();
        var startingPath = new Path(new List<int> { 0 });
        paths.Push(startingPath);
        var completePaths = new HashSet<Path>();
        var visitedPaths = new HashSet<Path> { startingPath };
        while (paths.Count > 0)
        {
            var curPath = paths.Pop();
            // generate paths
            var next = map2[curPath.Vertices.Last()];
            for (var i = 0; i < next.Count; i++)
            {
                if (names[next[i]] == "end")
                {
                    var path = new List<int>();
                    path.AddRange(curPath.Vertices);
                    path.Add(next[i]);
                    var pathT = new Path(path, curPath.VisitedSmallCaveTwice);
                    completePaths.Add(pathT);
                    visitedPaths.Add(pathT);
                }
                else if (names[next[i]] == "start")
                {
                    continue;
                }
                else
                {
                    if (names[next[i]][0].Apply(char.IsUpper))
                    {
                        var path = new List<int>();
                        path.AddRange(curPath.Vertices);
                        path.Add(next[i]);
                        var pathT = new Path(path, curPath.VisitedSmallCaveTwice);
                        if (visitedPaths.Add(pathT))
                        {
                            paths.Push(pathT);
                        }
                    }
                    else
                    {
                        var count = curPath.Vertices.Count(v => v == next[i]);
                        if (!curPath.VisitedSmallCaveTwice && count <= 1 || count == 0)
                        {
                            var path = new List<int>();
                            path.AddRange(curPath.Vertices);
                            path.Add(next[i]);
                            var pathT = new Path(path, curPath.VisitedSmallCaveTwice || count == 1);
                            if (visitedPaths.Add(pathT))
                            {
                                paths.Push(pathT);
                            }
                        }
                    }
                }
            }
        }
        // completePaths.ForEach(p => Join(",", p.Vertices.Select(v => names[v])).Print());
        // completePaths.ForEach(p => p.Print());
        completePaths.Count.ToString().Print();
    }

    private class Path : IEquatable<Path>
    {
        public Path(List<int> vertices, bool visitedSmallCaveTwice = false)
        {
            Vertices = vertices;
            VisitedSmallCaveTwice = visitedSmallCaveTwice;
        }

        public bool VisitedSmallCaveTwice { get; }

        public List<int> Vertices { get; }

        public bool Equals(Path? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Vertices.SequenceEqual(other.Vertices);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Path) obj);
        }

        public override int GetHashCode() => Vertices.Aggregate(HashCode.Combine);
    }
}