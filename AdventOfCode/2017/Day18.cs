using System;
using System.Collections.Generic;

namespace AdventOfCode._2017;

public class Day18 : Solution
{
    public Day18() : base(18, 2017)
    {
    }

    protected override void Solve()
    {
        var lines = ReadLines();

        var index = new int[2];
        var waiting = new bool[2];
        var regs = new Dictionary<char, long>[2];
        regs[0] = new Dictionary<char, long>();
        regs[1] = new Dictionary<char, long>();
        regs[1]['p'] = 1L;

        long GetV(int i, char c)
        {
            return !regs[i].ContainsKey(c) ? 0 : regs[i][c];
        }

        var sent = new Queue<long>[2];
        sent[0] = new Queue<long>();
        sent[1] = new Queue<long>();
        var sentCount = new int[2];
        while (true)
        {
            for (var i = 0; i < 2; i++)
            {
                if (!waiting[i]) continue;
                if (sent[(i + 1) % 2].Count > 0) waiting[i] = false;
            }

            if (waiting[0] && waiting[1])
            {
                Console.WriteLine(sentCount[0]);
                return;
            }

            for (var i = 0; i < 2; i++)
            {
                if (waiting[i]) continue;
                var line = lines[index[i]];
                var s = line.Split();
                var x = s[1][0];
                var xv = char.IsDigit(x) ? long.Parse(s[1]) : GetV(i, x);
                var y = -1L;
                if (s.Length > 2)
                {
                    if (char.IsDigit(s[2][0]) || s[2][0] == '-')
                        y = long.Parse(s[2]);
                    else
                        y = GetV(i, s[2][0]);
                }

                switch (s[0])
                {
                    case "snd":
                        sent[i].Enqueue(xv);
                        sentCount[i]++;
                        break;
                    case "set":
                        regs[i][x] = y;
                        break;
                    case "add":
                        regs[i][x] = xv + y;
                        break;
                    case "mul":
                        regs[i][x] = xv * y;
                        break;
                    case "mod":
                        regs[i][x] = xv % y;
                        break;
                    case "rcv":
                        var receiveFrom = sent[(i + 1) % 2];
                        if (receiveFrom.Count == 0)
                            waiting[i] = true;
                        else
                            regs[i][x] = receiveFrom.Dequeue();
                        break;
                    case "jgz":
                        if (xv > 0) index[i] += (int) y - 1;
                        break;
                }

                if (!waiting[i]) index[i]++;
            }
        }
    }
}