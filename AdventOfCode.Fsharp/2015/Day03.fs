module AdventOfCode.Fsharp._2015.Day03
let diff c =
    match c with
    | '^' -> (-1, 0)
    | '>' -> (0, 1)
    | 'v' -> (1, 0)
    | '<' -> (0, -1)
    | _ -> (0, 0)

let addTuples (t1: int*int) (t2: int*int) =
    (fst t1 + fst t2, snd t1 + snd t2)

let start = (0, 0)

let part1 input =
    let set = input |> (Seq.fold (fun state c -> (addTuples (diff c) (List.head state)) :: state) [start]) |> Set.ofSeq
    let setCount = set |> Set.count
    printfn "%d" setCount
    ignore

let part2 input =
    let isEven index = index % 2 = 0
    let filterByIndex input indexFilter = input |> Seq.indexed |> Seq.filter (fun (index, item) -> indexFilter index) |> Seq.map snd
    let santa = filterByIndex input isEven
    let roboSanta = filterByIndex input (isEven >> not)
    let getCoordinatesList path = path |> (Seq.fold (fun state c -> (addTuples (diff c) (List.head state)) :: state) [start])
    let listSanta = getCoordinatesList santa
    let listRoboSanta = getCoordinatesList roboSanta
    let set = listSanta |> (@) listRoboSanta |> Set.ofList
    let setCount = set |> Set.count
    printfn "%d" setCount
    ignore