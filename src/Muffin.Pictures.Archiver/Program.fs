open System
open Muffin.Pictures.Archiver.Pictures
open Muffin.Pictures.Archiver.Mover
open Muffin.Pictures.Archiver.Paths
open Muffin.Pictures.Archiver.Files
open Muffin.Pictures.Archiver.TimeTakenRetriever

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

    let timeProvider () = DateTimeOffset.UtcNow
    let fileProvider () = allFilesInPath sourcePath
    let timeTakenRetriever = timeTaken

    let getPictures = getOldPictures timeTakenRetriever timeProvider fileProvider

    getPictures
        |> move targetPath
        |> ignore

    Console.WriteLine("Done archiving!") |> ignore

    0
