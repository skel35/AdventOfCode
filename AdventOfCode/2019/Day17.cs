using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Math;

namespace AdventOfCode._2019
{
    public class Day17 : Solution
    {
        public Day17() : base(17, 2019) { }
        protected override void Solve()
        {
            var amp = new Amplifier(ReadText());
            amp.Prog[0] = 2;
            var grid = new int[49, 42];
            var p = (R:  0, C: 0);
            amp.Run();
            while (amp.Output.Count > 0)
            {
                var o = amp.Output.Dequeue();
                if (o == 10)
                {
                    p = (p.R + 1, 0);
                    Console.WriteLine();
                }
                else
                {
                    if (!p.OutOfBounds(grid.R(), grid.C()))
                    {
                        grid.At(p) = (int)o;
                        if ((char) o == '^')
                        {
                        }
                    }
                    Console.Write((char)o);
                    p = (p.R, p.C + 1);
                }
            }

            var res =
                "A,C,C,B,A,C,B,A,C,B\nL,6,R,12,L,4,L,6\nL,6,L,10,L,10,R,6\nR,6,L,6,R,12\ny\n";
            var pathInt = res.Select(c => (long) c).ToArray();
            pathInt.ForEach(amp.Input.Enqueue);
            amp.Run();
            while (amp.Output.Count > 0)
            {
                var outp = amp.Output.Dequeue();
                if (outp > 200)
                {
                    Console.WriteLine(outp);
                }
                else
                {
                    var ch = (char) outp;
                    Console.Write(ch);
                }
            }
            // amp.Input.Enqueue();

            // bool[,] scaffold = new bool[grid.L1(), grid.L2()];
            // for (var i = 1; i < grid.L1() - 1; i++)
            // {
            //     for (var j = 1; j < grid.L2() - 1; j++)
            //     {
            //         if (grid.At((i, j - 1)) == (int) '#'
            //             && grid.At((i, j + 1)) == (int) '#'
            //             && grid.At((i + 1, j)) == (int) '#'
            //             && grid.At((i - 1, j)) == (int) '#'
            //             && grid.At((i, j)) == (int) '#')
            //         {
            //             scaffold.At((i, j)) = true;
            //         }
            //     }
            // }
            // var path = BuildPath(grid, robot, (-1, 0));
            // string BuildPath(int[,] grid, (int r, int c) robot, (int r, int c) dir)
            // {
            //     bool[,] visited = new bool[grid.L1(), grid.L2()];
            //     visited.At(robot) = true;
            //     var sb = new StringBuilder();
            //     while (true)
            //     {
            //         var left = robot.Plus(dir.Left());
            //         var right = robot.Plus(dir.Right());
            //         var front = robot.Plus(dir);
            //
            //         bool Correct((int r, int c) point)
            //         {
            //             return !point.OutOfBounds(grid.L1(), grid.L2())
            //                    && grid.At(point) == '#'
            //                    && (scaffold.At(point) || !visited.At(point));
            //         }
            //
            //         (int r, int c) GetCorrectPoint()
            //         {
            //             if (Correct(front)) return front;
            //             if (Correct(left))
            //             {
            //                 sb.Append('L');
            //                 dir = dir.Left();
            //                 return left;
            //             }
            //
            //             if (Correct(right))
            //             {
            //                 sb.Append('R');
            //                 dir = dir.Right();
            //                 return right;
            //             }
            //
            //             return (-100, -100);
            //         }
            //
            //         var correct = GetCorrectPoint();
            //         if (correct == (-100, -100))
            //         {
            //             sb.Remove(sb.Length - 1, 1);
            //             return sb.ToString();
            //         }
            //         var count = 0;
            //         while (Correct(correct))
            //         {
            //             count++;
            //             visited.At(correct) = true;
            //             correct = correct.Plus(dir);
            //         }
            //
            //         robot = correct.Minus(dir);
            //
            //         sb.Append(count);
            //         sb.Append(',');
            //     }
            // }
            //
            // Console.WriteLine(path);
            // var sum = 0;
            // for (var i = 1; i < grid.L1() - 1; i++)
            // {
            //     for (var j = 1; j < grid.L2() - 1; j++)
            //     {
            //         if (grid.At((i, j - 1)) == (int) '#'
            //             && grid.At((i, j + 1)) == (int) '#'
            //             && grid.At((i + 1, j)) == (int) '#'
            //             && grid.At((i - 1, j)) == (int) '#'
            //             && grid.At((i, j)) == (int) '#')
            //         {
            //             sum += i * j;
            //         }
            //     }
            // }

            // Console.WriteLine(sum);
        }
    }
}
