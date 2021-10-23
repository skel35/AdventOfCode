using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2017;

public class Day19 : Solution
{
    public Day19() : base(19, 2017) { }
    protected override void Solve()
    {
        var lines = ReadLines();
        var startC = lines[0].IndexOf('|');
        var start = (R:  0, C: startC);
        var current = start;
        var prev = (R:  -1, C: startC);
        var steps = 0;
        do
        {
            var dir = (R:  current.R - prev.R, C: current.C - prev.C);
            var oldPrev = prev;
            prev = current;
            var ch = lines.At(current);
            switch (ch)
            {
                case '|':
                case '-':
                    current = current.Plus(dir);
                    break;
                case '+':
                    var next = current.Gen4Adjacent().Single(p => p != oldPrev && lines.At(p) != ' ');
                    current = next;
                    break;
                default:

                    if (char.IsLetter(ch))
                    {
                        lines.At(current);
                        current = current.Plus(dir);
                    }
                    else
                    {
                        Console.WriteLine(steps);
                        return;
                    }
                    break;
            }

            steps++;
        } while (current != prev);
    }
}