namespace Muffin.Pictures.Archiver

open System

open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Rop
open Muffin.Pictures.Archiver.TagRetriever

module Pictures =

    let toPicture timeTakenRetriever (tags:Tags.Root[]) file =
        let timeTaken = timeTakenRetriever tags file
        match timeTaken with
        | Some t -> Success { File = file; TakenOn = t }
        | None -> Failure <| Skip.FileHasNoTimeTaken file

    let isOld timeProvider picture =
        // todo: replace this with single time at start of the program,
        // ie: not a function but a DT value
        let { File = _; TakenOn = takenOn } = picture
        let currentTime : DateTimeOffset = timeProvider ()
        if currentTime.AddMonths(-1) > takenOn then
            Success picture
        else
            Failure <| Skip.PictureWasNotOldEnough picture

    let toOldPicture timeTakenRetriever timeProvider tags =
        toPicture timeTakenRetriever tags
        >=> (isOld timeProvider)

    let getPictures toOldPicture filesProvider directory =
        let tagsForAllFiles = callExifTool directory

        filesProvider directory
        |> List.map (toOldPicture tagsForAllFiles)
