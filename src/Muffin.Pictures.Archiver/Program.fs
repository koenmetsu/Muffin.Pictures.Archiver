open System
open Muffin.Pictures.Archiver.Files

[<EntryPoint>]
let main argv =

    let sourcePath =
        match argv with
        | [|first|] -> first
        | _ -> @"."

    allFilesInPath sourcePath
        |> onlyOldFiles
        |> groupByMonth
        |> mapTargetPath sourcePath
        |> move
        |> ignore

    Console.WriteLine("Done archiving!") |> ignore

    0
