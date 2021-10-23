using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2019;

public class Day9 : Solution
{
    public Day9() : base(9, 2019) { }
    protected override void Solve()
    {
        var n = ReadText().ParseLongs();
        var SIZE = 50000;
        var array = new long[SIZE];
        for (var j = 0; j < n.Length; j++) array[j] = n[j];
        var inp = new Queue<long>();
        inp.Enqueue(2);
        var outp = new Queue<long>();
        long pointer = 0;
        long offset = 0;
        Amplifier(array, inp, outp, ref pointer, ref offset);
        outp.Print();
    }

    /// <returns>True if application halted, otherwise false.</returns>
    void Amplifier(long[] n, Queue<long> input, Queue<long> output, ref long pointer, ref long offset)
    {
        while (pointer < n.Length)
        {
            var opcode = n[pointer] % 100;
            if (opcode == 99)
            {
                return;
                // halt
            }

            var modes = (n[pointer] / 100).ToString("000");

            ref long GetParam(int pos, long i, long offset)
            {
                switch (modes[3 - pos])
                {
                    case '0':
                        return ref n[n[i + pos]];
                    case '1':
                        return ref n[i + pos];
                    case '2':
                        return ref n[n[i + pos] + offset];
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            switch (opcode)
            {
                case 1:
                {
                    GetParam(3, pointer, offset) = GetParam(1, pointer, offset) + GetParam(2, pointer, offset);
                    pointer += 4;
                    break;
                }
                case 2:
                {
                    GetParam(3, pointer, offset) = GetParam(1, pointer, offset) * GetParam(2, pointer, offset);
                    pointer += 4;
                    break;
                }
                case 3:
                    if (input.Count > 0)
                    {
                        GetParam(1, pointer, offset) = input.Dequeue();
                        pointer += 2;
                    }
                    else
                    {
                        return;
                    }
                    break;
                case 4:
                {
                    output.Enqueue(GetParam(1, pointer, offset));
                    pointer += 2;
                    break;
                }
                case 5:
                {
                    if (GetParam(1, pointer, offset) != 0)
                    {
                        pointer = GetParam(2, pointer, offset);
                    }
                    else
                    {
                        pointer += 3;
                    }
                    break;
                }
                case 6:
                {
                    if (GetParam(1, pointer, offset) == 0)
                    {
                        pointer = GetParam(2, pointer, offset);
                    }
                    else
                    {
                        pointer += 3;
                    }

                    break;
                }
                case 7:
                {
                    GetParam(3, pointer, offset) = GetParam(1, pointer, offset) < GetParam(2, pointer, offset) ? 1 : 0;
                    pointer += 4;
                    break;
                }
                case 8:
                {
                    GetParam(3, pointer, offset) = GetParam(1, pointer, offset) == GetParam(2, pointer, offset) ? 1 : 0;
                    pointer += 4;
                    break;
                }
                case 9:
                {
                    offset += GetParam(1, pointer, offset);
                    pointer += 2;
                    break;
                }

            }
        }

        throw new InvalidOperationException("Impossibru");
    }

}