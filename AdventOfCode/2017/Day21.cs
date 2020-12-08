using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2017
{
    public class Day21 : Solution
    {
        public Day21() : base(21, 2017) { }
        protected override void Solve()
        {
            var lines = ReadLines();
            var twoBitPatterns = new Dictionary<int, bool[,]>();
            var threeBitPatterns = new Dictionary<int, bool[,]>();

            foreach (var line in lines)
            {
                if (line.Length <= 20)
                {
                    var n2 = line[9..].Split('/').Select(ll => ll.Select(ch => ch == '#').ToArray()).ToArray().To2D();
                    var miniGrid = line[..5].Split('/').Select(ll => ll.Select(ch => ch == '#').ToArray()).ToArray().To2D();
                    var n = MakeInt(miniGrid, 0, 0, 2);
                    twoBitPatterns.Add(n, n2);
                    
                    var flipped = Flip(miniGrid, 2);
                    n = MakeInt(flipped, 0, 0, 2);
                    twoBitPatterns[n] = n2;
                    
                    var rotated = miniGrid;
                    for (var i = 0; i < 3; i++)
                    {
                        rotated = Rotate(rotated, 2);
                        n = MakeInt(rotated, 0, 0, 2);
                        twoBitPatterns[n] = n2;
                        flipped = Flip(rotated, 2);
                        n = MakeInt(flipped, 0, 0, 2);
                        twoBitPatterns[n] = n2;                        
                    }
                }
                else
                {
                    var miniGrid = line[..11].Split('/').Select(ll => ll.Select(ch => ch == '#').ToArray()).ToArray().To2D();
                    var n = MakeInt(miniGrid, 0, 0, 3);
                    var n2 = line[15..].Split('/').Select(ll => ll.Select(ch => ch == '#').ToArray()).ToArray().To2D();
                    threeBitPatterns.Add(n, n2);
                    
                    var flipped = Flip(miniGrid, 3);
                    n = MakeInt(flipped, 0, 0, 3);
                    threeBitPatterns[n] = n2;
                    
                    var rotated = miniGrid;
                    for (var i = 0; i < 3; i++)
                    {
                        rotated = Rotate(rotated, 3);
                        n = MakeInt(rotated, 0, 0, 3);
                        threeBitPatterns[n] = n2;
                        flipped = Flip(rotated, 3);
                        n = MakeInt(flipped, 0, 0, 3);
                        threeBitPatterns[n] = n2;
                    }
                }
            }
            
            var grid = new[,]
            {
                {false, true, false},
                {false, false, true},
                {true, true, true}
            };

            bool[,] Rotate(bool[,] g, int size)
            {
                var g2 = new bool[size, size];
                for (var r = 0; r < size; r++)
                {
                    for (var c = 0; c < size; c++)
                    {
                        g2[c, size - 1 - r] = g[r, c];
                    }
                }
                return g2;
            }

            bool[,] Flip(bool[,] g, int size)
            {
                var g2 = new bool[size, size];
                for (var r = 0; r < size; r++)
                {
                    for (var c = 0; c < size; c++)
                    {
                        g2[r, size-1-c] = g[r, c];
                    }
                }
                return g2;
            }
            int MakeInt(bool[,] g, int row, int col, int size)
            {
                var resInt = 0;
                for (var r = row; r < row + size; r++)
                {
                    for (var c = col; c < col + size; c++)
                    {
                        resInt *= 2;
                        resInt += g[r, c] ? 1 : 0;
                    }
                }

                return resInt;
            }

            void WriteOnGrid(bool[,] g, int row, int col, int size, bool[,] outp)
            {
                for (var r = row + size - 1; r >= row; r--)
                {
                    for (var c = col + size - 1; c >= col; c--)
                    {
                        g[r, c] = outp[r - row, c - col];
                    }
                }
            }

            for (var iteration = 0; iteration < 18; iteration++)
            {
                var l = grid.R();
                if (l % 2 == 0)
                {
                    var newGrid = new bool[(l / 2) * 3, (l / 2) * 3];
                    for (var i = 0; i < l; i += 2)
                    {
                        for (var j = 0; j < l; j += 2)
                        {
                            var inp = MakeInt(grid, i, j, 2);
                            var outp = twoBitPatterns[inp];
                            WriteOnGrid(newGrid, (i/2)*3, (j/2)*3, 3, outp);
                        }
                    }

                    grid = newGrid;
                }
                else
                {
                    var newGrid = new bool[(l / 3) * 4, (l / 3) * 4];
                    for (var i = 0; i < l; i += 3)
                    {
                        for (var j = 0; j < l; j += 3)
                        {
                            var inp = MakeInt(grid, i, j, 3);
                            var outp = threeBitPatterns[inp];
                            WriteOnGrid(newGrid, (i/3)*4, (j/3)*4, 4, outp);
                        }
                    }

                    grid = newGrid;
                }
            }

            var countPx = 0;
            grid.Iter((i, j) =>
            {
                if (grid[i, j]) countPx++;
            });

            Console.WriteLine(countPx);
        }
    }
}