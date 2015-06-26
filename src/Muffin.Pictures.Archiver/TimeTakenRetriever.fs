namespace Muffin.Pictures.Archiver

open System.Text.RegularExpressions
open System

module TimeTakenRetriever =

    let r = new Regex(":")

    let notNullOrEmpty = not << System.String.IsNullOrEmpty

    let tryGetPrintValue (tagValue:ExifToolVBNetDemo.TagValue) =
        match tagValue.PrintValue with
            | [||] | null -> None
            | printValue -> Seq.tryFind (fun pValue -> notNullOrEmpty pValue) printValue

    let parseDate strDate =
        match DateTimeOffset.TryParse(strDate) with
        | true, date -> date
        | _ -> let dateTaken = r.Replace(strDate, "-", 2)
               DateTimeOffset.Parse(dateTaken)

    let timeTaken path =
        ExifToolLib.ExifToolIO.Initiailize() |> ignore // todo;
        let fileTagValues = ExifToolVBNetDemo.FileTagValues(path, [|"XMP-xmp:CreateDate";"ExifIFD:CreateDate"|])

        let date =
            fileTagValues.TagValueList
            |> Seq.cast
            |> Seq.tryPick (fun tagValue -> tryGetPrintValue tagValue)
            |> function
                | Some x -> parseDate x
                | None -> DateTimeOffset(System.IO.File.GetLastWriteTimeUtc(path))

        date