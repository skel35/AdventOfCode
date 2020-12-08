module AdventOfCode.Fsharp._2015.Day01

let part1 input = input |> Seq.sumBy (function '(' -> 1 | _ -> -1)
let part2 input =
    input
    |> Seq.scan (fun acc x -> acc + match x with | '(' -> 1 | _ -> - 1) 0
    |> (Seq.findIndex ((=) -1))
