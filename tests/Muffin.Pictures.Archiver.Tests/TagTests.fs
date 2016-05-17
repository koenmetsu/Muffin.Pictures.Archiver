namespace Muffin.Pictures.Archiver.Tests

open Swensen.Unquote
open NUnit.Framework
open FSharp.Data

open Muffin.Pictures.Archiver.Tests.TestHelpers
open Muffin.Pictures.Archiver.Domain

module TagRetriever =
    open System.Diagnostics

    type Tags = JsonProvider<"example_exiftool_output.json">

    let callExifTool exifTool folder =
        let processStartInfo = new ProcessStartInfo()
        processStartInfo.FileName <- exifTool
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
    open System.IO

    let private location name =
        System.Reflection.Assembly.GetExecutingAssembly().Location
        |> Directory.GetParent
        |> fun dir -> Path.Combine(dir.FullName, name)

    [<Test>]
    let ``Call exifTool and try to parse results`` () =
        let json = callExifTool
                        (location "exifTool.exe")
                        (location @"TestData")

        let tags = Tags.Parse(json)

        test <@ Seq.length tags = 3 @>

    [<Test>]
    let ``Try parsing test`` () =
        let tags = Tags.Load("example_exiftool_output.json")

        test <@ Seq.length tags = 5 @>
