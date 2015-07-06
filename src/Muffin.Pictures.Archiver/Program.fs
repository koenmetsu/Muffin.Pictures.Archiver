open System
open Muffin.Pictures.Archiver.Files
open Muffin.Pictures.Mover.Mover

[<EntryPoint>]
let main argv =

    let sourcePath =
        match argv with
        | [|source|] -> source
        | [|source; _|] -> source
        | _ -> @"."

    let targetPath =
        match argv with
        | [|_; target|] -> target
        | _ -> sourcePath

    sourcePath
        |> getOldPicturesWithMonth
        |> getMoves targetPath
        |> move
        |> ignore

    Console.WriteLine("Done archiving!") |> ignore

    0
