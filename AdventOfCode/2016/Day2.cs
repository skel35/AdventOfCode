namespace AdventOfCode._2016;

public class Day2 : Solution
{
    public Day2() : base(2, 2016) { }
    protected override void Solve()
    {
        var nums = new[,]
        {{'-', '-', '1', '-', '-'},
            {'-', '2', '3', '4', '-'},
            {'5', '6', '7', '8', '9'},
            {'-', 'A', 'B', 'C', '-'},
            {'-', '-', 'D', '-', '-'}};
        var p = (R:  2, C: 2);
        ReadLines().ForEach(line =>
        {
            foreach (var c in line)
            {
                switch (c)
                {
                    case 'U':
                        break;
                }

                var prevP = p;
                p = c switch
                {
                    'U' => ((p.R - 1), p.C),
                    'R' => (p.R, p.C + 1),
                    'D' => (p.R + 1, p.C),
                    'L' => (p.R, (p.C - 1)),
                    _ => throw new InvalidOperationException()
                };
                var isValid = p.R.IsBetweenNonStrict(0, 4) && p.C.IsBetweenNonStrict(0, 4)
                                                           // && (p.Manhattan((2, 2)) <= 2);
                                                           && nums[p.R, p.C] != '-';
                if (!isValid) p = prevP;
            }
            Console.Write(nums[p.R, p.C]);
        });
        Console.WriteLine();
    }
}