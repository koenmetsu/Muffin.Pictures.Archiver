namespace Muffin.Pictures.Archiver

open System.IO
open System
open Domain

module Files =

    let allFilesInPath path : seq<Domain.File> =
        let createFileInfo filePath =
            FileInfo filePath

        Directory.EnumerateFiles path
        |> Seq.map createFileInfo
        |> Seq.map (fun fileInfo -> {FullPath=fileInfo.FullName; Name=fileInfo.Name})