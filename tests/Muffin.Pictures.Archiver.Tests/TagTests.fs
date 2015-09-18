namespace Muffin.Pictures.Archiver.Tests

open Swensen.Unquote
open NUnit.Framework
open FSharp.Data

open Muffin.Pictures.Archiver.Tests.TestHelpers
open Muffin.Pictures.Archiver.Domain

module TagRetriever =
    open System.Diagnostics

    type Tags = JsonProvider<"test.json">

    let callExifTool folder =
        let processStartInfo = new ProcessStartInfo()
        processStartInfo.FileName <- "exiftool"
        processStartInfo.Arguments <- sprintf "-fast22 -m -q -j -d \"%%Y.%%m.%%d %%H:%%M:%%S\" %s" folder
        processStartInfo.CreateNoWindow <- true
        processStartInfo.UseShellExecute <- false
        processStartInfo.RedirectStandardOutput <- true
        processStartInfo.RedirectStandardError <- true
        processStartInfo.RedirectStandardInput <- true
        let exifProcess = Process.Start(processStartInfo)
        let builder = new System.Text.StringBuilder()
        exifProcess.OutputDataReceived.Add(fun e ->
            builder.AppendLine(e.Data) |> ignore)
        exifProcess.BeginOutputReadLine()
        exifProcess.WaitForExit()
        let json = builder.ToString()
        json

module TagTests =
    open TagRetriever

    [<Test>]
    let ``Call exifTool and try to parse results`` () =
        let json = callExifTool @"TestData"

        let tags = Tags.Parse(json)

        test <@ Seq.length tags = 3 @>

    [<Test>]
    let ``Try parsing test`` () =
        let tags = Tags.Load("test.json")

        test <@ Seq.length tags = 5 @>
