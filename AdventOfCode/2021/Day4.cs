namespace AdventOfCode._2021;

public class Day4 : Solution
{
    public Day4() : base(4, 2021) { }
    protected override void Solve()
    {
        var lines = ReadLines();
        var nums = lines[0].SplitBy(',').Select(int.Parse).ToArray();
        var l = 1;
        var boards = new List<int[][]>();
        while (l < lines.Length)
        {
            l++;
            var bingoBoard = lines.Skip(l).Take(5).ToArray();
            var board = bingoBoard.Select(line => line.Split2().Select(int.Parse).ToArray()).ToArray();
            boards.Add(board);
            l += 5;
        }

        var wasNamed = boards.Select(b => b.Select(n => n.Select(a => false).ToArray()).ToArray()).ToList();

        bool hasWon(bool[][] board)
        {
            var anyRow = board.Any(row => row.All(x => x));
            if (anyRow) return true;
            var anyCol = false;
            for (var c = 0; c < 5; c++)
            {
                var thisCol = true;
                for (var r = 0; r < 5; r++)
                {
                    thisCol = thisCol && board[r][c];
                    if (thisCol is false) break;
                }
                anyCol = anyCol || thisCol;
                if (anyCol) return true;
            }

            return false;
        }

        int sumOfUnmarked(int[][] board, bool[][] marks)
        {
            var sum = 0;
            board.Iter(
                (r, c, val) =>
                {
                    if (!marks[r][c]) sum += val;
                });
            return sum;
        }

        var res = 0;
        for (var i = 0; i < nums.Length; i++)
        {
            var num = nums[i];
            var j = 0;
            var toRemove = new List<int>();
            foreach (var board in boards)
            {
                board.Iter(
                    (r, c, val) =>
                    {
                        if (num == val)
                            wasNamed[j][r][c] = true;
                    });

                if (hasWon(wasNamed[j]))
                {
                    toRemove.Add(j);
                    var sum = sumOfUnmarked(board, wasNamed[j]);
                    res = sum * num;
                    // Console.WriteLine(res);
                    // return;
                }
                j++;
            }

            toRemove.Reverse();
            foreach (var removeMe in toRemove)
            {
                boards.RemoveAt(removeMe);
                wasNamed.RemoveAt(removeMe);
            }
        }

        Console.WriteLine(res);
    }
}