namespace Muffin.Pictures.Archiver

open System
open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Age

module Pictures =

    let toPicture timeTakenRetriever (file : File) =
        {Picture.File = file; TakenOn = timeTakenRetriever file}

    let getOldPictures (timeTakenRetriever : File -> TimeTaken) (timeProvider : unit -> DateTimeOffset) (files : seq<File>) =
        files
        |> Seq.map (fun file -> toPicture timeTakenRetriever file)
        |> Seq.filter (fun picture -> isOld timeProvider picture)
