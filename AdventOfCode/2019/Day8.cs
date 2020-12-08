using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2019
{
    public class Day8 : Solution
    {
        public Day8() : base(8, 2019) { }
        protected override void Solve()
        {
            var W = 25;
            var H = 6;
            var layers = ReadText().Select(c => c.ToInt()).Window(W*H).ToArray();
            // var minLayer = layers.MinBy(layer => layer.Count(x => x == 0));
            // var res = minLayer.Count(x => x == 1) * minLayer.Count(x => x == 2);
            // Console.WriteLine(res);

            for (var h = 0; h < H; h++)
            {
                for (var j = 0; j < W; j++)
                {
                    var px = 1; // white default
                    for (var t = 0; t < layers.Length; t++)
                    {
                        if (layers[t][h*W + j] == 2)
                        {
                        }
                        else
                        {
                            px = layers[t][h*W+ j];
                            break;
                        }
                    }

                    Console.Write(px == 0 ? '.' : '1');
                }

                Console.WriteLine();
            }
        }
    }
}
