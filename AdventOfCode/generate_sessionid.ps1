$code = @"
namespace AdventOfCode
{
    partial class Solution
    {
        // take from edge://settings/cookies/detail?site=adventofcode.com
        private const string SessionId = "your-session-id-from-cookies";
    }
}
"@

Set-Content -Path "$PSScriptRoot/Solution.SessionId.cs" $code -Force

$code = @"
namespace FetchInput
{
    partial class Program
    {
        // take from edge://settings/cookies/detail?site=adventofcode.com
        private const string SessionId = "your-session-id-from-cookies";
    }
}
"@

Set-Content -Path "$PSScriptRoot/../FetchInput/Program.SessionId.cs" $code -Force