namespace AdventOfCode._2020;

public class Day7 : Solution
{
    public Day7() : base(7, 2020) { }
    protected override void Solve()
    {
        var lines = ReadLines();
        var carries = new Dictionary<string, List<(string, int)>>();
        var isCarriedBy = new Dictionary<string, List<string>>();
        lines.ForEach(line =>
        {
            line = line.Replace(" bags", "")
                .Replace(" bag", "")
                .Replace(".", "");
            var s = line.Split("contain").Select(a => a.Trim())
                .ToArray();
            var other = s[1]
                .If(s[1] == "no other",
                    _ => new List<(string, int)>(),
                    s1 => s1.ToStr()
                        .SplitBy(',')
                        .Select(bags => bags.Trim())
                        .Select(bags => (bags[2..], bags.Split()[0].ToInt()))
                        .ToList());

            carries.AddOrUpdate(s[0], other,
                (_, list) =>
                {
                    list.AddRange(other);
                    return list;
                });
            other.ForEach(o => isCarriedBy.AddOrUpdate(o.Item1,
                new List<string> {s[0]},
                (_, list) =>
                {
                    list.Add(s[0]);
                    return list;
                }));

        });

        // Console.WriteLine(carries);

        // var queue = new Queue<string>();
        // isCarriedBy["shiny gold"]
        //     .ForEach(queue.Enqueue);
        // var set = isCarriedBy["shiny gold"].ToHashSet();
        // while (queue.Count > 0)
        // {
        //     var item = queue.Dequeue();
        //     if (!isCarriedBy.ContainsKey(item)) continue;
        //     isCarriedBy[item]
        //         .ForEach(color =>
        //         {
        //             if (!set.Add(color)) return;
        //             queue.Enqueue(color);
        //         });
        // }
        //
        // Console.WriteLine(set.Count);

        var res = new Dictionary<string, int>();
        var queue = new Queue<(string, int)>();
        queue.Enqueue(("shiny gold", 1));
        while (queue.Count > 0)
        {
            var item = queue.Dequeue();
            if (carries.ContainsKey(item.Item1) == false) continue;
            carries[item.Item1].ForEach(color =>
            {
                var (item1, item2) = color;
                var count = item2 * item.Item2;
                res.AddOrUpdate(item1, count,
                    (_, i) => i + count);
                queue.Enqueue((item1, count));
            });
        }

        Console.WriteLine(res.Sum(kvp => kvp.Value));
    }
}