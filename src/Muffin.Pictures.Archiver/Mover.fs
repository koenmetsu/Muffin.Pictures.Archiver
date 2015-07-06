namespace Muffin.Pictures.Mover

open Muffin.Pictures.Archiver.Files
open System.IO

module Mover =

    let private createDirectory destination =
        let fileInfo = System.IO.FileInfo destination
        if not (fileInfo.Directory.Exists) then
            Directory.CreateDirectory(fileInfo.Directory.FullName) |> ignore

    let private moveFile {Source=source; Destination=destination} =
        createDirectory destination
        File.Move(source, destination)

    let move (moves:seq<Move>) =
        moves
        |> Seq.iter (fun move -> moveFile move)
