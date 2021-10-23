using System;
using System.Collections.Generic;

namespace AdventOfCode;

public static partial class AoC
{
    // TODO: write generic selector value support (e.g. ValueTuple)

    public static T MaxBy<T>(this IEnumerable<T> enumerable, Func<T, int> selector)
        where T: struct
    {
        var maxV = int.MinValue;
        T maxItem = default;
        foreach (var item in enumerable)
        {
            var v = selector(item);
            if (v > maxV)
            {
                maxV = v;
                maxItem = item;
            }
        }

        return maxItem;
    }

    public static T MaxBy<T>(this IEnumerable<T> enumerable, Func<T, double> selector)
        where T: struct
    {
        var maxV = double.MinValue;
        T maxItem = default(T);
        foreach (var item in enumerable)
        {
            var v = selector(item);
            if (v > maxV)
            {
                maxV = v;
                maxItem = item;
            }
        }

        return maxItem;
    }

    public static T MaxBy<T>(this IEnumerable<T> enumerable, Func<T, long> selector)
        where T: struct
    {
        var maxV = long.MinValue;
        var maxItem = default(T);
        foreach (var item in enumerable)
        {
            var v = selector(item);
            if (v > maxV)
            {
                maxV = v;
                maxItem = item;
            }
        }

        return maxItem;
    }

    public static T MinBy<T>(this IEnumerable<T> enumerable, Func<T, int> selector)
        where T: struct
    {
        var minValue = int.MaxValue;
        var minItem = default(T);
        foreach (var item in enumerable)
        {
            var v = selector(item);
            if (v < minValue)
            {
                minValue = v;
                minItem = item;
            }
        }

        return minItem;
    }

    public static T MinBy<T>(this IEnumerable<T> enumerable, Func<T, double> selector)
        where T: struct
    {
        var minValue = double.MaxValue;
        var minItem = default(T);
        foreach (var item in enumerable)
        {
            var v = selector(item);
            if (v < minValue)
            {
                minValue = v;
                minItem = item;
            }
        }

        return minItem;
    }

    public static T MinBy<T>(this IEnumerable<T> enumerable, Func<T, long> selector)
        where T: struct
    {
        var minValue = long.MaxValue;
        var minItem = default(T);
        foreach (var item in enumerable)
        {
            var v = selector(item);
            if (v < minValue)
            {
                minValue = v;
                minItem = item;
            }
        }

        return minItem;
    }

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