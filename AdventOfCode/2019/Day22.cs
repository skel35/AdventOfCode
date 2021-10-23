using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2019;

public class Day22 : Solution
{
    public Day22() : base(22, 2019) { }
    protected override void Solve()
    {
        var N = 119315717514047;
        // var N = 10007;
        var M = 101741582076661;
        // var M = 1;
        var a = 1L;
        var b = 0L;
        // position i contains (a*i + b) % N
        // var cards = Enumerable.Range(0, N).ToArray();
        var input = ReadLines().Select(line =>
        {
            if (line.StartsWith("deal with increment"))
            {
                var spaceIndex = line.LastIndexOf(' ');
                var n = int.Parse(line[(spaceIndex + 1)..]);
                return (0, n);
            }
            else if (line.StartsWith("deal into new stack"))
            {
                return (1, 0);
            }
            else
            {
                var n = line.Split()[1].ToInt();
                return (2, n);
            }
        }).ToArray();
        for (var j = input.Length - 1; j >= 0; j--)
        {
            var (op, k) = input[j];
            switch (op)
            {
                case 0:
                {
                    var kInv = ModInverse(k, N);
                    var a2 = Mulmod(a, kInv, N);
                    var b2 = Mulmod(b, kInv, N);
                    a = a2;
                    b = b2;
                    break;
                }
                case 1:
                {
                    b++;
                    b *= -1;
                    a *= -1;
                    break;
                }
                default:
                    long iCut = k;
                    if (iCut < 0) iCut += N;
                    b += iCut;
                    break;
            }

            a = a % N;
            b = b % N;
            if (a < 0) a += N;
            if (b < 0) b += N;
        }

        var aRes = Modular(a, M, N);
        var result = (Mulmod(aRes, 2020, N) + Mulmod(b, Mulmod((Modular(a, M, N) + N - 1) % N,
            Modular(a - 1, N - 2, N), N), N)) % N;
        Console.WriteLine(result);
    }
    static long Mulmod(long a, long b, long mod)
    {
        long res = 0; // Initialize result
        a = a % mod;
        while (b > 0)
        {
            // If b is odd, add 'a' to result
            if (b % 2 == 1)
            {
                res = (res + a) % mod;
            }

            // Multiply 'a' with 2
            a = (a * 2) % mod;

            // Divide b by 2
            b /= 2;
        }

        // Return result
        return res % mod;
    }
    static long Modular(long b, long exp, long mod)
    {
        long x = 1;
        long i;
        long power = b % mod;

        for (i = 0; i < sizeof(long) * 8; i++) {
            long leastSigBit = 0x00000001 & (exp >> (int)i);
            if (leastSigBit > 0)
                x = Mulmod(x, power, mod);
            power = Mulmod(power, power, mod);
        }

        return x;
    }
    static long ModInverse(long a, long m)
    {
        var m0 = m;
        long y = 0, x = 1;

        if (m == 1)
            return 0;

        while (a > 1)
        {
            // q is quotient
            var q = a / m;

            var t = m;

            // m is remainder now, process
            // same as Euclid's algo
            m = a % m;
            a = t;
            t = y;

            // Update x and y
            y = x - q * y;
            x = t;
        }

        // Make x positive
        if (x < 0)
            x += m0;

        return x;
    }
}