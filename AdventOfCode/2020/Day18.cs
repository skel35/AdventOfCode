using AdventOfCode._2020.D18;

namespace AdventOfCode._2020
{
    public class Day18 : Solution
    {
        public Day18() : base(18, 2020) { }
        protected override void Solve()
        {
            var input =
                ReadLines()
                    .ArrayMap(line => line.Without(' ').Select(c => c.ToString()).ToArray());

            long Calculate(string[] tokens)
            {
                bool Process(Stack<string> stack1, string t, string op)
                {
                    if (stack1.Peek() == op)
                    {
                        stack1.Push(stack1.Pop().ApplyOp(stack1.Pop(), t));
                        return true;
                    }
                    else
                    {
                        stack1.Push(t);
                        return false;
                    }
                }

                var stack = new Stack<string>();
                stack.Push(" ");
                foreach (var t in tokens)
                {
                    switch (t)
                    {
                        case {} when t.IsNumber():
                            Process(stack, t, "+");
                            // stack.Push(t);
                            break;
                        case {} when t.IsOp():
                            stack.Push(t);
                            break;
                        case {} when t == "(":
                            stack.Push(t);
                            break;
                        case {} when t == ")":
                            while (true)
                            {
                                if (!Process(stack, stack.Pop(), "*"))
                                    break;
                            }
                            var temp = stack.Pop();
                            (stack.Pop() == "(").Assert();
                            Process(stack, temp, "+");
                            break;
                    }
                }

                while (true)
                {
                    if (!Process(stack, stack.Pop(), "*"))
                        break;
                }
                var calculated = stack.Pop();
                calculated.IsNumber().Assert();
                return calculated.ToLong();
            }

            var res = input.Select(Calculate).Sum();
            Console.WriteLine(res);
        }
    }

    namespace D18
    {
        public static class Ex
        {
            public static bool IsOp(this char c) => c == '*' || c == '+';
            public static bool IsOp(this string c) => c[0].IsOp();
            public static bool IsDigit(this char c) => char.IsDigit(c);
            public static bool IsNumber(this string c) => c[0].IsDigit();

            public static string ApplyOp(this string c, string arg1, string arg2) =>
                (c[0] switch
                {
                    '*' => arg1.ToLong() * arg2.ToLong(),
                    '+' => arg1.ToLong() + arg2.ToLong(),
                    _ => throw new InvalidOperationException()
                }).ToString();
        }
    }
}
