using System;

namespace AdventOfCode;

public static partial class AoC
{
    public static TRes Apply<TRes, TInput>(this TInput input, Func<TInput, TRes> func)
        => func(input);
}