using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode._2021;

public class Day19 : Solution
{
    public Day19() : base(19, 2021) { }
    [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
    protected override void Solve()
    {
        var scanners = ReadText().Split("\n\n").Select(scanner => scanner.Split("\n").Skip(1).Select(line => line.ParseInts()).Select(a => (X: a[0], Y: a[1], Z: a[2])).ToArray()).ToArray();
        var all = (..scanners.Length).ToEnumerable().ToHashSet();
        var overlaps = new Dictionary<int, HashSet<int>>();
        var overlapInfo = new Dictionary<(int, int), OverlapInfo>();
        var toBase = new Dictionary<int, OverlapInfo>();

        void addOverlap(int i1, int i2) => overlaps.AddOrUpdate(
            i1,
            new HashSet<int> { i2 },
            (_, set) =>
            {
                set.Add(i2);
                return set;
            });
        scanners.WithIndex()
            .Pairs()
            .Where(t =>
            {
                var isOverlap = IsOverlapping(t.first.Item, t.second.Item, out var info);
                overlapInfo.Add((t.first.Index, t.second.Index), info);
                overlapInfo.Add((t.second.Index, t.first.Index), info.Inverse());
                return isOverlap;
            })
            .ForEach(t =>
            {
                var ((_, i1), (_, i2)) = t;
                addOverlap(i1, i2);
                addOverlap(i2, i1);
            });

        var queue = new Queue<int>();
        queue.Enqueue(0);
        while (queue.Count > 0)
        {
            var item = queue.Dequeue();
            if (item == 0)
            {
                toBase[item] = OverlapInfo.Identity();
            }
            else if (overlaps[item].Contains(0))
            {
                toBase[item] = overlapInfo[(0, item)];
            }
            else
            {
                var mid = overlaps[item].First(toBase.ContainsKey);

                // var t1 = overlapInfo[(item, mid)].Apply(toBase[mid]);
                // var t2 = overlapInfo[(mid, item)].Apply(toBase[mid]);
                // var t3 = toBase[mid].Apply(overlapInfo[(item, mid)]);
                var t4 = toBase[mid].Apply(overlapInfo[(mid, item)]);

                toBase[item] = t4;
            }
            all.Remove(item);
            overlaps[item].Where(all.Contains).ForEach(queue.Enqueue);
        }

        var allBeacons = new HashSet<(int X, int Y, int Z)>();
        for (var i = 0; i < scanners.Length; i++)
        {
            scanners[i].Select(x => toBase[i].Convert(x)).ForEach(x => allBeacons.Add(x));
        }

        var maxDist = toBase.Values.Select(x => x.Move).Pairs().Select(t => t.first.Tuple.Manhattan(t.second)).Max();

        // allBeacons.OrderBy(t => t.X).Print("\n");
        // p1:
        // allBeacons.Count.ToString().Print();
        // p2:
        maxDist.ToString().Print();
    }

    bool IsOverlapping((int X, int Y, int Z)[] s1, (int X, int Y, int Z)[] s2, out OverlapInfo overlapInfo)
    {
        (HashSet<int> set, (int i, int j))[] getDiffs((int X, int Y, int Z)[] s1) =>
            s1.WithIndex()
            .Pairs()
            .Select(
                t =>
                {
                    var diff = t.first.Item.Minus(t.second.Item);
                    var set = new[] { diff.x, diff.y, diff.z }.Select(Math.Abs).ToHashSet();
                    return (set, (t.first.Index, t.second.Index));
                }).ToArray();

        var diffs1 = getDiffs(s1);
        var diffs2 = getDiffs(s2);
        var matchingDiffs = diffs1.Select(y => (y, diffs2.FirstOrDefault(x => x.set.SetEquals(y.set))))
            .Where(t => t.Item2 != default)
            .Select(t => (t.Item1.Item2, t.Item2.Item2))
            .ToArray();
        var grouped = matchingDiffs.SelectMany(y => new[] { y.Item1.i, y.Item1.j })
            .GroupBy(x => x).ToArray();
        var max = grouped.Select(g => g.Count()).MaxOrDefault();
        // Console.WriteLine(max);
        if (max >= 11)
        {
            var indices1 = grouped.Select(g => g.Key).ToArray();
            var getIndex = indices1.ReverseMapping();
            var candidates = new HashSet<int>?[indices1.Length];
            var indices2 = new int[indices1.Length];
            indices2.InitWith(-1);
            var count = 0;
            for (var i = 0; i < matchingDiffs.Length; i++)
            {
                var d = matchingDiffs[i];
                var (i1, i2) = d.Item1;
                updateFor(i1);
                updateFor(i2);
                void updateFor(int i1)
                {
                    if (indices2[getIndex[i1]] == -1)
                    {
                        if (candidates[getIndex[i1]] is null)
                        {
                            candidates[getIndex[i1]] = new[] { d.Item2.i, d.Item2.j }.ToHashSet();
                        }
                        else
                        {
                            candidates[getIndex[i1]]!.IntersectWith(new[] { d.Item2.i, d.Item2.j });
                            indices2[getIndex[i1]] = candidates[getIndex[i1]]!.Single();
                            candidates[getIndex[i1]] = null;
                            count++;
                        }
                    }
                }
                if (count == indices1.Length) break;
            }

            // (count == indices1.Length).Assert();
            var from = s2[indices2[1]].Minus(s2[indices2[0]]);
            var to = s1[indices1[1]].Minus(s1[indices1[0]]);
            var transform = Transform.Create(from, to);
            // (transform.Apply(from) == to).Assert();
            // (transform.Inverse().Apply(to) == from).Assert();
            var move = s1[indices1[0]].Minus(transform.Apply(s2[indices2[0]]));
            overlapInfo = new(transform, move);
            // (overlapInfo.Convert(s2[indices2[3]]) == s1[indices1[3]]).Assert();
            // (overlapInfo.Inverse().Convert(s1[indices1[3]]) == s2[indices2[3]]).Assert();
            return true;
        }

        overlapInfo = default;
        return false;
    }

    readonly record struct OverlapInfo(Transform Transform, T3 Move)
    {
        public T3 Convert(T3 from)
        {
            return Transform.Apply(from).Tuple.Plus(Move);
        }

        public OverlapInfo Apply(OverlapInfo o1)
        {
            var tRes = Transform.Apply(o1.Transform);
            var mRes = Transform.Apply(o1.Move).Tuple.Plus(Move);
            return new(tRes, mRes);
        }

        public OverlapInfo Inverse()
        {
            var tInv = Transform.Inverse();
            var mInv = -(tInv.Apply(Move));
            return new(tInv, mInv);
        }

        public static OverlapInfo Identity()
        {
            return new(Transform.Identity(), (0, 0, 0));
        }
    }

    readonly record struct Transform(int XX, int XY, int XZ, int YX, int YY, int YZ, int ZX, int ZY, int ZZ)
    {
        public static Transform Create(T3 from, T3 to)
        {
            var xx = to.X == from.X ? 1 : to.X == -from.X ? -1 : 0;
            var xy = to.X == from.Y ? 1 : to.X == -from.Y ? -1 : 0;
            var xz = to.X == from.Z ? 1 : to.X == -from.Z ? -1 : 0;
            var yx = to.Y == from.X ? 1 : to.Y == -from.X ? -1 : 0;
            var yy = to.Y == from.Y ? 1 : to.Y == -from.Y ? -1 : 0;
            var yz = to.Y == from.Z ? 1 : to.Y == -from.Z ? -1 : 0;
            var zx = to.Z == from.X ? 1 : to.Z == -from.X ? -1 : 0;
            var zy = to.Z == from.Y ? 1 : to.Z == -from.Y ? -1 : 0;
            var zz = to.Z == from.Z ? 1 : to.Z == -from.Z ? -1 : 0;
            return new(xx, xy, xz, yx, yy, yz, zx, zy, zz);
        }

        public Transform Apply(Transform t1)
        {
            return
                new(
                    XX*t1.XX + XY*t1.YX + XZ*t1.ZX,
                    XX*t1.XY + XY*t1.YY + XZ*t1.ZY,
                    XX*t1.XZ + XY*t1.YZ + XZ*t1.ZZ,

                    YX*t1.XX + YY*t1.YX + YZ*t1.ZX,
                    YX*t1.XY + YY*t1.YY + YZ*t1.ZY,
                    YX*t1.XZ + YY*t1.YZ + YZ*t1.ZZ,

                    ZX*t1.XX + ZY*t1.YX + ZZ*t1.ZX,
                    ZX*t1.XY + ZY*t1.YY + ZZ*t1.ZY,
                    ZX*t1.XZ + ZY*t1.YZ + ZZ*t1.ZZ
                    );
        }

        public Transform Inverse()
        {
            return new(XX, YX, ZX, XY, YY, ZY, XZ, YZ, ZZ);
        }

        public T3 Apply(T3 from)
        {
            return
            (
                from.X * XX + from.Y * XY + from.Z * XZ,
                from.X * YX + from.Y * YY + from.Z * YZ,
                from.X * ZX + from.Y * ZY + from.Z * ZZ
            );
        }

        public static Transform Identity() => new(1, 0, 0, 0, 1, 0, 0, 0, 1);
    }
}