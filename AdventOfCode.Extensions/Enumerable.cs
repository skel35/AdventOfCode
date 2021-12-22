using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace AdventOfCode;

public static partial class AoC
{
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var item in enumerable) action(item);
    }

    public static void Execute<T>(this IEnumerable<T> enumerable)
    {
        foreach (var _ in enumerable) { }
    }
    public static void ForEach<T, TRes>(this IEnumerable<T> enumerable, Func<T, TRes> func)
    {
        foreach (var item in enumerable)
        {
            func(item);
        }
    }

    public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, params T[] items)
    {
        return Enumerable.Concat(enumerable, items);
    }

    /// <summary>
    /// sequence-based
    /// </summary>
    public static IEnumerable<T> Without<T>(this IEnumerable<T> enumerable, params T[] items)
    {
        return enumerable.Where(it => items.All(item => !Equals(item, it)));
    }

    /// <summary>
    /// set-based
    /// </summary>
    public static IEnumerable<T> Except<T>(this IEnumerable<T> enumerable, params T[] items)
    {
        return Enumerable.Except(enumerable, items);
    }

    public static IEnumerable<(T Item, int Index)> WithIndex<T>(this IEnumerable<T> enumerable)
    {
        var i = 0;
        foreach (var item in enumerable)
        {
            yield return (item, i);
            i++;
        }
    }

    public static void Iteri<T>(this IEnumerable<T> collection, Action<int, T> action)
    {
        var i = 0;
        foreach (var item in collection)
        {
            action(i, item);
            i++;
        }
    }

    public static IEnumerable<TOut> Map<TIn, TOut>(this IEnumerable<TIn> collection, Func<TIn, TOut> func) => collection.Select(func);

    public static IEnumerable<TOut> Mapi<TIn, TOut>(this IEnumerable<TIn> collection, Func<int, TIn, TOut> func)
    {
        var i = 0;
        foreach (var item in collection)
        {
            yield return func(i, item);
            i++;
        }
    }

    public static IEnumerable<TState> Scan<TSource, TState>(
        this IEnumerable<TSource> collection,
        Func<TState, TSource, TState> folder,
        TState initialState)
    {
        var state = initialState;
        foreach(var item in collection)
        {
            state = folder(state, item);
            yield return state;
        }
    }

    public static long SumLong(this IEnumerable<int> enumerable) =>
        enumerable.Aggregate<int, long>(0, (current, n) => current + n);

    public static long SumLong<T>(this IEnumerable<T> enumerable, Func<T, int> valueFunc) =>
        enumerable.Aggregate<T, long>(0, (current, n) => valueFunc(n) + current);

    public static ulong Sum(this IEnumerable<ulong> s)
        => s.Aggregate(0ul, (current, n) => current + n);

    public static BigInteger Sum(this IEnumerable<BigInteger> s)
        => s.Aggregate(new BigInteger(0), (current, n) => current + n);

    public static BigInteger Product(this IEnumerable<BigInteger> s)
        => s.Aggregate(new BigInteger(1), (current, n) => current * n);

    public static T[] ToArray<T>(this IEnumerable<T> enumerable, int capacity)
    {
        var res = new T[capacity];
        var i = 0;
        foreach (var item in enumerable) res[i++] = item;
        return res;
    }

    public static List<T> ToList<T>(this IEnumerable<T> enumerable, int capacity)
    {
        var res = new List<T>(capacity);
        res.AddRange(enumerable);
        return res;
    }

    public static IEnumerable<(T first, T second)> Pairs<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.ToArray().Pairs();
    }

    public static (T first, T second)[] Pairs<T>(this T[] array)
    {
        if (array.Length < 2) return Array.Empty<(T first, T second)>();
        var res = new (T first, T second)[array.Length * (array.Length - 1) / 2];
        var count = 0;
        for (var i = 0; i < array.Length - 1; i++)
        {
            for (var j = i + 1; j < array.Length; j++)
            {
                res[count++] = (array[i], array[j]);
            }
        }

        return res;
    }

    public static (T first, T second, T third)[] Triples<T>(this T[] array)
    {
        if (array.Length < 3) throw new ArgumentException(nameof(array));
        var res = new (T first, T second, T third)[array.Length * (array.Length - 1) * (array.Length - 2) / 6];
        var count = 0;
        for (var i = 0; i < array.Length - 1; i++)
        {
            for (var j = i + 1; j < array.Length; j++)
            {
                for (var k = j + 1; k < array.Length; k++)
                    res[count++] = (array[i], array[j], array[k]);
            }
        }

        return res;
    }

    public static IEnumerable<(TSource first, TSource second)> Pairwise<TSource>(this IEnumerable<TSource> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return _(); IEnumerable<(TSource, TSource)> _()
        {
            using var e = source.GetEnumerator();

            if (!e.MoveNext())
                yield break;

            var previous = e.Current;
            while (e.MoveNext())
            {
                yield return (previous, e.Current);
                previous = e.Current;
            }
        }
    }


    public static IEnumerable<TResult> Pairwise<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource, TResult> resultSelector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

        return _(); IEnumerable<TResult> _()
        {
            using var e = source.GetEnumerator();

            if (!e.MoveNext())
                yield break;

            var previous = e.Current;
            while (e.MoveNext())
            {
                yield return resultSelector(previous, e.Current);
                previous = e.Current;
            }
        }
    }

    public static IEnumerable<TResult> Triplewise<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource, TResult> resultSelector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

        return _(); IEnumerable<TResult> _()
        {
            using var e = source.GetEnumerator();

            if (!e.MoveNext())
                yield break;

            var previousPrevious = e.Current;
            if (!e.MoveNext())
                yield break;

            var previous = e.Current;
            while (e.MoveNext())
            {
                yield return resultSelector(previousPrevious, previous, e.Current);
                previousPrevious = previous;
                previous = e.Current;
            }
        }
    }

    public static TResult If<TSource, TResult>(
        this IEnumerable<TSource> source,
        bool condition,
        Func<IEnumerable<TSource>, TResult> branchIf,
        Func<IEnumerable<TSource>, TResult> branchElse)
    {
        return (condition ? branchIf : branchElse)(source);
    }

    public static TResult If<TSource, TResult>(
        this IList<TSource> source,
        Func<IList<TSource>, bool> condition,
        Func<IList<TSource>, TResult> branchIf,
        Func<IList<TSource>, TResult> branchElse)
    {
        return (condition(source) ? branchIf : branchElse)(source);
    }
}