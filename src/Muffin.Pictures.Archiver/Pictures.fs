namespace Muffin.Pictures.Archiver

open System

open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Rop
open Muffin.Pictures.Archiver.TagRetriever

module Pictures =
    open TimeTakenRetriever

    let toPicture timeTakenRetriever (tags:Tags.Root[]) file =
        let timeTaken = timeTakenRetriever tags file
        match timeTaken with
        | Some t -> Success { File = file; TakenOn = t; Location = location tags file}
        | None -> Failure <| Skip.FileHasNoTimeTaken file

    let isOld timeProvider picture =
        let { File = _; TakenOn = takenOn } = picture
        let currentTime : DateTimeOffset = timeProvider
        if currentTime.AddDays(-1.0) >= takenOn then
            Success picture
        else
            Failure <| Skip.PictureWasNotOldEnough picture

    let toOldPicture timeTakenRetriever timeProvider tags =
        toPicture timeTakenRetriever tags
        >=> (isOld timeProvider)

    let getPictures toOldPicture filesProvider exifTool directory =
        let tagsForAllFiles = callExifTool exifTool directory

        filesProvider directory
        |> List.map (toOldPicture tagsForAllFiles)
