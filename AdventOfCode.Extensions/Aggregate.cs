using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode;

public static partial class AoC
{
    public static long Product(this IEnumerable<long> enumerable)
        => enumerable.Aggregate(1L, (x, y) => x * y);

    public static int Product(this IEnumerable<int> enumerable)
        => enumerable.Aggregate(1, (x, y) => x * y);
    public static long ProductLong(this IEnumerable<int> enumerable)
        => enumerable.Aggregate(1L, (x, y) => x * y);
}