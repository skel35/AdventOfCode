using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Math;

namespace AdventOfCode._2020
{
    public class Day11 : Solution
    {
        public Day11() : base(11, 2020) { }
        protected override void Solve()
        {
            var grid =
                ReadLines()
                    .ToSb();


            while (true)
            {
                var noChanges = true;
                var gridC = new StringBuilder[grid.Length];
                for (var i = 0; i < gridC.Length; i++)
                {
                    gridC[i] = new StringBuilder(grid[i].ToString());
                }
                grid.Iter((r, c, ch) =>
                {
                    if (grid[r][c] == '.') return;
                    var dirs = (r, c).Gen8Adjacent().Select(p => p.Minus((r, c))).ToArray();
                    var countOccupied = dirs.Sum(dir =>
                    {
                        var p = (r, c).Plus(dir);
                        while (true)
                        {
                            if (!p.InBounds(grid)) return 0;
                            if (grid.At(p) == 'L') return 0;
                            if (grid.At(p) == '#') return 1;
                            p = p.Plus(dir);
                        }
                    });
                    // var count = (r, c).Gen8Adjacent()
                    //     .Where(p => p.InBounds(grid))
                    //     .Count(p => grid.At(p) == '#');
                    if (grid[r][c] == 'L' && countOccupied == 0)
                    {
                        gridC[r][c] = '#';
                        noChanges = false;
                    }

                    if (grid[r][c] == '#' && countOccupied >= 5)
                    {
                        gridC[r][c] = 'L';
                        noChanges = false;
                    }
                });

                grid = gridC;
                grid.Print();
                Console.WriteLine();
                if (noChanges) break;
            }

            var occupied = 0;
            grid.Iter(ch =>
            {
                if (ch == '#') occupied++;
            });
            Console.WriteLine(occupied);
            // 2081
        }
    }
}
