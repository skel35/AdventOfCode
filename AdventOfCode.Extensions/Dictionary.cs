using System;
using System.Collections.Generic;

namespace AdventOfCode;

public static partial class AoC
{
    public static void AddOrUpdate<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey keyToAdd,
        TValue valueToAdd,
        Func<TKey, TValue, TValue> updateValueFunc)
    {
        if (!dictionary.TryAdd(keyToAdd, valueToAdd))
        {
            dictionary[keyToAdd] = updateValueFunc(keyToAdd, dictionary[keyToAdd]);
        }
    }

}