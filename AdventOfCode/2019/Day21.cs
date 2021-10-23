using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2019;

public class Day21 : Solution
{
    public Day21() : base(21, 2019) { }
    protected override void Solve()
    {
        var amp = new Amplifier(ReadText());
        // amp.Input.Enqueue(1);
        amp.Input("NOT A J");
        amp.Input("NOT B T");
        amp.Input("OR T J");
        amp.Input("NOT C T");
        amp.Input("OR T J");

        amp.Input("AND D J");

        amp.Input("NOT E T");
        amp.Input("NOT T T");
        amp.Input("OR H T");

        amp.Input("AND T J");

        amp.Input("RUN");
        amp.Run();
        while (amp.Output.Count > 0)
        {
            var outp = amp.GetAsciiOutputIfInRange();
            Console.Write(outp);
        }
    }
}