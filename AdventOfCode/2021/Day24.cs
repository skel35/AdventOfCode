namespace AdventOfCode._2021;

public class Day24 : Solution
{
    public Day24() : base(24, 2021) { }
    protected override void Solve()
    {
        // found by manual input program translation
        bool isValid(int[] inp) =>
            inp[2] == inp[3] &&
            inp[4] + 4 == inp[5] &&
            inp[6] + 3 == inp[7] &&
            inp[9] + 8 == inp[10] &&
            inp[8] - 6 == inp[11] &&
            inp[1] - 7 == inp[12] &&
            inp[0] - 3 == inp[13];

        // found by logic from the expr above
        var largestValid = "99995969919326"; //.ToCharArray().Map(AoC.ToInt);
        largestValid.Print(); // p1
        var smallestValid = "48111514719111"; //.ToCharArray().Map(AoC.ToInt);
        smallestValid.Print(); // p2
    }
}