namespace Muffin.Pictures.Archiver

open System

open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Rop

module Pictures =

    let toPicture timeTakenRetriever (file : File) =
        let timeTaken = timeTakenRetriever file
        match timeTaken with
        | Some t -> Success {Picture.File = file; TakenOn = t}
        | None -> Failure <| Skip.FileHasNoTimeTaken file

    let isOld (timeProvider : unit -> DateTimeOffset) picture =
        // todo: replace this with single time at start of the program,
        // ie: not a function but a DT value
        let {File=_; TakenOn=takenOn} = picture
        let currentTime = timeProvider ()
        if currentTime.AddMonths(-1) > takenOn then
            Success picture
        else
            Failure <| Skip.PictureWasNotOldEnough picture

    let getOldPictures timeTakenRetriever (timeProvider : unit -> DateTimeOffset) (filesProvider : string -> seq<File>) path =
        let toOldPicture =
            toPicture timeTakenRetriever
            >=> (isOld timeProvider)

        filesProvider path
        |> List.ofSeq
        |> List.map toOldPicture
