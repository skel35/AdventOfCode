namespace AdventOfCode._2017;

public class Day25 : Solution
{
    public Day25() : base(25, 2017) { }
    protected override void Solve()
    {
        const int n = 5000000;
        var array = new BitArray(n);
        var i = n / 2;
        var state = 0;
        const int count = 12919244;
        for (var ind = 0; ind < count; ind++)
        {
            var val = array[i];
            switch (state)
            {
                case 0:
                    if (!val)
                    {
                        array[i] = true;
                        i++;
                        state = 1;
                    }
                    else
                    {
                        array[i] = false;
                        i--;
                        state = 2;
                    }
                    break;
                case 1:
                    array[i] = true;
                    if (!val)
                    {
                        i--;
                        state = 0;
                    }
                    else
                    {
                        i++;
                        state = 3;
                    }

                    break;
                case 2:
                    if (!val)
                    {
                        array[i] = true;
                        i++;
                        state = 0;
                    }
                    else
                    {
                        array[i] = false;
                        i--;
                        state = 4;
                    }

                    break;
                case 3:
                    if (!val)
                    {
                        array[i] = true;
                        i++;
                        state = 0;
                    }
                    else
                    {
                        array[i] = false;
                        i++;
                        state = 1;
                    }

                    break;
                case 4:
                    if (!val)
                    {
                        array[i] = true;
                        i--;
                        state = 5;
                    }
                    else
                    {
                        array[i] = true;
                        i--;
                        state = 2;
                    }

                    break;
                case 5:
                    if (!val)
                    {
                        array[i] = true;
                        i++;
                        state = 3;
                    }
                    else
                    {
                        array[i] = true;
                        i++;
                        state = 0;
                    }

                    break;
            }
        }

        var ones = 0;
        for (var ind = 0; ind < n; ind++)
            if (array[ind])
                ones++;
        Console.WriteLine(ones);
    }
}