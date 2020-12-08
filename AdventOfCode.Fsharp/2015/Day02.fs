module AdventOfCode.Fsharp._2015.Day02

let part1 input =
    let boxes = input |> Seq.map (fun (x: string) -> x.Split('x') |> Array.map int)
    let paper (box: int array) =
        let sides = [|box.[0]*box.[1]; box.[1]*box.[2]; box.[2]*box.[0]|]
        (Seq.sum sides |> (*) 2) + (Array.min sides)
    let sum = boxes |> Seq.map paper |> Seq.sum
    printfn "%d" sum
    ignore

let part2 input =
    let boxes = input |> Seq.map (fun (x: string) -> x.Split('x') |> Array.map int)
    let ribbon (box: int array) =
        let perimeters = [|box.[0] + box.[1]; box.[1] + box.[2]; box.[2] + box.[0]|]
        let toWrap = perimeters |> Array.min |> (*) 2
        let bow = box |> Seq.reduce (*)
        toWrap + bow
    let sum = boxes |> Seq.map ribbon |> Seq.sum
    printfn "%d" sum
    ignore