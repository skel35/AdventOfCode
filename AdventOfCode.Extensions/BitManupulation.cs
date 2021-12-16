using System.Collections.Generic;
using System.Numerics;

namespace AdventOfCode;

public static partial class AoC
{
    public static int ToInt(this IList<bool> boolArray)
    {
        var n = 0;
        for (var i = 0; i < boolArray.Count; i++)
        {
            n *= 2;
            if (boolArray[i]) n += 1;
        }

        return n;
    }

    public static long ToLong(this IList<bool> boolArray)
    {
        var n = 0L;
        for (var i = 0; i < boolArray.Count; i++)
        {
            n *= 2;
            if (boolArray[i]) n += 1;
        }

        return n;
    }

    public static BigInteger ToBigInt(this IList<bool> boolArray)
    {
        var n = new BigInteger(0);
        for (var i = 0; i < boolArray.Count; i++)
        {
            n *= 2;
            if (boolArray[i]) n += 1;
        }

        return n;
    }


    public static int ToInt(this IEnumerable<bool> boolArray)
    {
        var n = 0;
        foreach (var item in boolArray)
        {
            n *= 2;
            if (item) n++;
        }

        return n;
    }

    public static bool[] ToBoolArray(this int n, int size)
    {
        var res = new bool[size];
        while (n > 0)
        {
            res[--size] = n % 2 == 1;
            n /= 2;
        }

        return res;
    }


    public static bool Get(this uint bitArray, int index) => (bitArray >> index) % 2 == 1;
    public static uint Set(this uint bitArray, int index, bool val)
    {
        var valInt = val ? 1u : 0u;
        return bitArray | (valInt << index);
    }
}