using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2016
{
    public class Day8 : Solution
    {
        public Day8() : base(8, 2016) { }
        protected override void Solve()
        {
            var C = 50;
            // var C = 7;
            // var R = 3;
            var R = 6;
            var grid = new bool[R, C];
            ReadLines().ForEach(line =>
            {
                var s = line.Split();
                if (s[0] == "rect")
                {
                    var d = s[1].Split('x');
                    var c = d[0].ToInt();
                    var r = d[1].ToInt();
                    for (var i = 0; i < c; i++)
                    {
                        for (var j = 0; j < r; j++)
                        {
                            grid[j, i] = true;
                        }
                    }
                }
                else
                {
                    var offset = s[4].ToInt();

                    if (s[1] == "row")
                    {
                        var r = s[2][2..].ToInt();
                        var row = new bool[C];
                        for (var i = 0; i < C; i++)
                        {
                            row[i] = grid[r, i];
                        }

                        for (var i = 0; i < C; i++)
                        {
                            grid[r, (i + offset) % C] = row[i];
                        }
                    }
                    else
                    {
                        var c = s[2][2..].ToInt();
                        var col = new bool[R];
                        for (var i = 0; i < R; i++)
                        {
                            col[i] = grid[i, c];
                        }
                        for (var i = 0; i < R; i++)
                        {
                            grid[(i + offset) % R, c] = col[i];
                        }
                    }
                }
            });
            var count = 0;
            grid.Map(x => x ? '#' : '.').Print();
            grid.Iter(b =>
            {
                if (b) count++;
            });
            Console.WriteLine(count);
        }
    }
}
