using System.Text;

namespace AdventOfCode._2019;

public class Day19 : Solution
{
    public Day19() : base(19, 2019) { }
    protected override void Solve()
    {
        var str = ReadText();
        var count = 0;
        // part 1:
        50.IterSqr((x, y) =>
        {
            count = (int) (count + Run(str, x, y));
        });
        Console.WriteLine(count);

        // part 2:
        var s = new StringBuilder();
        bool hadBeams = false;
        int firstBeam = 0;
        var prevLength = 0;
        var l = new int[2000];
        var fb = new int[2000];
        for (var x = 0; x < 2000; x++)
        {
            hadBeams = false;
            for (var y = firstBeam; y < 2000; y++)
            {
                if (Run(str, x, y) == 1)
                {
                    s.Append('#');
                    if (!hadBeams)
                    {
                        firstBeam = y;
                        fb[x] = firstBeam;
                        hadBeams = true;
                        y += prevLength;

                        if (x > 100)
                        {
                            if (l[x - 99] + fb[x - 99] - 100 >= fb[x])
                            {
                                Console.WriteLine((x - 99) * 10000 + (fb[x]));
                                return;
                            }
                        }
                    }

                }
                else
                {
                    s.Append('.');
                    if (hadBeams)
                    {
                        prevLength = y - firstBeam;
                        l[x] = prevLength;
                        break;
                    }
                }
            }

            s.Append("\n");
        }
    }

    private static long Run(string str, int x, int y)
    {
        var amp = new Amplifier(str);
        amp.Input.Enqueue(x);
        amp.Input.Enqueue(y);
        amp.Run();
        var outp = amp.Output.Dequeue();
        return outp;
    }
}