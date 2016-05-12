namespace Muffin.Pictures.Archiver

open System
open System.Text.RegularExpressions

open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.TagRetriever

module TimeTakenRetriever =

    let private r = new Regex(":")

    let private parseDate strDate =
        match strDate with
        | Some d ->
            match DateTimeOffset.TryParse(d) with
            | true, date -> date
            | _ -> let dateTaken = r.Replace(d, "-", 2)
                   DateTimeOffset.Parse(dateTaken)
            |> Some
        | None -> None


    let findExifCreateDate (tags:Tags.Root option) =
        match tags with
        | Some (t:Tags.Root) -> parseDate t.DateTimeOriginal
        | None -> None

    let findXmpCreateDate (tags:Tags.Root option) =
        match tags with
        | Some (t:Tags.Root) -> parseDate t.DateCreated
        | None -> None

    let findTimeInName (tags:Tags.Root option) =
        match tags with
        | Some (t:Tags.Root) ->
            let dateRegex = Regex("[0-9]{8}_[0-9]{6}")
            let matched = dateRegex.Match(t.FileName)
            if matched.Success then
               let couldParse, parsed = DateTimeOffset.TryParseExact(matched.Value, [|"yyyyMMdd_HHmmss"|], System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None)
               if couldParse
               then Some parsed
               else None
            else None
        | None -> None

    let timeTaken timeTakenMode tags file : TimeTaken option =
        let path = file.FullPath
        let fileTagValues = getTags tags path

        let findTagFunctions = [   findExifCreateDate
                                   findXmpCreateDate
                                   findTimeInName ]
        findTagFunctions
        |> List.tryPick (fun findTag -> findTag fileTagValues)
        |> function
            | Some x -> Some x
            | None ->
                match timeTakenMode with
                | Strict ->
                    None
                | Fallback ->
                    Some <| DateTimeOffset(System.IO.File.GetLastWriteTimeUtc(path))

    let location tags file : string option =
        let path = file.FullPath
        let fileTagValues = getTags tags path

        match fileTagValues with
        | Some (t:Tags.Root) ->
            match t.GpsLongitude, t.GpsLatitude with
            | Some lat, Some lon ->
                Some <| sprintf "%s, %s" lon lat
            | _ -> None
        | None -> None


