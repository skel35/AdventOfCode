using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public static partial class AoC
    {
        public static int[] ParseInts(this string str) => str.SplitAndDo(int.Parse);
        public static long[] ParseLongs(this string str) => str.SplitAndDo(long.Parse);
        public static short[] ParseShorts(this string str) => str.SplitAndDo(short.Parse);
        public static byte[] ParseBytes(this string str) => str.SplitAndDo(byte.Parse);
        public static double[] ParseDoubles(this string str) => str.SplitAndDo(double.Parse);
        public static int[] ParseInts(this string str, int capacity) => str.SplitAndDo(int.Parse, capacity);
        public static long[] ParseLongs(this string str, int capacity) => str.SplitAndDo(long.Parse, capacity);
        public static short[] ParseShorts(this string str, int capacity) => str.SplitAndDo(short.Parse, capacity);
        public static byte[] ParseBytes(this string str, int capacity) => str.SplitAndDo(byte.Parse, capacity);
        public static double[] ParseDoubles(this string str, int capacity) => str.SplitAndDo(double.Parse, capacity);

        public static T[] SplitAndDo<T>(this string str, Func<string, T> parseFunc) =>
            str.Split(new[] {',', ';', ' ', '\t', '\n', '\r'}, StringSplitOptions.RemoveEmptyEntries).Select(parseFunc).ToArray();
        public static T[] SplitAndDo<T>(this string str, Func<string, T> parseFunc, int capacity) =>
            str.Split(new[] {',', ';', ' ', '\t', '\n', '\r'}, StringSplitOptions.RemoveEmptyEntries).Select(parseFunc).ToArray(capacity);

        public static string[] Split2(this string str) =>
            str.Split(new[] {',', ';', ' ', '\t', '\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);

        public static string[] SplitBy(this string str, params char[] pars) => str.Split(pars, StringSplitOptions.RemoveEmptyEntries);
        public static string Ss(this string str, string before, params string[] after)
        {
            var i11 = str.IndexOf(before, StringComparison.Ordinal);
            if (i11 < 0) return "";
            var i1 = i11 + before.Length;
            var i2 = str.Length;
            foreach (var s in after)
            {
                var ind = str.IndexOf(s, i1, StringComparison.Ordinal);
                if (ind >= 0 && ind < i2) i2 = ind;
            }
            return str[i1..i2];
        }
        
        public static string ToStr(this IEnumerable<char> chars) => string.Concat(chars);
        public static string Sort(this string str) => str.OrderBy(c => c).ToStr();
        public static StringBuilder ToSb(this string str) => new(str);
        public static StringBuilder[] ToSb(this string[] strArray)
        {
            var res = new StringBuilder[strArray.Length];
            for (var i = 0; i < res.Length; i++) res[i] = strArray[i].ToSb();
            return res;
        }

        public static string[] ToStringArray(this StringBuilder[] array)
        {
            return array.ArrayMap(sb => sb.ToString());
        }
        public static int ToInt(this char c) => (int) char.GetNumericValue(c);
        public static int ToInt(this string s) => int.Parse(s);

        public static long ToLong(this string s) => long.Parse(s);
        public static string ToHexString(this byte[] array) => string.Concat(array.Select(b => b.ToString("x2")).ToArray());
        public static char Last(this string s) => s[^1];
    }
}