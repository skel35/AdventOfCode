$year = 2021
md -Force "$PSScriptRoot/$year/"
foreach ($day in 1..25) {
  $code = @"
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._$year
{
    public class Day$day : Solution
    {
        public Day$day() : base($day, $year) { }
        protected override void Solve()
        { 
        }
    }
}
"@

  Set-Content -Path "$PSScriptRoot/$year/Day$day.cs" $code -Force
  Start-Sleep -Milliseconds 10
}
