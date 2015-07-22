namespace Muffin.Pictures.Archiver

open System.IO
open System
open Muffin.Pictures.Archiver.TimeTakenRetriever
open Muffin.Pictures.Archiver.Age
open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Files

module Pictures =

    let toPicture timeTakenRetriever (file : File) =
        {Picture.File = file; TakenOn = timeTakenRetriever file.FullPath}

    let getOldPictures (timeTakenRetriever : string -> TimeTaken) (timeProvider : unit -> DateTimeOffset) (files : unit -> seq<File>) =
        files ()
        |> Seq.map (fun file -> toPicture timeTakenRetriever file)
        |> Seq.filter (fun picture -> isOld timeProvider picture)
