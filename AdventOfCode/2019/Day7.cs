using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2019
{
    public class Day7 : Solution
    {
        public Day7() : base(7, 2019) { }
        protected override void Solve()
        {
            // TODO: OOP to make code nicer
            // TODO: Properly generate permutations
            var n = ReadText().ParseInts();
            var state = new int[5][];
            var inputStreams = new Queue<int>[5];
            var maxOutput = int.MinValue;
            for (var ps = 56789; ps <= 98765; ps++)
            {
                var pss = ps.ToString("00000");
                if (pss.Any(c => c < '5')) continue;
                if (pss.Distinct().Count() < 5) continue;
                var psi = pss.Select(c => c.ToInt()).ToArray();
                for (var j = 0; j < 5; j++)
                {
                    inputStreams[j] = new Queue<int>();
                    inputStreams[j].Enqueue(psi[j]);
                }

                inputStreams[0].Enqueue(0);

                var pointer = new int[5];
                for (var j = 0; j < 5; j++)
                {
                    state[j] = n.DeepCopy();
                }

                var amp = 0;
                while (true)
                // for (var amp = 0; amp < 5; amp++)
                {
                    var res = Amplifier(inputStreams[amp], state[amp], ref pointer[amp], inputStreams[(amp + 1) % 5]);

                    if (res && amp == 4)
                    {
                        var output = inputStreams[0].Dequeue();
                        if (output > maxOutput) maxOutput = output;
                        break;
                    }

                    amp = (amp + 1) % 5;
                }
            }

            Console.WriteLine(maxOutput);
        }

        /// <returns>True if application halted, otherwise false.</returns>
        bool Amplifier(Queue<int> input, int[] n, ref int i, Queue<int> output)
        {
            while (i < n.Length)
            {
                var opcode = n[i] % 100;
                if (opcode == 99)
                {
                    return true;
                    // halt
                }

                var modes = (n[i] / 100).ToString("000");

                int GetParam(int pos, int i)
                {
                    return modes[3 - pos] == '0' ? n[n[i + pos]] : n[i + pos];
                }

                switch (opcode)
                {
                    case 1:
                    {
                        n[n[i + 3]] = GetParam(1, i) + GetParam(2, i);
                        i += 4;
                        break;
                    }
                    case 2:
                    {
                        n[n[i + 3]] = GetParam(1, i) * GetParam(2, i);
                        i += 4;
                        break;
                    }
                    case 3:
                        if (input.Count > 0)
                        {
                            n[n[i + 1]] = input.Dequeue();
                            i += 2;
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case 4:
                    {
                        output.Enqueue(GetParam(1, i));
                        i += 2;
                        break;
                    }
                    case 5:
                    {
                        if (GetParam(1, i) != 0)
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
                        if (GetParam(1, i) == 0)
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
                        n[n[i + 3]] = GetParam(1, i) < GetParam(2, i) ? 1 : 0;
                        i += 4;
                        break;
                    }
                    case 8:
                    {
                        n[n[i + 3]] = GetParam(1, i) == GetParam(2, i) ? 1 : 0;
                        i += 4;
                        break;
                    }
                }
            }

            throw new InvalidOperationException("Impossibru");
        }
    }
}
