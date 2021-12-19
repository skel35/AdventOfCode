using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AdventOfCode;

public static class RangeExtensions
{
    public struct RangeEnumerator
    {
        public RangeEnumerator(int start, int end) => (Current, _end) = (start - 1, end);
        public int Current { get; private set; }
        private readonly int _end;
        public bool MoveNext() => ++Current < _end;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeEnumerator GetEnumerator(this Range range)
    {
        if (range.Start.IsFromEnd || range.End.IsFromEnd)
            ThrowIsFromEnd();

        if (range.Start.Value > range.End.Value)
            ThrowStartIsGreaterThanEnd();

        return new RangeEnumerator(range.Start.Value, range.End.Value);

        static void ThrowIsFromEnd() => throw new ArgumentException("range start and end must not be from end");
        static void ThrowStartIsGreaterThanEnd() => throw new ArgumentException("start is greater than end");
    }

    public static EnumerableRange ToEnumerable(this Range range)
    {
        return new EnumerableRange(range);
    }

}

public class EnumerableRange : IEnumerable<int>
{
    public Range Range { get; private set; }

    public EnumerableRange(Range range)
    {
        Range = range;
    }

    public IEnumerator<int> GetEnumerator()
    {
        var enumerator = Range.GetEnumerator();
        while (enumerator.MoveNext())
            yield return enumerator.Current;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}