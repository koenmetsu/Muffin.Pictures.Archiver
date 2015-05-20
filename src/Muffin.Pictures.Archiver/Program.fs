// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open System.IO
open Muffin.Pictures.Archiver.Files

[<EntryPoint>]
let main argv = 

    let path = 
        match argv with 
        | [|first|] -> first
        | _ -> "."

    let files = allFilesInPath path

    printfn "Found %i files in total" (Seq.length files)

    let oldFiles = onlyOldFiles files

    printfn "Found %i old files" (Seq.length oldFiles)
    printfn "Would move the following files:"

    printOldFiles files

    Console.ReadLine() |> ignore

    0 // return an integer exit code
    