open System
open System.IO
open AdventOfCode.Fsharp._2015.Day03

[<EntryPoint>]
let main argv =
    let input = File.ReadAllText("input.txt")
    part2 input |> ignore
//    part2 input |> ignore
    0