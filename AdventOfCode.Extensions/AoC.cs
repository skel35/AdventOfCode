using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AdventOfCode;

public static partial class AoC
{
    public static T Circular<T>(this IList<T> list, int index, int moveBy)
    {
        return list[(index + moveBy + list.Count) % list.Count];
    }
    public static ref T Circular<T>(this T[] array, int index, int moveBy)
    {
        return ref array[(index + moveBy + array.Length) % array.Length];
    }
    public static void SetCircular<T>(this IList<T> list, int index, int moveBy, T value)
    {
        list[(index + moveBy + list.Count) % list.Count] = value;
    }
    public static LinkedListNode<T> NextCircular<T>(this LinkedList<T> list, LinkedListNode<T> current, int n)
    {
        if (n < 0) return list.PrevCircular(current, -n);
        for (var i = 0; i < n; i++) current = current.Next ?? list.First!;
        return current;
    }

    public static LinkedListNode<T> PrevCircular<T>(this LinkedList<T> list, LinkedListNode<T> current, int n)
    {
        for (var i = 0; i < n; i++) current = current.Previous ?? list.Last!;
        return current;
    }

    public static void Swap<T>(ref T a, ref T b) where T : notnull
        => (a, b) = (b, a);

    /// <summary>
    /// l can be either more or less than r.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBetween(this int a, int l, int r)
    {
        return (a > l && a < r) || (a > r && a < l);
    }

    /// <summary>
    /// l can be either more or less than r.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBetweenNonStrict(this int a, int l, int r)
    {
        return (a >= l && a <= r) || (a >= r && a <= l);
    }

    public static void Assert(this bool condition)
    {
        if (!condition) throw new InvalidOperationException();
    }
    public static void Assert(this bool condition, string assertionMessage)
    {
        if (!condition) throw new InvalidOperationException(assertionMessage);
    }

}