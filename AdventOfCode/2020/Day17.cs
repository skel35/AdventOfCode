using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Math;

namespace AdventOfCode._2020
{
    public class Day17 : Solution
    {
        public Day17() : base(17, 2020) { }
        protected override void Solve()
        {
            var input =
                ReadLines()
                    .Select(s => s.ToCharArray())
                    .ToArray();

            var C = 6;
            var size = input.Length + C*2;
            var sizeZ = 1 + C * 2;
            var grid = new char[size, size, sizeZ];
            for (var i = 0; i < input.Length; i++)
            {
                for (var j = 0; j < input[i].Length; j++)
                {
                    grid[i + C, j + C, C] = input[i][j];
                }
            }

            for (var c = 0; c < C; c++)
            {
                var gridPrev = new char[size, size, sizeZ];
                Buffer.BlockCopy(grid, 0, gridPrev, 0, grid.Length * sizeof(char));
                for (var i = 0; i < size; i++)
                {
                    for (var j = 0; j < size; j++)
                    {
                        for (var z = 0; z < sizeZ; z++)
                        {
                            var cur = (i, j, z);
                            var count = cur
                                .Gen26Adjacent()
                                .Where(x => !x.OutOfBounds(size, size, sizeZ))
                                .Count(p => gridPrev.At(p) == '#');
                            var isCube = gridPrev.At(cur) == '#';
                            grid.At(cur) =
                                ((isCube && count == 3)
                                 ||
                                 (isCube && count == 2)
                                 ||
                                 (!isCube && count == 3))
                                    ? '#'
                                    : '.';
                        }
                    }
                }
            }

            var res = grid.Flatten().Count(c => c == '#');
            Console.WriteLine(res);
        }
    }
}
