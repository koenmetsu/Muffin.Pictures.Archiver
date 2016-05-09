namespace Muffin.Pictures.Archiver

open System.IO

open Muffin.Pictures.Archiver.Domain

module Files =

    let allFilesInPath path =
        let createFileInfo filePath =
            FileInfo filePath

        Directory.EnumerateFiles(path, "*.jpg")
        |> List.ofSeq
        |> List.map createFileInfo
        |> List.map (fun fileInfo -> { FullPath=fileInfo.FullName; Name=fileInfo.Name })
