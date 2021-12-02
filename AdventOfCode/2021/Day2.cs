using RegExtract;

namespace AdventOfCode._2021;

public class Day2 : Solution
{
    public Day2() : base(2, 2021) { }
    protected override void Solve()
    {
        var res = ReadLines()
            .Select(s => s.Extract<(string dir, int val)>(@"(.*) (\d+)"))
            .Aggregate(
                (hor: 0, depth: 0, aim: 0),
                (pos, instr) => instr.dir switch
                {
                    "forward" => (pos.hor + instr.val, pos.depth + pos.aim * instr.val, pos.aim),
                    "down" => (pos.hor, pos.depth, pos.aim + instr.val),
                    "up" => (pos.hor, pos.depth, pos.aim - instr.val),
                    _ => pos
                });
        (res.hor * res.depth).ToString().Print();

    }
}