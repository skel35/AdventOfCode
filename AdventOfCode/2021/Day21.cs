namespace AdventOfCode._2021;

public class Day21 : Solution
{
    public Day21() : base(21, 2021) { }
    protected override void Solve()
    {
        void Part1()
        {
            var p1 = 4;
            var p2 = 3 ;
            var p1Score = 0;
            var p2Score = 0;
            var dieState = 1;
            var rolledTimes = 0;
            bool rollDie(ref int dieState, ref int rolledTimes, ref int pPos, ref int pScore)
            {
                var rollSum = dieState + (dieState + 1) % 100 + (dieState + 2) % 100;
                dieState = (dieState + 3) % 100;
                rolledTimes += 3;
                pPos = (pPos - 1 + rollSum) % 10 + 1;
                pScore += pPos;
                if (pScore >= 1000)
                {
                    return true;
                }
                return false;
            }

            while (true)
            {
                if (rollDie(ref dieState, ref rolledTimes, ref p1, ref p1Score)
                    || rollDie(ref dieState, ref rolledTimes, ref p2, ref p2Score))
                {
                    var loserScore = Math.Min(p1Score, p2Score);
                    (loserScore * rolledTimes).ToString().Print();
                    return;
                }
            }
        }

        var score = new Dictionary<((int sc, int pos) p1, (int sc, int pos) p2), long>();
        score[((0, 4), (0, 3))] = 1L;
        var p1Wins = 0L;
        var p2Wins = 0L;
        while (score.Count > 0)
        {
            var rollSums = new[]
            {
                (val: 3, freq: 1L),
                (val: 4, freq: 3L),
                (val: 5, freq: 6L),
                (val: 6, freq: 7L),
                (val: 7, freq: 6L),
                (val: 8, freq: 3L),
                (val: 9, freq: 1L)
            };

            score.ToArray().ForEach(
                kvp =>
                {
                    var (p1, p2) = kvp.Key;
                    foreach (var (rollSum, rollFreq) in rollSums)
                    {
                        var p1Pos = (p1.pos - 1 + rollSum) % 10 + 1;
                        var p1Score = p1.sc + p1Pos;
                        var val = kvp.Value * rollFreq;
                        if (p1Score >= 21) p1Wins += val;
                        else
                            score.AddOrUpdate(((p1Score, p1Pos), p2), val, (_, curVal) => curVal + val);
                    }

                    var prevVal = score[kvp.Key];
                    if (prevVal == kvp.Value) score.Remove(kvp.Key);
                    else score[kvp.Key] = prevVal - kvp.Value;
                });
            if (score.Count == 0)
            {
                break;
            }
            score.ToArray().ForEach(
                kvp =>
                {
                    var (p1, p2) = kvp.Key;
                    foreach (var (rollSum, rollFreq) in rollSums)
                    {
                        var p2Pos = (p2.pos - 1 + rollSum) % 10 + 1;
                        var p2Score = p2.sc + p2Pos;
                        var val = kvp.Value * rollFreq;
                        if (p2Score >= 21) p2Wins += val;
                        else
                            score.AddOrUpdate((p1, (p2Score, p2Pos)), val, (_, curVal) => curVal + val);
                    }

                    var prevVal = score[kvp.Key];
                    if (prevVal == kvp.Value) score.Remove(kvp.Key);
                    else score[kvp.Key] = prevVal - kvp.Value;
                });
        }

        Math.Max(p1Wins, p2Wins).ToString().Print();
    }
}