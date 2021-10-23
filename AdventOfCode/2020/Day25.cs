namespace AdventOfCode._2020;

public class Day25 : Solution
{
    public Day25() : base(25, 2020) { }
    protected override void Solve()
    {
        var input =
            ReadLines()
                .Select(l => int.Parse(l))
                .ToArray();
        // 7, cards's loop size -> input[0]
        // 7, door's loop size -> input[1]
        // input[1], card's loop size -> enc_key
        // input[0], door's loop size -> enc_key

        var cardLoopSize = 13207740;
        var subjectNumber = input[1];
        // var subjectNumber = 7;
        var current = 1L;
        for (var i = 0; i < cardLoopSize; i++)
        {
            current *= subjectNumber;
            current = current % 20201227;

            // if (current == input[0])
            // {
            //     Console.WriteLine("card loop size = " + (i + 1));
            // }
        }
        Console.WriteLine("enc_key = " + current);
            
        var doorLoopSize = 8229037;
        subjectNumber = input[0];
        // subjectNumber = 7;
        current = 1L;
        var dict2 = new Dictionary<long, int>();
        for (var i = 0L; i < doorLoopSize; i++)
        {
            current *= subjectNumber;
            current = current % 20201227;
            // if (current == input[1])
            // {
            //     Console.WriteLine("device loop size = " + (i + 1));
            //     break;
            // }
        }

        Console.WriteLine("enc_key = " + current);
    }
}