using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace AdventOfCode._2020;

public class Day22 : Solution
{
    public Day22() : base(22, 2020) { }
    protected override void Solve()
    {
        var input =
            ReadText()
                .Split("\n\n")
                .Select(s => s.SplitBy('\n'))
                .Select(s => s[1..].Select(int.Parse).ToList())
                .ToArray();

        static int Hash(List<int> p1, List<int> p2)
        {
            return string.Join(',', p1
                .Concat(p2)
                .Append(p1.Count)).GetHashCode();
        }

        static int Score(List<int> winner)
        {
            return winner
                .AsEnumerable()
                .Reverse()
                .WithIndex()
                .Select(x => x.Item * (x.Index + 1))
                .Sum();
        }

        // p1:
        // while (true)
        // {
        //     if (input[0].Count == 0 ||
        //         input[1].Count == 0)
        //         break;
        //     var first = input[0][0];
        //     var second = input[1][0];
        //     input[0].RemoveAt(0);
        //     input[1].RemoveAt(0);
        //
        //     if (first > second)
        //     {
        //         input[0].Add(first);
        //         input[0].Add(second);
        //     }
        //     else
        //     {
        //         input[1].Add(second);
        //         input[1].Add(first);
        //     }
        // }

        // var winner = input[0].Count == 0 ? input[1] : input[0];
        // Console.WriteLine(Score(winner));


        // if returnScore is true, returns winner's scope
        // otherwise returns winner: 0 or 1
        int Game(List<int> p1, List<int> p2, bool returnScore = false)
        {
            var roundsPlayed = new HashSet<int>();

            while (true)
            {
                if (p1.Count == 0)
                {
                    return returnScore ? Score(p2) : 1;
                }
                if (p2.Count == 0)
                {
                    return returnScore ? Score(p1) : 0;
                }

                var hash = Hash(p1, p2);
                if (!roundsPlayed.Add(hash))
                {
                    // p1 wins
                    return returnScore ? Score(p1) : 0;
                }

                // Console.Write("Player 1's deck: ");
                // p1.Print();
                // Console.Write("Player 2's deck: ");
                // p2.Print();

                var first = p1[0];
                var second = p2[0];
                p1.RemoveAt(0);
                p2.RemoveAt(0);

                // Console.WriteLine($"Player 1 plays: {first}");
                // Console.WriteLine($"Player 2 plays: {second}");

                var winner =
                    (p1.Count >= first && p2.Count >= second)
                        ? Game(p1.Take(first).ToList(), p2.Take(second).ToList())
                        : (first > second ? 0 : 1);

                // Console.WriteLine($"Player {winner + 1} wins round x of game x");
                // Console.WriteLine();
                if (winner == 0)
                {
                    p1.Add(first);
                    p1.Add(second);
                }
                else
                {
                    p2.Add(second);
                    p2.Add(first);
                }
            }
        }

        var score = Game(input[0], input[1], returnScore: true);
        Console.WriteLine(score);
    }
}