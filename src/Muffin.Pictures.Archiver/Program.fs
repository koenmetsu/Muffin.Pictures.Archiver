open System
open System.IO
open Muffin.Pictures.Archiver.Files

[<EntryPoint>]
let main argv = 

    let sourcePath = 
        match argv with 
        | [|first|] -> first
        | _ -> "."

    let files = allFilesInPath sourcePath
    let oldFiles = onlyOldFiles files

    printfn "Found %i files in total" (Seq.length files)
    printfn "Found %i old files" (Seq.length oldFiles)
    printfn "Would move the following files:"
    oldFiles |> Seq.iter printFile

    Console.ReadLine() |> ignore

    0
    