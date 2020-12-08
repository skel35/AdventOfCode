using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2017
{
    public class Day23 : Solution
    {
        public Day23() : base(23, 2017) { }
        protected override void Solve()
        {
            // var lines = ReadLines().Select(line =>
            // {
            //     var split =  line.Split();
            //     var isReg = false;
            //     var isReg2 = false;
            //     if (!int.TryParse(split[1], out int p1))
            //     {
            //         p1 = split[1][0] - 'a';
            //         isReg2 = true;
            //     }
            //
            //     if (!int.TryParse(split[2], out int p2))
            //     {
            //         p2 = split[2][0] - 'a';
            //         isReg = true;
            //     }
            //
            //     return (split[0], p1, p2, isReg, isReg2);
            // }).ToArray();
            //
            // var regs = new int['h' - 'a' + 1];
            // regs[0] = 1;
            // var ind = 0;
            // var mulInvoked = 0;
            // while (ind < lines.Length)
            // {
            //     var cmd = lines[ind];
            //     switch (cmd.Item1)
            //     {
            //         case "set":
            //             regs[cmd.p1] = cmd.isReg ? regs[cmd.p2] : cmd.p2;
            //             break;
            //         case "sub":
            //             regs[cmd.p1] -= cmd.isReg ? regs[cmd.p2] : cmd.p2;
            //             break;
            //         case "mul":
            //             regs[cmd.p1] *= cmd.isReg ? regs[cmd.p2] : cmd.p2;
            //             mulInvoked++;
            //             break;
            //         case "jnz":
            //             var x = cmd.isReg2 ? regs[cmd.p1] : cmd.p1;
            //             if (x != 0)
            //             {
            //                 var y = cmd.isReg ? regs[cmd.p2] : cmd.p2;
            //                 ind += (y - 1);
            //             }
            //
            //             break;
            //     }
            //     
            //     ind++;
            // }
            //
            // Console.WriteLine(regs['h'-'a']);
            var nonPrimes = 0;
            for (var b = 109300; b <= 126300; b += 17)
            {
                var isPrime = true;
                for (var x = 2; x * x <= b; x++)
                {
                    if (b % x == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }

                if (!isPrime) nonPrimes++;
            }

            Console.WriteLine(nonPrimes);
        }
    }
}