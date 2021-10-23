using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode._2016;

public class Day5 : Solution
{
    public Day5() : base(5, 2016) { }
    protected override void Solve()
    {
        var doorId = "cxdnnyjw";
        var md5 = MD5.Create();
        var j = 0;
        var password = new StringBuilder("        ");
        for (var i = 0; i < int.MaxValue - 1; i++)
        {
            var inp = doorId + i.ToString();
            var bytes = Encoding.Default.GetBytes(inp);
            var hashBytes = md5.ComputeHash(bytes);
            if (hashBytes[0] == 0 && hashBytes[1] == 0
                                  && hashBytes[2] / 16 == 0
                                  && hashBytes[2] % 16 < 8)
            {
                var c = hashBytes[3] / 16;
                char ch = (char) (c > 9 ? 'a' + (c - 10) : '0'+c);
                var pos = hashBytes[2] % 16;
                if (password[pos] != ' ') continue;
                password[pos] = ch;
                // password.Append(ch);
                j++;
                if (j == 8) break;
            }
        }

        Console.WriteLine(password.ToString());
    }
}