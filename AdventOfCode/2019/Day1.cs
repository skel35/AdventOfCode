using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;
using static AdventOfCode.AoC;

namespace AdventOfCode._2019;

public class Day1 : Solution
{
    public Day1() : base(1, 2019){}

    private static int RequiredFuel(int fuelMass)
    {
        var req = fuelMass / 3 - 2;
        if (req <= 0) return 0;
        return req + RequiredFuel(req);
    }

    protected override void Solve()
    {
        var sum = ReadLines().Select(int.Parse).SumLong(RequiredFuel);
        Console.WriteLine(sum);
    }
}