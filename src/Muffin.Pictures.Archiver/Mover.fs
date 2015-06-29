namespace Muffin.Pictures.Mover

open Muffin.Pictures.Archiver.Files

module Mover =
    let private moveFile targetBase (picture:Picture) =
        let targetFull = combine targetBase picture.Name
        System.IO.Directory.CreateDirectory(targetBase)
        |> ignore
        System.IO.File.Move(picture.Name, targetFull)

    let private moveFiles {Pictures=pictures; MonthYear=takenOn} =
        let targetBase = takenOn
        pictures
        |> Seq.iter (fun file -> moveFile targetBase file)

    let move (files:seq<PicturesPerMonth>) =
        files
        |> Seq.iter (fun fileGroup -> moveFiles fileGroup)
