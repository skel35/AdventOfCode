using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2019;

public class Day13 : Solution
{
    public Day13() : base(13, 2019) { }
    protected override void Solve()
    {
        var amp = new Amplifier(ReadText());
        var grid = new int[40, 24];
        var halted = false;
        amp.Prog[0] = 2;
        var score = 0;
        var v = (x: 0, y: 0);
        var lastPos = (x: 0, y: 0);
        // amp.Input.Enqueue(0);
        // amp.Input.Enqueue(0);
        var tilePos = (x: -1, y: -1);
        while (!halted)
        {
            var move = 0;
            if (tilePos.x == -1)
            {
                move = int.Parse(Console.ReadLine()!);
            }
            else
            {
                if (tilePos.x == lastPos.x && tilePos.y == lastPos.y + 1)
                {
                    move = 0;
                }
                else
                {
                    move = (lastPos.x + v.x) - tilePos.x;
                }
            }

            Console.WriteLine("move: " + move);
            amp.Input.Enqueue(move);
            halted = amp.Run();
            while (amp.Output.Count >= 3)
            {
                var x = (int)amp.Output.Dequeue();
                var y = (int)amp.Output.Dequeue();
                var type = (int)amp.Output.Dequeue();

                if (x == -1 && y == 0) score = type;
                else
                {
                    grid[x, y] = type;

                    if (type == 3)
                    {
                        tilePos = (x, y);
                    }
                    else if (type == 4)
                    {
                        v = (x, y).Minus(lastPos);
                        lastPos = (x, y);
                    }
                }
            }
            grid.Print(lastPos);
            lastPos.ToString().Print();
            score.ToString().Print();
        }
        // '▮'
        Console.WriteLine("score: " + score);
        // var c = 0;
        // for (var i = 0; i < grid.L1(); i++)
        // {
        //     for (var j = 0; j < grid.L2(); j++)
        //     {
        //         if (grid[i, j] == 2) c++;
        //     }
        // }
        // grid.Count(tile => tile == 2);
        // Console.WriteLine(c);
    }
}