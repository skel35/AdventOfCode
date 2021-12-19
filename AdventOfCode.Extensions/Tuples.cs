using System;
using System.Linq;
using System.Text;

namespace AdventOfCode;

public static partial class AoC
{
    public static (int R, int C) Avg(this (int R, int C) p1, (int R, int C) p2) => ((p1.R + p2.R) / 2, (p1.C + p2.C) / 2);
    public static (int x, int y, int z) Avg(this (int x, int y, int z) p1, (int x, int y, int z) p2) => ((p1.x + p2.x) / 2, (p1.y + p2.y) / 2, (p1.z + p2.z) / 2);
    public static (long x, long y, long z) Avg(this (long x, long y, long z) p1, (long x, long y, long z) p2) => ((p1.x + p2.x) / 2, (p1.y + p2.y) / 2, (p1.z + p2.z) / 2);
    public static (long x, long y, long z) Div(this (long x, long y, long z) t, long n) => (t.x / n, t.y / n, t.z / n);
    public static (int x, int y, int z) Div(this (int x, int y, int z) t, int n) => (t.x / n, t.y / n, t.z / n);
    public static (int R, int C) Div(this (int R, int C) t, int n) => (t.R / n, t.C / n);
    public static (long x, long y, long z) Plus(this (long x, long y, long z) t, (long x, long y, long z) b) => (t.x + b.x, t.y + b.y, t.z + b.z);
    public static (int x, int y, int z) Plus(this (int x, int y, int z) t, (int x, int y, int z) b) => (t.x + b.x, t.y + b.y, t.z + b.z);
    public static (int R, int C) Plus(this (int R, int C) t, (int R, int C) b) => (t.R + b.R, t.C + b.C);
    // public static T2 Plus(this T2 t, T2 b) => (t.R + b.R, t.C + b.C);
    public static (int R, int C) Mult(this (int R, int C) t, int n) => (t.R * n, t.C * n);
    public static (int x, int y, int z) Mult(this (int x, int y, int z) t, int n) => (t.x * n, t.y * n, t.z * n);
    public static (int R, int C) Minus(this (int R, int C) t, (int R, int C) b) => (t.R - b.R, t.C - b.C);
    public static (int x, int y, int z) Minus(this (int x, int y, int z) t, (int x, int y, int z) b) => (t.x - b.x, t.y - b.y, t.z - b.z);

    public static (int R, int C) Right(this (int R, int C) d) => (d.C, -d.R);
    public static (int R, int C) Left(this (int R, int C) d) => (-d.C, d.R);
    public static (int R, int C) Opposite(this (int R, int C) d) => (-d.R, -d.C);

