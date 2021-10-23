using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using static System.Console;

namespace AdventOfCode;

public static partial class AoC
{
    public static void Print(this IEnumerable<string> lines) => lines.ForEach(WriteLine);

    public static void Print(this IEnumerable<StringBuilder> lines) => lines.ForEach(WriteLine);

    public static void Print<T>(this T[,] twoDimArray, char separator = ' ')
    {
        for (var i = 0; i < twoDimArray.R(); i++)
        {
            for (var j = 0; j < twoDimArray.C() - 1; j++)
            {
                Write((twoDimArray[i,j]?.ToString() ?? " ") + separator);
            }

            WriteLine(twoDimArray[i,twoDimArray.C() - 1]);
        }
    }
    public static void Print<T>(this T[,] twoDimArray, (int, int) p, char separator = ' ')
    {
        for (var i = 0; i < twoDimArray.R(); i++)
        {
            for (var j = 0; j < twoDimArray.C() - 1; j++)
            {
                var str = twoDimArray[i, j]?.ToString() ?? " ";
                if (p.Item1 == i & p.Item2 == j)
                    Write(str + '_');
                else Write(str + separator);
            }
                
            WriteLine(twoDimArray[i, twoDimArray.C() - 1]);
        }
    }
        
    public static void Print<T>(this IList<IList<T>> twoDimArray, string separator = " ")
    {
        for (var i = 0; i < twoDimArray.Count; i++)
        {
            WriteLine(string.Join(separator, twoDimArray[i]));
        }
    }
        
    public static void Print<T>(this IList<T>[] twoDimArray, string separator = " ")
    {
        for (var i = 0; i < twoDimArray.Length; i++)
        {
            WriteLine(string.Join(separator, twoDimArray[i]));
        }
    }

    public static void Print<T>(this IList<T[]> twoDimArray, string separator = " ")
    {
        for (var i = 0; i < twoDimArray.Count; i++)
        {
            WriteLine(string.Join(separator, twoDimArray[i]));
        }
    }
        
    public static void Print<T>(this T[][] twoDimArray, string separator = " ")
    {
        for (var i = 0; i < twoDimArray.Length; i++)
        {
            WriteLine(string.Join(separator, twoDimArray[i]));
        }
    }

    public static void Print<T>(this IEnumerable<T> enumerable, string separator = " ") => WriteLine(string.Join(separator, enumerable));
    public static void Print(this string str) => WriteLine(str);
}