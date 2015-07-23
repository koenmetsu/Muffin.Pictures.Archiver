namespace Muffin.Pictures.Archiver

open System.IO
open Domain
open Paths
open Moves

module FileMover =

    let private createDirectory destination =
        let fileInfo = FileInfo destination
        if not (fileInfo.Directory.Exists) then
            Directory.CreateDirectory(fileInfo.Directory.FullName) |> ignore

    let private moveFile {Source=source; Destination=destination} =
        createDirectory destination
        File.Move(source, destination)

    let move targetPath pictures =
        pictures
        |> getMoves targetPath
        |> Seq.iter moveFile