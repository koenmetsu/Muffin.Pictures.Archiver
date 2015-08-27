namespace Muffin.Pictures.Archiver

open System
open System.Text.RegularExpressions

open Muffin.Pictures.Archiver.Domain

module TimeTakenRetriever =

    let private r = new Regex(":")

    let private tryGetPrintValue (tagValue:ExifToolVBNetDemo.TagValue) =
        match tagValue.PrintValue with
            | [||] | null -> None
            | printValue -> Seq.tryFind (fun pValue -> notNullOrEmpty pValue) printValue

    let private parseDate strDate =
        match DateTimeOffset.TryParse(strDate) with
        | true, date -> date
        | _ -> let dateTaken = r.Replace(strDate, "-", 2)
               DateTimeOffset.Parse(dateTaken)

    let private timeTakenFromPath timeTakenMode (path : string) =
        ExifToolLib.ExifToolIO.Initiailize() |> ignore // todo;
        let fileTagValues = ExifToolVBNetDemo.FileTagValues(path, [|"XMP-xmp:CreateDate";"ExifIFD:CreateDate"|])

        fileTagValues.TagValueList
        |> Seq.cast
        |> Seq.tryPick tryGetPrintValue
        |> function
            | Some x ->
                Some <| parseDate x
            | None ->
                match timeTakenMode with
                | Strict ->
                    None
                | Fallback ->
                    Some <| DateTimeOffset(System.IO.File.GetLastWriteTimeUtc(path))

    let timeTaken timeTakenMode (file : File) =
        timeTakenFromPath timeTakenMode file.FullPath