    public static int Manhattan(this (int x, int y, int z) p1, (int x, int y, int z) p2) =>
        Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y) + Math.Abs(p1.z - p2.z);
    public static int Manhattan(this (int x, int y, int z) p1) => Math.Abs(p1.x) + Math.Abs(p1.y) + Math.Abs(p1.z);
    public static int Manhattan(this (int R, int C) p1) => Math.Abs(p1.R) + Math.Abs(p1.C);
    public static int Manhattan(this (int R, int C) p1, (int R, int C) p2) => Math.Abs(p1.R - p2.R) + Math.Abs(p1.C - p2.C);

    public static double Euclidean(this (int R, int C) p1, (int R, int C) p2) =>
        Math.Sqrt((p2.R - p1.R) * (p2.R - p1.R) + (p2.C - p1.C) * (p2.C - p1.C));
    public static double Euclidean(this (int R, int C) p1) => Math.Sqrt(p1.R * p1.R + p1.C * p1.C);
    public static double Euclidean(this (int x, int y, int z) p1, (int x, int y, int z) p2) =>
        Math.Sqrt((p2.x - p1.x) * (p2.x - p1.x) + (p2.y - p1.y) * (p2.y - p1.y) + (p2.z - p1.z)*(p2.z - p1.z));
    public static double Euclidean(this (int x, int y, int z) p1) => Math.Sqrt(p1.x * p1.x + p1.y * p1.y + p1.z * p1.z);
    public static bool OutOfBounds(this (int R, int C) point, int size)
    {
        return point.R < 0 || point.C < 0 || point.R >= size || point.C >= size;
    }

    public static bool OutOfBounds(this (int R, int C) point, int sizeR, int sizeC)
    {
        return point.R < 0 || point.C < 0 || point.R >= sizeR || point.C >= sizeC;
    }
    public static bool InBounds(this (int R, int C) point, int rLeft, int rRight, int cLeft, int cRight)
    {
        return point.R > rLeft && point.C > cLeft && point.R < rRight && point.C < cRight;
    }

    public static bool InBounds(this (int R, int C) point, StringBuilder[] grid)
    {
        return point.R >= 0 && point.C >= 0 && point.R < grid.Length && point.C < grid[0].Length;
    }

    public static bool InBounds<T>(this (int R, int C) point, T[,] grid)
    {
        return point.R >= 0 && point.C >= 0 && point.R < grid.GetLength(0) && point.C < grid.GetLength(1);
    }

    public static bool InBounds<T>(this (int R, int C) point, T[][] grid)
    {
        return point.R >= 0 && point.C >= 0 && point.R < grid.Length && point.C < grid[0].Length;
    }

    public static bool InBounds(this (int R, int C) point, string[] grid)
    {
        return point.R >= 0 && point.C >= 0 && point.R < grid.Length && point.C < grid[0].Length;
    }

    public static bool OutOfBounds(this (int x, int y, int z) point, int size)
    {
        return point.x < 0 || point.y < 0 || point.z < 0 ||
               point.x >= size || point.y >= size || point.z >= size;
    }

    public static bool OutOfBounds(this (int x, int y, int z) point, int sizeX, int sizeY, int sizeZ)
    {
        return point.x < 0 || point.y < 0 || point.z < 0 ||
               point.x >= sizeX || point.y >= sizeY || point.z >= sizeZ;
    }

    public static bool OutOfBounds(this (int x, int y, int z, int w) point, int sizeX, int sizeY, int sizeZ, int sizeW)
    {
        return point.x < 0 || point.y < 0 || point.z < 0 || point.w < 0 ||
               point.x >= sizeX || point.y >= sizeY || point.z >= sizeZ || point.w >= sizeW;
    }

    public static (int R, int C)[] Gen8Adjacent(this (int R, int C) point)
    {
        var (r, c) = point;
        return new[]
        {
            (r - 1, c), (r - 1, c - 1), (r - 1, c + 1),
            (r, c - 1), (r, c + 1),
            (r + 1, c - 1), (r + 1, c), (r + 1, c + 1)
        };
    }

    public static (int X, int Y, int Z)[] Gen26Adjacent(this (int X, int Y, int Z) point)
    {
        var (x, y, z) = point;
        return (x, y)
            .Gen8Adjacent()
            .Select(t => new[] {(t.R, t.C, z), (t.R, t.C, z - 1), (t.R, t.C, z + 1)})
            .Append(new[] {(x, y, z - 1), (x, y, z + 1)})
            .SelectMany(a => a)
            .ToArray();
    }

    public static (int X, int Y, int Z, int W)[] Gen80Adjacent(this (int X, int Y, int Z, int W) point)
    {
        var (x, y, z, w) = point;
        return (x, y, z)
            .Gen26Adjacent()
            .Select(t => new[] {(t.X, t.Y, t.Z, w), (t.X, t.Y, t.Z, w - 1), (t.X, t.Y, t.Z, w + 1)})
            .Append(new[] {(x, y, z, w - 1), (x, y, z, w + 1)})
            .SelectMany(a => a)
            .ToArray();
    }

    public static (int R, int C)[] Gen4Adjacent(this (int, int) point)
    {
        var (r, c) = point;
        return new[] {(r - 1, c), (r, c - 1), (r + 1, c), (r, c + 1)};
    }

    public static (int R, int C)? GridIntersection(
        (int R, int C) p11, (int R, int C) p12,
        (int R, int C) p21, (int R, int C) p22)
    {
        if (p11.R == p12.R)
        {
            if (p11.R.IsBetween(p21.R, p22.R)
                && p21.C.IsBetween(p11.C, p12.C))
            {
                return (p11.R, p21.C);
            }
        }
        else if (p11.C == p12.C)
        {
            if (p11.C.IsBetween(p21.C, p22.C)
                && p21.R.IsBetween(p11.R, p12.R))
            {
                return (p21.R, p11.C);
            }
        }

        return null;
    }
}

