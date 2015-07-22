namespace Muffin.Pictures.Archiver

open System.Text.RegularExpressions
open System
open Domain

module TimeTakenRetriever =

    let private r = new Regex(":")

    let private notNullOrEmpty = not << System.String.IsNullOrEmpty

    let private tryGetPrintValue (tagValue:ExifToolVBNetDemo.TagValue) =
        match tagValue.PrintValue with
            | [||] | null -> None
            | printValue -> Seq.tryFind (fun pValue -> notNullOrEmpty pValue) printValue

    let private parseDate strDate =
        match DateTimeOffset.TryParse(strDate) with
        | true, date -> date
        | _ -> let dateTaken = r.Replace(strDate, "-", 2)
               DateTimeOffset.Parse(dateTaken)

    let private timeTakenFromPath (path : string) : TimeTaken =
        ExifToolLib.ExifToolIO.Initiailize() |> ignore // todo;
        let fileTagValues = ExifToolVBNetDemo.FileTagValues(path, [|"XMP-xmp:CreateDate";"ExifIFD:CreateDate"|])

        fileTagValues.TagValueList
        |> Seq.cast
        |> Seq.tryPick (fun tagValue -> tryGetPrintValue tagValue)
        |> function
            | Some x -> parseDate x
            | None -> DateTimeOffset(System.IO.File.GetLastWriteTimeUtc(path))

    let timeTaken (file : File) =
        timeTakenFromPath file.FullPath