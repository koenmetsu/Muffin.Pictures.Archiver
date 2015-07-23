namespace Muffin.Pictures.Archiver

open Domain
open Paths

module Moves =
    let private getMove basePath (picture: Picture) =
        let destinationFolder =
            pathCombine basePath picture.formatTakenOn

        let destination =
            pathCombine destinationFolder picture.File.Name

        {Source=picture.File.FullPath; Destination=destination}

    let getMoves basePath (pictures:seq<Picture>) =
        pictures
        |> Seq.map (fun picture -> getMove basePath picture)