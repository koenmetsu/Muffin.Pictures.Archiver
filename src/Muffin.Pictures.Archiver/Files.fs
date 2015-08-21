namespace Muffin.Pictures.Archiver

open System.IO

open Muffin.Pictures.Archiver.Domain

module Files =

    let allFilesInPath path : seq<File> =
        let createFileInfo filePath =
            FileInfo filePath

        Directory.EnumerateFiles path
        |> Seq.map createFileInfo
        |> Seq.map (fun fileInfo -> {FullPath=fileInfo.FullName; Name=fileInfo.Name})