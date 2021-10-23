namespace AdventOfCode;

partial class Solution
{
    protected (string Op, int Arg)[] ReadProgram()
    {
        return ReadLines()
            .Select(line => line.Split())
            .Select(a => (Op: a[0], Arg: a[1].ToInt()))
            .ToArray();
    }

    protected (int Acc, int Ind) ProcessLine((string Op, int Arg) p, (int Acc, int Ind) c)
    {
        var (acc, ind) = c;
        return p switch
        {
            ("nop", _) => (acc, ind + 1),
            ("acc", { } n) => (acc + n, ind + 1),
            ("jmp", { } n) => (acc, ind + n),
            _ => (acc, ind)
        };
    }
}