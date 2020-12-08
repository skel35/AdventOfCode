using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public static partial class AoC
    {
        public static ref T At<T>(this T[,] twoDimArray, (int, int) point) => ref twoDimArray[point.Item1, point.Item2];
        public static ref T At<T>(this T[][] twoDimArray, (int, int) point) => ref twoDimArray[point.Item1][point.Item2];
        public static bool At(this BitArray[] twoDimArray, (int, int) point) => twoDimArray[point.Item1][point.Item2];
        public static char At(this string[] twoDimArray, (int, int) point) => twoDimArray[point.Item1][point.Item2];
        public static char At(this StringBuilder[] twoDimArray, (int, int) point) => twoDimArray[point.Item1][point.Item2];
        public static char SetAt(this StringBuilder[] twoDimArray, (int, int) point, char v) => twoDimArray[point.Item1][point.Item2] = v;
        public static int L<T>(this T[] array) => array.Length;
        public static int R<T>(this T[,] array) => array.GetLength(0);
        public static int C<T>(this T[,] array) => array.GetLength(1);
        public static int R<T>(this T[][] array) => array.Length;
        public static int C<T>(this T[][] array) => array[0].Length;

        public static int R(this string[] grid) => grid.Length;
        public static int C(this string[] grid) => grid[0].Length;
        // public static int Sum(this Span<int> span)
        // {
        //     var sum = 0;
        //     for (var i = 0; i < span.Length; i++) sum += span[i];
        //     return sum;
        // }
        public static T[,] To2D<T>(this T[][] source)
        {
            try
            {
                var firstDim = source.Length;
                var secondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

                var result = new T[firstDim, secondDim];
                for (var i = 0; i < firstDim; i++)
                {
                    for (var j = 0; j < secondDim; j++)
                    {
                        result[i, j] = source[i][j];
                    }
                }

                return result;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("The given jagged array is not rectangular.");
            } 
        }

        public static T[] DeepCopy<T>(this T[] source)
        {
            var res = new T[source.Length];
            Array.Copy(source, res, source.Length);
            return res;
        }
    }
}