namespace Muffin.Pictures.Archiver

open System
open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Age

module Pictures =

    let toPicture timeTakenRetriever (file : File) =
        let timeTaken = timeTakenRetriever file
        match timeTaken with
        | Some t -> Some {Picture.File = file; TakenOn = t}
        | None -> None

    let getOldPictures timeTakenRetriever (timeProvider : unit -> DateTimeOffset) (filesProvider : string -> seq<File>) path =
        filesProvider path
        |> Seq.choose (toPicture timeTakenRetriever)
        |> Seq.filter (isOld timeProvider)