public readonly struct T2 : IEquatable<T2>, IEquatable<(int R, int C)>
{
    public T2(int a, int b) : this((a, b))
    {
    }

    public T2((int R, int C) tuple)
    {
        Tuple = tuple;
    }
    public (int R, int C) Tuple { get; }
    public int R => Tuple.R;
    public int C => Tuple.C;

    public static implicit operator (int R, int C)(T2 d) => d.Tuple;
    public static implicit operator T2((int R, int C) tuple) => new(tuple);

    public bool Equals(T2 other) => Tuple.Equals(other.Tuple);
    public bool Equals((int R, int C) other) => Tuple.Equals(other);
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is T2 other && Equals(other);
    }
    public override int GetHashCode() => Tuple.GetHashCode();

    public static bool operator ==(T2 left, T2 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(T2 left, T2 right)
    {
        return !(left == right);
    }
}

public readonly struct T3 : IEquatable<T3>, IEquatable<(int x, int y, int z)>
{
    public T3((int x, int y, int z) tuple)
    {
        Tuple = tuple;
    }

    public T3(int a, int b, int c) : this((a, b, c))
    {
    }

    public int X => Tuple.x;
    public int Y => Tuple.y;
    public int Z => Tuple.z;
    public (int x, int y, int z) Tuple { get; }

    public static implicit operator (int x, int y, int z)(T3 d) => d.Tuple;
    public static implicit operator T3((int x, int y, int z) tuple) => new(tuple);
    public override int GetHashCode() => Tuple.GetHashCode();
    public bool Equals(T3 other) => Tuple.Equals(other.Tuple);
    public bool Equals((int x, int y, int z) other) => Tuple.Equals(other);
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is T3 other && Equals(other);
    }

    public static bool operator ==(T3 left, T3 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(T3 left, T3 right)
    {
        return !(left == right);
    }

    public static T3 operator -(T3 left)
    {
        return new(-left.X, -left.Y, -left.Z);
    }
}

public readonly struct V4 : IEquatable<V4>
{
    public V4((int R, int C) p1,
        (int R, int C) p2,
        (int R, int C) p3,
        (int R, int C) p4)
    {
        P1 = p1;
        P2 = p2;
        P3 = p3;
        P4 = p4;
    }

    public (int R, int C) P1 { get; }
    public (int R, int C) P2 { get; }
    public (int R, int C) P3 { get; }
    public (int R, int C) P4 { get; }
    public (int R, int C)[] ToPointsArray() => new[] {P1, P2, P3, P4};

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = P1.GetHashCode();
            hashCode = (hashCode * 397) ^ P2.GetHashCode();
            hashCode = (hashCode * 397) ^ P3.GetHashCode();
            hashCode = (hashCode * 397) ^ P4.GetHashCode();
            return hashCode;
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is V4 other && Equals(other);
    }

    public bool Equals(V4 other)
    {
        return P1.Equals(other.P1) && P2.Equals(other.P2) && P3.Equals(other.P3) && P4.Equals(other.P4);
    }

    public static bool operator ==(V4 left, V4 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(V4 left, V4 right)
    {
        return !(left == right);
    }
}