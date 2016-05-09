namespace Muffin.Pictures.Archiver

open System
open System.Text.RegularExpressions

open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.TagRetriever

module TimeTakenRetriever =

    let private r = new Regex(":")

    let private parseDate strDate =
        match DateTimeOffset.TryParse(strDate) with
        | true, date -> date
        | _ -> let dateTaken = r.Replace(strDate, "-", 2)
               DateTimeOffset.Parse(dateTaken)

    let findExifCreateDate (tags:Tags.Root option) =
        match tags with
        | Some (t:Tags.Root) -> t.DateTimeOriginal
        | None -> None

    let findXmpCreateDate (tags:Tags.Root option) =
        match tags with
        | Some (t:Tags.Root) -> t.DateCreated
        | None -> None

    let timeTaken timeTakenMode tags file : TimeTaken option =
        let path = file.FullPath
        let fileTagValues = getTags tags path

        let findTagFunctions = [   findExifCreateDate
                                   findXmpCreateDate ]
        findTagFunctions
        |> List.tryPick (fun findTag -> findTag fileTagValues)
        |> function
            | Some x ->
                Some <| parseDate x
            | None ->
                match timeTakenMode with
                | Strict ->
                    None
                | Fallback ->
                    Some <| DateTimeOffset(System.IO.File.GetLastWriteTimeUtc(path))

