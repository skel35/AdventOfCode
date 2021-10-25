using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static AdventOfCode.AoC;

namespace AdventOfCode;

public static class AocSpecial
{
    public static byte[] KnotHash(this string str)
    {
        var inp = str.Select(c => (int) c).Concat(17, 31, 73, 47, 23).ToArray();
        var current = 0;
        var skipSize = 0;
        const int size = 256;
        var list = Enumerable.Range(0, size).ToArray();
        for (var round = 0; round < 64; round++)
        {
            var lengths = inp.ToArray();
            foreach (var length in lengths)
            {
                var jFront = current;
                var jBack = (current + length) % size;
                for (var i = 0; i < length / 2; i++)
                {
                    (list[jFront], list[jBack]) = (list[jBack], list[jFront]); 
                    jFront = (jFront + 1) % size;
                    jBack = (jBack - 1 + size) % size;
                }
                current = (current + length + skipSize) % size;
                skipSize++;
            }
        }

        var res = new byte[16];
        for (var j = 0; j < 16; j++)
        {
            res[j] = list.Skip(j * 16).Take(16).Aggregate((byte)0, (b1, b2) => (byte)(b1 ^ b2));
        }
        return res;
    }

    public class ElvesAssembly
    {
        public static void Solve()
        {
            //            var input = File.ReadAllText("input.txt");
            var lines = File.ReadAllLines("input.txt");
            var p = lines[0].Split().Last().ToInt();
            var regs = new int[6];
            regs[0] = 1;
            var ip = 0;
            while (true)
            {
                if (ip + 1 >= lines.Length || ip + 1 < 0)
                {
                    Console.WriteLine(regs[0]);
                    return;
                }
                var s = lines[ip + 1].Split();
                var op = s[0];
                var pars = s.Skip(1).Select(int.Parse).ToArray();
                regs[p] = ip;
                switch (op)
                {
                    case "seti":
                        regs[pars[2]] = pars[0];
                        break;
                    case "setr":
                        regs[pars[2]] = regs[pars[0]];
                        break;
                    case "addi":
                        regs[pars[2]] = (regs[pars[0]] + pars[1]);
                        break;
                    case "addr":
                        regs[pars[2]] = (regs[pars[0]] + regs[pars[1]]);
                        break;
                    case "muli":
                        regs[pars[2]] = regs[pars[0]] * pars[1];
                        break;
                    case "mulr":
                        regs[pars[2]] = regs[pars[0]] * regs[pars[1]];
                        break;
                    case "gtri":
                        regs[pars[2]] = regs[pars[0]] > pars[1] ? 1 : 0;
                        break;
                    case "gtrr":
                        regs[pars[2]] = regs[pars[0]] > regs[pars[1]] ? 1 : 0;
                        break;
                    case "gtir":
                        regs[pars[2]] = pars[0] > regs[pars[1]] ? 1 : 0;
                        break;
                    case "eqri":
                        regs[pars[2]] = regs[pars[0]] == pars[1] ? 1 : 0;
                        break;
                    case "eqrr":
                        regs[pars[2]] = regs[pars[0]] == regs[pars[1]] ? 1 : 0;
                        break;
                    case "eqir":
                        regs[pars[2]] = pars[0] == regs[pars[1]] ? 1 : 0;
                        break;
                    case "bori":
                        regs[pars[2]] = regs[pars[0]] | pars[1];
                        break;
                    case "borr":
                        regs[pars[2]] = regs[pars[0]] | regs[pars[1]];
                        break;
                    case "bandi":
                        regs[pars[2]] = regs[pars[0]] & pars[1];
                        break;
                    case "bandr":
                        regs[pars[2]] = regs[pars[0]] & regs[pars[1]];
                        break;
                }

                ip = regs[p];

                ip++;
                //                regs[p]++;

                //                regs.Print();
            }

        }

        public static int[] Execute(int[] regs, int[] inp, Func<int[], int[], int> func)
        {
            var res = new int[4];
            Array.Copy(regs, res, 4);
            res[inp[3]] = func(regs, inp);
            return res;
        }
    }
}