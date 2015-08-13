namespace Muffin.Pictures.Archiver

open System
open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Age

module Pictures =

    let toPicture timeTakenRetriever (file : File) =
        {Picture.File = file; TakenOn = timeTakenRetriever file}

    let getOldPictures (timeTakenRetriever : File -> TimeTaken) (timeProvider : unit -> DateTimeOffset) (filesProvider : string -> seq<File>) path=
        filesProvider path
        |> Seq.map (toPicture timeTakenRetriever)
        |> Seq.filter (isOld timeProvider)