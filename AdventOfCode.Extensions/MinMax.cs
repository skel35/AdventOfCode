using System;
using System.Collections.Generic;

namespace AdventOfCode;

public static partial class AoC
{
    public static (int item, int index) MaxWithIndex(this IEnumerable<int> enumerable)
    {
        var maxV = int.MinValue;
        var maxI = -1;
        var i = 0;
        foreach (var item in enumerable)
        {
            if (item > maxV)
            {
                maxV = item;
                maxI = i;
            }

            i++;
        }

        return (maxV, maxI);
    }

    public static (int item, int index) MaxWithIndex(this IEnumerable<short> enumerable)
    {
        var maxV = short.MinValue;
        var maxI = -1;
        var i = 0;
        foreach (var item in enumerable)
        {
            if (item > maxV)
            {
                maxV = item;
                maxI = i;
            }

            i++;
        }

        return (maxV, maxI);
    }


    public static (T item, int index) MaxWithIndex<T>(this IEnumerable<T> enumerable, Func<T, int> selector)
        where T: struct
    {
        var maxV = int.MinValue;
        var maxI = -1;
        T maxItem = default;
        var i = 0;
        foreach (var item in enumerable)
        {
            var it = selector(item);
            if (it > maxV)
            {
                maxV = it;
                maxI = i;
                maxItem = item;
            }

            i++;
        }

        return (maxItem, maxI);
    }

    public static (T item, int index) MinWithIndex<T>(this IEnumerable<T> enumerable, Func<T, int> selector)
        where T: struct
    {
        var minV = int.MaxValue;
        var minI = -1;
        T minItem = default;
        var i = 0;
        foreach (var item in enumerable)
        {
            var it = selector(item);
            if (it < minV)
            {
                minV = it;
                minI = i;
                minItem = item;
            }

            i++;
        }

        return (minItem, minI);
    }
        
    public static (T item, int index) MinWithIndex<T>(this IEnumerable<T> enumerable) where T : IComparable<T>
    {
        var enumerator = enumerable.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            throw new ArgumentException("Empty enumerable!");
        }

        var minItem = enumerator.Current;
        var minI = 0;
        var i = 1;
        while (enumerator.MoveNext())
        {
            if (enumerator.Current.CompareTo(minItem) < 0)
            {
                minItem = enumerator.Current;
                minI = i;
            }

            i++;
        }

        return (minItem, minI);
    }

    public static (int item, int index) MinWithIndex(this IEnumerable<int> enumerable)
    {
        var minV = int.MaxValue;
        var minI = -1;
        var i = 0;
        foreach (var item in enumerable)
        {
            if (item < minV)
            {
                minV = item;
                minI = i;
            }

            i++;
        }

        return (minV, minI);
    }

    public static LinkedListNode<T> MinNode<T>(this LinkedList<T> enumerable, Func<T, int> selector)
    {
        var current = enumerable.First!;
        var minItem = current;
        var minV = selector(current.Value);
        while (current.Next != null)
        {
            current = current.Next;
            var v = selector(current.Value);
            if (v < minV)
            {
                minV = v;
                minItem = current;
            }
        }

        return minItem;
    }
        
    public static LinkedListNode<T> MaxNode<T>(this LinkedList<T> enumerable, Func<T, int> selector)
    {
        var current = enumerable.First!;
        var maxItem = current;
        var maxV = selector(current.Value);
        while (current.Next != null)
        {
            current = current.Next;
            var v = selector(current.Value);
            if (v > maxV)
            {
                maxV = v;
                maxItem = current;
            }
        }

        return maxItem;
    }
}