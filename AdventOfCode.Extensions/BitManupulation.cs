using System.Collections.Generic;

namespace AdventOfCode
{
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


        public static bool Get(this uint bitArray, int index) => (bitArray >> index) % 2 == 1;
        public static uint Set(this uint bitArray, int index, bool val)
        {
            var valInt = val ? 1u : 0u;
            return bitArray | (valInt << index);
        }
    }
}