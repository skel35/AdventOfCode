using System.Diagnostics;

namespace AdventOfCode._2021;

public class Day18 : Solution
{
    public Day18() : base(18, 2021) { }
    protected override void Solve()
    {
        var inputString = ReadLines();
        // var input = ReadLines().Select(s => Parse(s, 0, out _));
        // var p1 = input.Aggregate((n1, n2) => n1.Add(n2));
        // p1.Magnitude.ToString().Print();

        var p2 = inputString.Pairs()
            .SelectMany(x => new[] { Parse(x.first).Add(Parse(x.second)), Parse(x.second).Add(Parse(x.first)) })
            .Select(x => x.Magnitude)
            .Max();
        p2.ToString().Print();
    }

    SnailFishNum Parse(string str) => Parse(str, 0, out _);
    SnailFishNum Parse(string str, int ind, out int length)
    {
        if (char.IsNumber(str[ind]))
        {
            length = 1;
            return new Number(int.Parse(str[ind].ToString()));
        }

        (str[ind] == '[').Assert();
        var left = Parse(str, ind + 1, out var leftLength);
        (str[ind + 1 + leftLength] == ',').Assert();
        var right = Parse(str, ind + 1 + leftLength + 1, out var rightLength);
        (str[ind + 1 + leftLength + 1 + rightLength] == ']').Assert();
        length = 1 + leftLength + 1 + rightLength + 1;
        return Pair.Create(left, right);
    }

    abstract class SnailFishNum
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract long Magnitude { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Pair? Parent { get; set; }
        public Number FindRightMostLeaf() => this switch
        {
            Number number => number,
            Pair pair => pair.Right.FindRightMostLeaf(),
            _ => throw new InvalidOperationException()
        };

        public Number FindLeftMostLeaf() => this switch
        {
            Number number => number,
            Pair pair => pair.Left.FindLeftMostLeaf(),
            _ => throw new InvalidOperationException()
        };
        public Pair Add(SnailFishNum toAdd)
        {
            var pair = Pair.Create(this, toAdd);
            pair.Reduce();
            return pair;
        }

        // public Pair? Root => Parent is null ? this as Pair : Parent.Root;
    }

    class Number : SnailFishNum
    {
        public Number(int val)
        {
            Value = val;
        }
        public int Value { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override long Magnitude => Value;

        public Pair Split()
        {
            var pair = Pair.Create(Value / 2, (Value + 1) / 2);
            Parent?.ReplaceChild(this, pair);
            return pair;
        }

        public Number Add(int value)
        {
            var num = new Number(Value + value);
            Parent?.ReplaceChild(this, num);
            return num;
        }
    }

    class Pair : SnailFishNum
    {
        public SnailFishNum Left { get; set; }
        public SnailFishNum Right { get; set; }
        public static Pair Create(int leftValue, int rightValue) => Create(new Number(leftValue), new Number(rightValue));

        public static Pair Create(SnailFishNum left, SnailFishNum right)
        {
            var pair = new Pair { Left = left, Right = right };
            left.Parent = pair;
            right.Parent = pair;
            return pair;
        }

        public Pair? FindFirstPairWithDepth(int depth)
        {
            if (depth == 0) return this;
            return (Left as Pair)?.FindFirstPairWithDepth(depth - 1)
                ?? (Right as Pair)?.FindFirstPairWithDepth(depth - 1);
        }

        public Number? FindFirstNumberToSplit()
        {
            if (Left is Number { Value: >= 10 } numLeft) return numLeft;
            var leftNumFound = (Left as Pair)?.FindFirstNumberToSplit();
            if (leftNumFound is not null) return leftNumFound;
            if (Right is Number { Value: >= 10 } numRight) return numRight;
            return (Right as Pair)?.FindFirstNumberToSplit();
        }

        public void Reduce()
        {
            while (true)
            {
                var pairToExplode = FindFirstPairWithDepth(4);
                if (pairToExplode is not null)
                {
                    pairToExplode.Explode();
                    continue;
                }

                var numberToSplit = FindFirstNumberToSplit();
                if (numberToSplit is not null)
                {
                    numberToSplit.Split();
                    continue;
                }

                break;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override long Magnitude => 3*Left.Magnitude + 2*Right.Magnitude;

        public Number? FindLeafToTheLeftOfMe()
        {
            if (Parent is null) return null;
            return ReferenceEquals(Parent.Left, this)
                ? Parent.FindLeafToTheLeftOfMe()
                : Parent.Left.FindRightMostLeaf();
        }
        public Number? FindLeafToTheRightOfMe()
        {
            if (Parent is null) return null;
            return ReferenceEquals(Parent.Right, this)
                ? Parent.FindLeafToTheRightOfMe()
                : Parent.Right.FindLeftMostLeaf();
        }

        public void Explode()
        {
            (Left is Number).Assert();
            (Right is Number).Assert();
            var leftValue = ((Number) Left).Value;
            var rightValue = ((Number) Right).Value;
            var leafToTheLeftOfMe = FindLeafToTheLeftOfMe();
            leafToTheLeftOfMe?.Add(leftValue);
            var leafToTheRightOfMe = FindLeafToTheRightOfMe();
            leafToTheRightOfMe?.Add(rightValue);
            Parent?.ReplaceChild(this, new Number(0));
        }

        public Pair ReplaceChild(SnailFishNum toReplace, SnailFishNum newChild)
        {
            Left = ReferenceEquals(Left, toReplace) ? newChild : Left;
            Left.Parent = this;
            Right = ReferenceEquals(Right, toReplace) ? newChild : Right;
            Right.Parent = this;
            return this;
        }
    }
}