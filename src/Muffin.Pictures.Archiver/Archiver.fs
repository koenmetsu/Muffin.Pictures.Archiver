namespace Muffin.Pictures.Archiver

open System.IO
open System
open TimeTakenRetriever

module Files =

    type FilesPerMonth = string * seq<FileInfo>

    let private isOld (file:FileInfo) = DateTimeOffset.UtcNow.AddMonths(-1) > timeTaken file.FullName

    let private getMonthYear (file:FileInfo) =
        let time = timeTaken file.FullName
        sprintf "%i-%02i" time.Year time.Month

    let private allFilesInPath path =
        Directory.EnumerateFiles path
        |> Seq.map (fun fileName -> new FileInfo(fileName))

    let private onlyOldFiles files =
        files
        |> Seq.filter<FileInfo> isOld

    let private groupByMonth files =
        files
        |> Seq.groupBy (fun file -> getMonthYear file)

    let combine basePath target =
        System.IO.Path.Combine(basePath, target)

    let private mapWithFullPath basePath (fileGroup:FilesPerMonth) =
        let (month,files) = fileGroup
        let target =
            month
            |> combine basePath

        (target, files)

    let mapTargetPath basePath (filesPerMonth:seq<FilesPerMonth>) =
        filesPerMonth
        |> Seq.map (fun files -> mapWithFullPath basePath files)

    let getOldFilesByMonth sourcePath =
        sourcePath
        |> allFilesInPath
        |> onlyOldFiles
        |> groupByMonth
