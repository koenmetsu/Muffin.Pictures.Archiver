namespace Muffin.Pictures.Archiver

open System.IO
open System
open TimeTakenRetriever

module Files =

    type Picture = {FullPath:string; Name:string; TakenOn:DateTimeOffset}
    type PicturesPerMonth = {MonthYear:string; Pictures:Picture seq}

    let private isOld {FullPath=_; TakenOn=takenOn} = DateTimeOffset.UtcNow.AddMonths(-1) > takenOn

    let private getMonthYear picture =
        let time = picture.TakenOn
        sprintf "%i-%02i" time.Year time.Month

    let private allFilesInPath path =
        Directory.EnumerateFiles path
        |> Seq.map FileInfo
        |> Seq.map (fun fileInfo -> {FullPath=fileInfo.FullName; Name=fileInfo.Name; TakenOn=timeTaken fileInfo.FullName})

    let private onlyOldFiles files =
        files
        |> Seq.filter isOld

    let private groupByMonth files =
        files
        |> Seq.groupBy (fun file -> getMonthYear file)
        |> Seq.map (fun group -> {MonthYear= fst group; Pictures= snd group})

    let combine basePath target =
        System.IO.Path.Combine(basePath, target)

    let private mapWithFullPath basePath (fileGroup:PicturesPerMonth) =
        let {MonthYear=month; Pictures=pictures} = fileGroup
        let target =
            month
            |> combine basePath

        {MonthYear=target; Pictures=pictures}

    let mapTargetPath basePath (picturesPerMonth:seq<PicturesPerMonth>) =
        picturesPerMonth
        |> Seq.map (fun files -> mapWithFullPath basePath files)

    let getOldFilesByMonth sourcePath =
        sourcePath
        |> allFilesInPath
        |> onlyOldFiles
        |> groupByMonth
