namespace AdventOfCode._2019;

public class Amplifier
{
    public static int ProgramSize = 10000;
    public long Offset;
    public long Ptr;

    public Amplifier(string program)
    {
        Prog = program.ParseLongs(ProgramSize);
        Input = new Queue<long>();
        Output = new Queue<long>();
        Ptr = 0;
        Offset = 0;
    }

    public Queue<long> Input { get; }
    public Queue<long> Output { get; }
    public long[] Prog { get; }
}

public static class IntCodeExtensions
{
    public static void Input(this Amplifier amp, string command)
    {
        command.ForEach(ch =>  amp.Input.Enqueue(ch));
        amp.Input.Enqueue('\n');
    }

    // public static void Run(this Amplifier amp) => Amplifier.Run(amp);
    /// <returns>True if application halted, otherwise false.</returns>
    public static bool Run(this Amplifier amp)
    {
        while (amp.Ptr < amp.Prog.Length)
        {
            var opcode = amp.Prog[amp.Ptr] % 100;
            if (opcode == 99)
                return true;
            // halt

            var modes = (amp.Prog[amp.Ptr] / 100).ToString("000");

            ref long GetParam(int pos, Amplifier amp)
            {
                switch (modes[3 - pos])
                {
                    case '0':
                        return ref amp.Prog[amp.Prog[amp.Ptr + pos]];
                    case '1':
                        return ref amp.Prog[amp.Ptr + pos];
                    case '2':
                        return ref amp.Prog[amp.Prog[amp.Ptr + pos] + amp.Offset];
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            switch (opcode)
            {
                case 1:
                {
                    GetParam(3, amp) = GetParam(1, amp) + GetParam(2, amp);
                    amp.Ptr += 4;
                    break;
                }
                case 2:
                {
                    GetParam(3, amp) = GetParam(1, amp) * GetParam(2, amp);
                    amp.Ptr += 4;
                    break;
                }
                case 3:
                    if (amp.Input.Count > 0)
                    {
                        GetParam(1, amp) = amp.Input.Dequeue();
                        amp.Ptr += 2;
                    }
                    else
                    {
                        return false;
                    }

                    break;
                case 4:
                {
                    amp.Output.Enqueue(GetParam(1, amp));
                    amp.Ptr += 2;
                    break;
                }
                case 5:
                {
                    if (GetParam(1, amp) != 0)
                        amp.Ptr = GetParam(2, amp);
                    else
                        amp.Ptr += 3;
                    break;
                }
                case 6:
                {
                    if (GetParam(1, amp) == 0)
                        amp.Ptr = GetParam(2, amp);
                    else
                        amp.Ptr += 3;

                    break;
                }
                case 7:
                {
                    GetParam(3, amp) = GetParam(1, amp) < GetParam(2, amp) ? 1 : 0;
                    amp.Ptr += 4;
                    break;
                }
                case 8:
                {
                    GetParam(3, amp) = GetParam(1, amp) == GetParam(2, amp) ? 1 : 0;
                    amp.Ptr += 4;
                    break;
                }
                case 9:
                {
                    amp.Offset += GetParam(1, amp);
                    amp.Ptr += 2;
                    break;
                }
            }
        }

        throw new InvalidOperationException("Impossibru");
    }


    public static string GetAsciiOutputIfInRange(this Amplifier amp)
    {
        var outp = amp.Output.Dequeue();
        if (outp < 255)
        {
            var ch = (char) outp;
            return ch.ToString();
        }
        else
        {
            return outp.ToString() + "\n";
        }
    }
}