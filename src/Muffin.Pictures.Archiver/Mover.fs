namespace Muffin.Pictures.Archiver

open System.IO
open Domain
open Pictures
open Paths

module Mover =

    let private getMove basePath (picture: Picture) =
        let destinationFolder =
            pathCombine basePath picture.formatTakenOn

        let destination =
            pathCombine destinationFolder picture.File.Name

        {Source=picture.File.FullPath; Destination=destination}

    let private getMoves basePath (picturesPerMonth:seq<Picture>) =
        picturesPerMonth
        |> Seq.map (fun pictures -> getMove basePath pictures)

    let private createDirectory destination =
        let fileInfo = FileInfo destination
        if not (fileInfo.Directory.Exists) then
            Directory.CreateDirectory(fileInfo.Directory.FullName) |> ignore

    let private moveFile {Source=source; Destination=destination} =
        createDirectory destination
        File.Move(source, destination)

    let move targetPath picturesPerMonth =
        picturesPerMonth
        |> getMoves targetPath 
        |> Seq.iter moveFile