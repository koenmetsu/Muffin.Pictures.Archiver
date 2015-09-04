open System

open Muffin.Pictures.Archiver.Arguments
open Muffin.Pictures.Archiver.CompositionRoot
open Muffin.Pictures.Archiver.Runner

open System.Diagnostics

[<EntryPoint>]
let main argv =

    let callExifTool folder =
        let processStartInfo = new ProcessStartInfo()
        processStartInfo.FileName <- "D:\Projects\Muffin.Pictures.Archiver\lib\exiftool.exe"
        processStartInfo.Arguments <- sprintf "-fast22 -m -q -j -d \"%%Y.%%m.%%d %%H:%%M:%%S\" %s" folder
        processStartInfo.CreateNoWindow <- true
        processStartInfo.UseShellExecute <- false
        processStartInfo.RedirectStandardOutput <- true
        processStartInfo.RedirectStandardError <- true
        processStartInfo.RedirectStandardInput <- true
        let builder = new System.Text.StringBuilder()
        let exifProcess = Process.Start(processStartInfo)
        exifProcess.OutputDataReceived.Add(fun e ->
            builder.Append(e.Data) |> ignore)
        exifProcess.BeginOutputReadLine()
        exifProcess.WaitForExit() //(5000) |> ignore
        let json = builder.ToString()//exifProcess.StandardOutput.ReadToEnd()
        Console.WriteLine(json)

    let json = callExifTool @"D:\Projects\Muffin.Pictures.Archiver\tests\Muffin.Pictures.Archiver.Tests\testdata\"
    Console.ReadLine() |> ignore
    0

//
//    let arguments = parseArguments argv
//
//    let move = composeMove
//    let getMoveRequests = composeGetMoveRequests arguments
//
//    runner move getMoveRequests arguments
//
//    0
