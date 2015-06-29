open System
open Muffin.Pictures.Archiver.Files
open Muffin.Pictures.Mover.Mover

[<EntryPoint>]
let main argv =

    let sourcePath =
        match argv with
        | [|first|] -> first
        | _ -> @"."

    sourcePath
        |> getOldFilesByMonth
        |> mapTargetPath sourcePath
        |> move
        |> ignore

    Console.WriteLine("Done archiving!") |> ignore

    0
