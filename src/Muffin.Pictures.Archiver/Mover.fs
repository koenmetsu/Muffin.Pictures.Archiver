namespace Muffin.Pictures.Mover

open System.IO
open Muffin.Pictures.Archiver.Files

module Mover =
    let private moveFile targetBase (file:FileInfo) =
        let targetFull = combine targetBase file.Name
        System.IO.Directory.CreateDirectory(targetBase)
        |> ignore
        System.IO.File.Move(file.FullName, targetFull)

    let private moveFiles (fileGroup:FilesPerMonth) =
        let targetBase = fst fileGroup
        snd fileGroup
        |> Seq.iter (fun file -> moveFile targetBase file)

    let move (files:seq<FilesPerMonth>) =
        files
        |> Seq.iter (fun fileGroup -> moveFiles fileGroup)
