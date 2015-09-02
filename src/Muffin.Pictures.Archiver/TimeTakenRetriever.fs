namespace Muffin.Pictures.Archiver

open System
open System.Text.RegularExpressions

open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Rop

module TimeTakenRetriever =

    let private r = new Regex(":")

    let private parseDate strDate =
        match DateTimeOffset.TryParse(strDate) with
        | true, date -> date
        | _ -> let dateTaken = r.Replace(strDate, "-", 2)
               DateTimeOffset.Parse(dateTaken)

    let findExifCreateDate tags =
        tags |> Seq.tryPick (fun tag ->
                                match fst(tag) with
                                | "Date/Time Original" -> Some (snd tag)
                                | _ -> None)

    let findXmpCreateDate tags =
        tags |> Seq.tryPick (fun tag ->
                                match fst(tag) with
                                | "Create Date" -> Some (snd tag)
                                | _ -> None)


    let private timeTakenFromPath timeTakenMode tagProvider path =
        let fileTagValues = tagProvider path

        let findTagFunctions = [   findExifCreateDate
                                   findXmpCreateDate]
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

    let timeTaken timeTakenMode tagProvider file =
        timeTakenFromPath timeTakenMode tagProvider file.FullPath
