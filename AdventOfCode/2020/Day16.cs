using Rule = System.ValueTuple<string, System.Func<int, bool>>;

namespace AdventOfCode._2020;

public class Day16 : Solution
{
    public Day16() : base(16, 2020) { }
    protected override void Solve()
    {
        // p1:
        // var input = ReadText()
        //     .Split("\n\n");
        // var rules =
        //     input[0]
        //         .Split("\n")
        //         .Select(s =>
        //             s[(s.IndexOf(':') + 2)..]
        //                 .Split(" or ")
        //                 .Select(p => p.Split("-").ArrayMap(int.Parse))
        //                 .Select(r => new Func<int, bool>(x => x >= r[0] && x <= r[1]))
        //                 .ToArray())
        //         // .Select(cond => Or(cond[0], cond[1]))
        //         .Select(cond => cond.Aggregate(Or))
        //         .Aggregate(Or);
        //
        // var nearbyTickets =
        //     input[2]
        //         .Split("\n")
        //         .Skip(1)
        //         .SelectMany(line => line.Split(",").Select(int.Parse))
        //         .Where(x => !rules(x))
        //         .SumLong();
        //
        // Console.WriteLine(nearbyTickets);

        // p2:
        var input = ReadText()
            .Split("\n\n");
        var rules =
            input[0]
                .Split("\n")
                .Select(s => s.Split(": "))
                .Select(s =>
                    new Rule(
                        s[0],
                        s[1]
                            .Split(" or ")
                            .Select(p => p.Split("-").ArrayMap(int.Parse))
                            .Select(r => new Func<int, bool>(x => x >= r[0] && x <= r[1]))
                            .Aggregate(Or)
                    ))
                .ToArray();

        var matchesAnyRule =
            rules
                .Select(r => r.Item2)
                .Aggregate(Or);

        var validTickets =
            input[2]
                .Split("\n")
                .Skip(1)
                .Select(line => line.Split(",").Select(int.Parse))
                .Where(ticketValues => ticketValues.All(matchesAnyRule));

        var valuesPerIndex =
            validTickets
                .SelectMany(ticketValues => ticketValues.WithIndex())
                .GroupBy(value => value.Index)
                .OrderBy(g => g.Key)
                .Select(g => g.Select(value => value.Item))
                .ToArray();

        var rulesPerIndex =
            valuesPerIndex
                .Select(values
                    => rules
                        .Where(r => values.All(r.Item2))
                        .Select(r => r.Item1)
                        .ToHashSet())
                .ToArray();
        var rulePerIndex = new string[rulesPerIndex.Length];
        while (rulesPerIndex.Any(x => x.Count > 0))
        {
            var index = Array.FindIndex(rulesPerIndex, x => x.Count == 1);
            rulePerIndex[index] = rulesPerIndex[index].Single();
            rulesPerIndex.ForEach(set => set.Remove(rulePerIndex[index]));
        }

        var myTicket = input[1].Split("\n")[1].Split(",").ArrayMap(int.Parse);
        rulePerIndex
            .WithIndex()
            .Where(t => t.Item.StartsWith("departure"))
            .Select(t => (long)myTicket[t.Index])
            .Product()
            .ToString()
            .Print();
    }

    private static Func<T, bool> Or<T>(Func<T, bool> condition1, Func<T, bool> condition2)
        => x => condition1(x) || condition2(x);
}