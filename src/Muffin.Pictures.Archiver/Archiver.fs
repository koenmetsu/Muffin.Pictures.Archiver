namespace Muffin.Pictures.Archiver

open System.IO
open System
open TimeTakenRetriever

module Files =
    let isOld (file:FileInfo) = DateTimeOffset.UtcNow.AddMonths(-1) > timeTaken file.FullName

    let getMonthYear (file:FileInfo) =
        let time = timeTaken file.FullName
        sprintf "%i-%02i" time.Year time.Month

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
