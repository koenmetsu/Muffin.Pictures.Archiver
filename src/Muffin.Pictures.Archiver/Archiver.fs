namespace Muffin.Pictures.Archiver

open System.IO
open System
open TimeTakenRetriever

module Files =

    type Picture = {FullPath:string; Name:string; TakenOn:DateTimeOffset}
    type PictureWithMonth = {Picture:Picture; MonthYear:string}
    type PicturesPerMonth = {MonthYear:string; Pictures:Picture seq}
    type Move = {Source:string; Destination:string}

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

    let private toPictureWithMonth pictures =
        pictures
        |> Seq.map (fun picture -> {Picture=picture; MonthYear=getMonthYear picture})

    let pathCombine basePath target =
        System.IO.Path.Combine(basePath, target)

    let private getMove basePath {MonthYear=monthYear; Picture=picture} =
        let destinationFolder =
            pathCombine basePath monthYear

        let destination =
            pathCombine destinationFolder picture.Name

        {Source=picture.FullPath; Destination=destination}

    let getMoves basePath (picturesPerMonth:seq<PictureWithMonth>) =
        picturesPerMonth
        |> Seq.map (fun pictures -> getMove basePath pictures)

    let getOldPicturesWithMonth sourcePath =
        sourcePath
        |> allFilesInPath
        |> onlyOldFiles
        |> toPictureWithMonth
