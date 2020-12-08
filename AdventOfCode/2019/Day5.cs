using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2019
{
    public class Day5 : Solution
    {
        public Day5() : base(5, 2019) { }
        protected override void Solve()
        {
            var n = ReadText().ParseInts();
            var input = 5;
            var outputLines = new List<string>();
            for (var i = 0; i < n.Length;)
            {
                var opcode = n[i] % 100;
                if (opcode == 99)
                {
                    Console.WriteLine(outputLines.Last());
                    return;
                    // halt
                }

                var modes = (n[i] / 100).ToString("000");

                int GetParam(int pos)
                {
                    return modes[3 - pos] == '0' ? n[n[i + pos]] : n[i + pos];
                }

                switch (opcode)
                {
                    case 1:
                    {
                        n[n[i + 3]] = GetParam(1) + GetParam(2);
                        i += 4;
                        break;
                    }
                    case 2:
                    {
                        n[n[i + 3]] = GetParam(1) * GetParam(2);
                        i += 4;
                        break;
                    }
                    case 3:
                        n[n[i + 1]] = input;
                        i += 2;
                        break;
                    case 4:
                    {
                        outputLines.Add(GetParam(1).ToString());
                        i += 2;
                        break;
                    }
                    case 5:
                    {
                        if (GetParam(1) != 0)
                        {
                            var p2 = modes[1] == '0' ? n[n[i + 2]] : n[i + 2];
                            i = p2;
                        }
                        else
                        {
                            i += 3;
                        }
                        break;
                    }
                    case 6:
                    {
                        if (GetParam(1) == 0)
                        {
                            var p2 = modes[1] == '0' ? n[n[i + 2]] : n[i + 2];
                            i = p2;
                        }
                        else
                        {
                            i += 3;
                        }

                        break;
                    }
                    case 7:
                    {
                        n[n[i + 3]] = GetParam(1) < GetParam(2) ? 1 : 0;
                        i += 4;
                        break;
                    }
                    case 8:
                    {
                        n[n[i + 3]] = GetParam(1) == GetParam(2) ? 1 : 0;
                        i += 4;
                        break;
                    }
                }
            }
        }
    }
}
