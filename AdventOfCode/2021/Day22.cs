using RegExtract;
using static System.Math;

namespace AdventOfCode._2021;

public class Day22 : Solution
{
    public Day22() : base(22, 2021) { }

    private readonly record struct Cube(int x1, int x2, int y1, int y2, int z1, int z2)
    {
        public (int x1, int x2) X => (x1, x2);
        public (int y1, int y2) Y => (y1, y2);
        public (int z1, int z2) Z => (z1, z2);
        public long V => (x2 - x1 + 1L) * (y2 - y1 + 1L) * (z2 - z1 + 1L);
    }
    protected override void Solve()
    {
        var input = ReadLines()
            .Map(
                line => (isOn: line.StartsWith("on"),
                    cube: line.Extract<Cube>(@".+ x=(-?\d+)..(-?\d+),y=(-?\d+)..(-?\d+),z=(-?\d+)..(-?\d+)")))
            // .TakeWhile(t => t.cube.x1 is >= -50 and <= 50) // p1
            .ToArray();

        var cubes = new List<Cube>();
        for (var i = 0; i < input.Length; i++)
        {
            var (isOn, cube) = input[i];
            AddCube(cube, 0, isOn);
            // var overlapping = cubes.Pairs().Where(pair => Overlaps(pair.first, pair.second)).ToArray();
            // if (overlapping.Any())
            // {
                // Console.WriteLine("test");
            // }
        }

        var total = cubes.Sum(c => c.V);
        total.ToString().Print();

        void AddCube(Cube cube, int checkFrom, bool isOn = true)
        {
            Cube cube2;
            do
            {
                if (checkFrom == cubes.Count)
                {
                    if (isOn)
                    {
                        // var overlapping = cubes.FirstOrDefault(c => Overlaps(c, cube));
                        // if (overlapping != default)
                        // {
                        //     Console.WriteLine("test");
                        // }
                        cubes.Add(cube);
                    }

                    return;
                }

                cube2 = cubes[checkFrom];
                if (Overlaps(cube, cube2))
                    break;

                checkFrom++;
            } while (true);

            var xInt = Intersect(cube.X, cube2.X);
            var yInt = Intersect(cube.Y, cube2.Y);
            var zInt = Intersect(cube.Z, cube2.Z);

            if (isOn)
            {
                var newCube = new Cube(xInt.x1, xInt.x2, yInt.x1, yInt.x2, zInt.x1, zInt.x2);
                
                // var overlapping = cubes.Except(cube2).FirstOrDefault(c => Overlaps(c, newCube));
                // if (overlapping != default)
                // {
                //     Console.WriteLine("test");
                // }

                cubes[checkFrom] = newCube;
            }

            if (isOn)
            {
                Except(cube.X, cube2.X)
                    .ForEach(t => AddCube(cube with { x1 = t.x1, x2 = t.x2 }, checkFrom + 1));

                Except(cube.Y, cube2.Y)
                    .ForEach(
                        t => AddCube(
                            cube with { x1 = xInt.x1, x2 = xInt.x2, y1 = t.x1, y2 = t.x2 },
                            checkFrom + 1));

                Except(cube.Z, cube2.Z)
                    .ForEach(
                        t => AddCube(
                            new Cube(xInt.x1, xInt.x2, yInt.x1, yInt.x2, t.x1, t.x2),
                            checkFrom + 1));
            }

            Except(cube2.X, cube.X)
                .ForEach(t => AddCube(cube2 with { x1 = t.x1, x2 = t.x2 }, checkFrom + 1));

            Except(cube2.Y, cube.Y)
                .ForEach(
                    t => AddCube(
                        cube2 with { x1 = xInt.x1, x2 = xInt.x2, y1 = t.x1, y2 = t.x2 },
                        checkFrom + 1));

            Except(cube2.Z, cube.Z)
                .ForEach(
                    t => AddCube(
                        new Cube(xInt.x1, xInt.x2, yInt.x1, yInt.x2, t.x1, t.x2),
                        checkFrom + 1));

            if (!isOn)
            {
                cubes.RemoveAt(checkFrom);
                AddCube(cube, checkFrom, isOn: false);
            }
        }

        IEnumerable<(int x1, int x2)> Except((int x1, int x2) c1, (int x1, int x2) c2)
        {
            if (c1.x1 < c2.x1) yield return (c1.x1, c2.x1 - 1);
            if (c1.x2 > c2.x2) yield return (c2.x2 + 1, c1.x2);
        }

        (int x1, int x2) Intersect((int x1, int x2) c1, (int x1, int x2) c2) => (Max(c1.x1, c2.x1), Min(c1.x2, c2.x2));

        bool Overlaps(Cube cube1, Cube cube2) =>
            (cube1.x2 >= cube2.x1 && cube1.x1 <= cube2.x1
            || cube2.x2 >= cube1.x1 && cube2.x1 <= cube1.x1)
            && (cube1.y2 >= cube2.y1 && cube1.y1 <= cube2.y1
             || cube2.y2 >= cube1.y1 && cube2.y1 <= cube1.y1)
            && (cube1.z2 >= cube2.z1 && cube1.z1 <= cube2.z1
             || cube2.z2 >= cube1.z1 && cube2.z1 <= cube1.z1);
    }
}