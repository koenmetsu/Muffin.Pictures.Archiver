// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open System.IO

[<EntryPoint>]
let main argv = 

    let path = 
        match argv with 
        | [|first|] -> first
        | _ -> "."

    let filesToArchive path = 
        Directory.EnumerateFiles(path)
        |> Seq.map (fun fileName -> new FileInfo(fileName))

    let isOld (file:FileInfo) = file.LastWriteTimeUtc < DateTime.UtcNow.AddMonths(-1)
    
    let files = filesToArchive path

    printfn "Found %i files" (Seq.length files)

    let oldFiles = 
        files
        |> Seq.filter isOld

    printfn "Found %i old files" (Seq.length oldFiles)
    printfn "Would move the following files:"

    let printOldFile (file:FileInfo) = 
        printfn "%s with date %s" file.FullName (file.LastWriteTimeUtc.ToShortDateString())

    oldFiles
        |> Seq.iter printOldFile
        |> ignore
    Console.ReadLine() |> ignore

    0 // return an integer exit code
    