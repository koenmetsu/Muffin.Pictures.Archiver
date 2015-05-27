namespace Muffin.Pictures.Archiver

open System.IO
open System

module Files =
    let isOld (file:FileInfo) = file.LastWriteTimeUtc < DateTime.UtcNow.AddMonths(-1)

    let getMonthYear (file:FileInfo) =
        sprintf "%i-%i" file.LastWriteTimeUtc.Year file.LastWriteTimeUtc.Month

    let allFilesInPath path =
        Directory.EnumerateFiles path
        |> Seq.map (fun fileName -> new FileInfo(fileName))

    let onlyOldFiles files =
        files
        |> Seq.filter<FileInfo> isOld

    let groupByMonth files =
        files
        |> Seq.groupBy (fun file -> getMonthYear file)

    let targetPath basePath target =
        System.IO.Path.Combine(basePath, target)

    let mapWithFullPath basePath (fileGroup:string * seq<FileInfo>) =
        let target =
            fst fileGroup
            |> targetPath basePath
        let files = snd fileGroup

        (target, files)

    let moveFile targetBase (file:FileInfo) =
        let targetFull = targetPath targetBase file.Name
        System.IO.Directory.CreateDirectory(targetBase)
        |> ignore
        System.IO.File.Move(file.FullName, targetFull)

    let moveFiles (fileGroup:string * seq<FileInfo>) =
        let targetBase = fst fileGroup
        snd fileGroup
        |> Seq.iter (fun file -> moveFile targetBase file)

    let move basePath (files:seq<string * seq<FileInfo>>) =
        files
        |> Seq.map (fun fileGroup -> mapWithFullPath basePath fileGroup)
        |> Seq.iter (fun fileGroup -> moveFiles fileGroup)
