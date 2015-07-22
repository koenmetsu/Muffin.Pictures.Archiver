open System
open Muffin.Pictures.Archiver.Pictures
open Muffin.Pictures.Archiver.Mover
open Muffin.Pictures.Archiver.Paths
open Muffin.Pictures.Archiver.Files
open Muffin.Pictures.Archiver.TimeTakenRetriever
open Muffin.Pictures.Archiver.Age
open Muffin.Pictures.Archiver.Domain

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
    let fileProvider = allFilesInPath sourcePath

    let getPictures = getOldPictures timeTaken timeProvider fileProvider

    getPictures
        |> move targetPath
        |> ignore

    Console.WriteLine("Done archiving!") |> ignore

    0
