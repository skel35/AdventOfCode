using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode
{
    public static partial class AoC
    {
        public static void Iter<T>(this T[,] twoDimArray, Action<int, int, T> action)
        {
            for (var x = 0; x < twoDimArray.GetLength(0); x++)
            {
                for (var y = 0; y < twoDimArray.GetLength(1); y++)
                {
                    action(x, y, twoDimArray[x,y]);
                }
            }
        }
        
        public static void Iter<T>(this T[,] twoDimArray, Action<int, int> action)
        {
            for (var x = 0; x < twoDimArray.GetLength(0); x++)
            {
                for (var y = 0; y < twoDimArray.GetLength(1); y++)
                {
                    action(x, y);
                }
            }
        }
        
        public static void Iter<T>(this T[,] twoDimArray, Action<T> action)
        {
            for (var x = 0; x < twoDimArray.GetLength(0); x++)
            {
                for (var y = 0; y < twoDimArray.GetLength(1); y++)
                {
                    action(twoDimArray[x,y]);
                }
            }
        }
        
        public static void Iter<T>(this T[][] twoDimArray, Action<int, int, T> action)
        {
            for (var x = 0; x < twoDimArray.Length; x++)
            {
                for (var y = 0; y < twoDimArray[x].Length; y++)
                {
                    action(x, y,twoDimArray[x][y]);
                }
            }
        }

        public static void Iter<T>(this T[][] twoDimArray, Action<int, int> action)
        {
            for (var x = 0; x < twoDimArray.Length; x++)
            {
                for (var y = 0; y < twoDimArray[x].Length; y++)
                {
                    action(x, y);
                }
            }
        }
        
        public static void Iter<T>(this T[][] twoDimArray, Action<T> action)
        {
            for (var x = 0; x < twoDimArray.Length; x++)
            {
                for (var y = 0; y < twoDimArray[x].Length; y++)
                {
                    action(twoDimArray[x][y]);
                }
            }
        }

        public static void Iter(this string[] twoDimArray, Action<int, int, char> action)
        {
            for (var x = 0; x < twoDimArray.Length; x++)
            {
                for (var y = 0; y < twoDimArray[x].Length; y++)
                {
                    action(x, y, twoDimArray[x][y]);
                }
            }
        }
        
        public static void Iter(this string[] twoDimArray, Action<int, int> action)
        {
            for (var x = 0; x < twoDimArray.Length; x++)
            {
                for (var y = 0; y < twoDimArray[x].Length; y++)
                {
                    action(x, y);
                }
            }
        }
        
        public static T[,] Map<T>(this string[] twoDimArray, Func<int, int, T> func)
        {
            var res = new T[twoDimArray.Length, twoDimArray[0].Length];
            for (var x = 0; x < twoDimArray.Length; x++)
            {
                for (var y = 0; y < twoDimArray[x].Length; y++)
                {
                    res[x, y] = func(x, y);
                }
            }

            return res;
        }
        
        public static void Iter(this string[] twoDimArray, Action<char> action)
        {
            for (var x = 0; x < twoDimArray.Length; x++)
            {
                for (var y = 0; y < twoDimArray[x].Length; y++)
                {
                    action(twoDimArray[x][y]);
                }
            }
        }

        
        public static void Iter(this StringBuilder[] twoDimArray, Action<int, int, char> action)
        {
            for (var x = 0; x < twoDimArray.Length; x++)
            {
                for (var y = 0; y < twoDimArray[x].Length; y++)
                {
                    action(x, y, twoDimArray[x][y]);
                }
            }
        }
        public static void Iter(this StringBuilder[] twoDimArray, Action<int, int> action)
        {
            for (var x = 0; x < twoDimArray.Length; x++)
            {
                for (var y = 0; y < twoDimArray[x].Length; y++)
                {
                    action(x, y);
                }
            }
        }
        
        public static T[,] Map<T>(this StringBuilder[] twoDimArray, Func<int, int, T> func)
        {
            var res = new T[twoDimArray.Length, twoDimArray[0].Length];
            for (var x = 0; x < twoDimArray.Length; x++)
            {
                for (var y = 0; y < twoDimArray[x].Length; y++)
                {
                    res[x, y] = func(x, y);
                }
            }

            return res;
        }
        
        public static T[,] Map<T>(this StringBuilder[] twoDimArray, Func<int, int, char, T> func)
        {
            var res = new T[twoDimArray.Length, twoDimArray[0].Length];
            for (var x = 0; x < twoDimArray.Length; x++)
            {
                for (var y = 0; y < twoDimArray[x].Length; y++)
                {
                    res[x, y] = func(x, y, twoDimArray[x][y]);
                }
            }

            return res;
        }

        public static TOut[,] Map<TIn, TOut>(this TIn[,] twoDimArray, Func<int, int, TOut> func)
        {
            var r = twoDimArray.GetLength(0);
            var c = twoDimArray.GetLength(1);
            var res = new TOut[r, c];
            for (var x = 0; x < r; x++)
            {
                for (var y = 0; y < c; y++)
                {
                    res[x, y] = func(x, y);
                }
            }

            return res;
        }

        public static TOut[,] Map<TIn, TOut>(this TIn[,] twoDimArray, Func<int, int, TIn, TOut> func)
        {
            var r = twoDimArray.GetLength(0);
            var c = twoDimArray.GetLength(1);
            var res = new TOut[r, c];
            for (var x = 0; x < r; x++)
            {
                for (var y = 0; y < c; y++)
                {
                    res[x, y] = func(x, y, twoDimArray[x, y]);
                }
            }

            return res;
        }
        
        public static TOut[,] Map<TIn, TOut>(this TIn[,] twoDimArray, Func<TIn, TOut> func)
        {
            var r = twoDimArray.GetLength(0);
            var c = twoDimArray.GetLength(1);
            var res = new TOut[r, c];
            for (var x = 0; x < r; x++)
            {
                for (var y = 0; y < c; y++)
                {
                    res[x, y] = func(twoDimArray[x, y]);
                }
            }

            return res;
        }
        
        public static TOut[][] Map<TIn, TOut>(this TIn[][] twoDimArray, Func<TIn, TOut> func)
        {
            var r = twoDimArray.GetLength(0);
            var c = twoDimArray.GetLength(1);
            var res = new TOut[r][];
            for (var x = 0; x < r; x++)
            {
                res[x] = new TOut[c];
                for (var y = 0; y < c; y++)
                {
                    res[x][y] = func(twoDimArray[x][y]);
                }
            }

            return res;
        }
        
        public static TOut[][] Map<TIn, TOut>(this TIn[][] twoDimArray, Func<int, int, TIn, TOut> func)
        {
            var r = twoDimArray.GetLength(0);
            var c = twoDimArray.GetLength(1);
            var res = new TOut[r][];
            for (var x = 0; x < r; x++)
            {
                res[x] = new TOut[c];
                for (var y = 0; y < c; y++)
                {
                    res[x][y] = func(x, y, twoDimArray[x][y]);
                }
            }

            return res;
        }
        
        public static TOut[][] Map<TIn, TOut>(this TIn[][] twoDimArray, Func<int, int, TOut> func)
        {
            var r = twoDimArray.GetLength(0);
            var c = twoDimArray.GetLength(1);
            var res = new TOut[r][];
            for (var x = 0; x < r; x++)
            {
                res[x] = new TOut[c];
                for (var y = 0; y < c; y++)
                {
                    res[x][y] = func(x, y);
                }
            }

            return res;
        }

        public static TOut[] ArrayMap<TIn, TOut>(this TIn[] array, Func<TIn, TOut> func)
        {
            var arrayOut = new TOut[array.Length];
            for (var i = 0; i < array.Length; i++) arrayOut[i] = func(array[i]);
            return arrayOut;
        }
        
        public static TOut[] ArrayMapi<TIn, TOut>(this TIn[] array, Func<int, TIn, TOut> func)
        {
            var arrayOut = new TOut[array.Length];
            for (var i = 0; i < array.Length; i++) arrayOut[i] = func(i, array[i]);
            return arrayOut;
        }
        
        public static List<TOut> ListMap<TIn, TOut>(this IList<TIn> list, Func<TIn, TOut> func)
        {
            var listOut = new List<TOut>(list.Count);
            for (var i = 0; i < list.Count; i++) listOut.Add(func(list[i]));
            return listOut;
        }
        
        public static List<TOut> ListMapi<TIn, TOut>(this IList<TIn> list, Func<int, TIn, TOut> func)
        {
            var listOut = new List<TOut>(list.Count);
            for (var i = 0; i < list.Count; i++) listOut.Add(func(i, list[i]));
            return listOut;
        }

        public static void Iter(this StringBuilder[] twoDimArray, Action<char> action)
        {
            for (var x = 0; x < twoDimArray.Length; x++)
            {
                for (var y = 0; y < twoDimArray[x].Length; y++)
                {
                    action(twoDimArray[x][y]);
                }
            }
        }
        
        public static void IterSqr<T>(this T[] array, Action<int, int, T, T> action)
        {
            for (var i = 0; i < array.Length; i++)
            {
                for (var j = 0; j < array.Length; j++)
                {
                    action(i, j, array[i], array[j]);
                }
            }
        }
        
        public static void IterSqr2<T>(this T[] array, Action<int, int, T, T> action)
        {
            for (var i = 0; i < array.Length - 1; i++)
            {
                for (var j = i + 1; j < array.Length; j++)
                {
                    action(i, j, array[i], array[j]);
                }
            }
        }
        
        public static void IterSqr<T>(this T[] array, Action<T, T> action)
        {
            for (var i = 0; i < array.Length; i++)
            {
                for (var j = 0; j < array.Length; j++)
                {
                    action(array[i], array[j]);
                }
            }
        }

        public static void IterSqr2<T>(this T[] array, Action<T, T> action)
        {
            for (var i = 0; i < array.Length - 1; i++)
            {
                for (var j = i + 1; j < array.Length; j++)
                {
                    action(array[i], array[j]);
                }
            }
        }
        
        public static void IterSqr(this int arrayLength, Action<int, int> action)
        {
            for (var i = 0; i < arrayLength; i++)
            {
                for (var j = 0; j < arrayLength; j++)
                {
                    action(i, j);
                }
            }
        }
        
        public static void IterSqr2(this int arrayLength, Action<int, int> action)
        {
            for (var i = 0; i < arrayLength - 1; i++)
            {
                for (var j = i + 1; j < arrayLength; j++)
                {
                    action(i, j);
                }
            }
        }
        
        public static void Iter(this int arrayLength, Action<int> action)
        {
            for (var i = 0; i < arrayLength; i++)
            {
                action(i);
            }
        }

        public static void Iteri<T>(this T[] array, Action<int, T> action)
        {
            for (var i = 0; i < array.Length; i++)
            {
                action(i, array[i]);
            }
        }

        // public static IEnumerable<TRes> Unfold<TRes, T>(this T item, Func<T, (TRes, T)?> generator)
        // {
        //     while (true)
        //     {
        //         var res = generator(item);
        //         if (res == null) yield break;
        //         yield return res.Value.Item1;
        //         item = res.Value.Item2;
        //     }
        // }

        public static IEnumerable<T> Unfold<T>(this T item, Func<T, (T, T)?> generator)
        {
            while (true)
            {
                var res = generator(item);
                if (res == null) yield break;
                yield return res.Value.Item1;
                item = res.Value.Item2;
            }
        }

    }
}