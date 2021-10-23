using System.Text.RegularExpressions;

namespace AdventOfCode._2020;

public class Day4 : Solution
{
    public Day4() : base(4, 2020) { }
    protected override void Solve()
    {
        Func<string, bool> getFunc(string pattern, Func<GroupCollection, bool> isValid)
        {
            return (s =>
            {
                var match = Regex.Match(s, pattern);
                return match.Success && isValid(match.Groups);
            });
        }
        Func<string, bool>[] req = {
            getFunc("byr:(\\d+)", g => g[1].Value.ToInt().IsBetweenNonStrict(1920, 2002)),
            getFunc("iyr:(\\d+)", g => g[1].Value.ToInt().IsBetweenNonStrict(2010, 2020)),
            getFunc("eyr:(\\d+)", g => g[1].Value.ToInt().IsBetweenNonStrict(2020, 2030)),
            getFunc("hgt:(\\d+)(cm|in)", g =>
                ((g[1].Value.ToInt().IsBetweenNonStrict(150, 193) && g[2].Value == "cm")
                 || (g[1].Value.ToInt().IsBetweenNonStrict(59, 76) && g[2].Value == "in"))),
            getFunc("hcl:#([0-9]|[a-f]){6}\\b", _ => true),
            getFunc("ecl:(amb|blu|brn|gry|grn|hzl|oth)\\b", _ => true),
        };
        var birthdays = ReadText().Split("\n\n");
        var res = birthdays.Count(b => req.All(r => r(b)));
        Console.WriteLine(res);
    }
}