using System.Collections.Immutable;

namespace AdventOfCode._2020;

public class Day21 : Solution
{
    public Day21() : base(21, 2020)
    {
    }

    protected override void Solve()
    {
        var input =
            ReadLines()
                .Select(line => line.Split(" (contains "))
                .Select(s =>
                    (Ingredients: s[0].SplitBy(' ').ToImmutableHashSet(),
                        Allergens: s[1][..^1].SplitBy(' ', ',').ToImmutableHashSet()))
                .ToArray();
        // allergen to ingredient
        var map = new Dictionary<string, string>();
        var map2 = new Dictionary<string, ImmutableHashSet<string>>();

        foreach (var inp in input)
        {
            inp.Allergens.ForEach(a =>
            {
                map2.AddOrUpdate(a,
                    inp.Ingredients,
                    (_, cur) => cur.Intersect(inp.Ingredients));
            });
        }

        while (true)
        {
            var matched = map2.FirstOrDefault(kvp => kvp.Value.Count == 1);
            if (matched.Equals(default(KeyValuePair<string, ImmutableHashSet<string>>)))
                break;
            var key = matched.Key;
            var value = matched.Value.Single();
            map.Add(key, value);
            map2.Remove(key);
            map2.ForEach(kvp =>
            {
                map2[kvp.Key] = map2[kvp.Key].Remove(value);
            });
            input.Iteri((i, x) =>
            {
                input[i] = (x.Ingredients.Remove(value), x.Allergens.Remove(key));
            });
        }

        // p1:
        // Console.WriteLine(input.Select(x => x.Ingredients.Count).Sum());

        // p2:
        string.Join(',', map.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value))
            .Print();
    }
}