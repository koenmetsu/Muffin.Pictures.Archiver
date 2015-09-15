namespace Muffin.Pictures.Archiver

open System.Diagnostics
open FSharp.Data
open FSharp
open System.IO

module TagRetriever =

    type Tags = JsonProvider<"folder.json">

    let private exifFileName =
        if System.Type.GetType ("Mono.Runtime") <> null then
            "exiftool"
        else
            "exiftool.exe"

    let callExifTool folder =
        let processStartInfo = new ProcessStartInfo()
        processStartInfo.FileName <- exifFileName
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
        Tags.Parse json

    let getTags (tags:Tags.Root[]) (file:FilePath) =
        tags
        |> Array.tryFind(fun t -> t.SourceFile = file.Replace("\\","/"))
