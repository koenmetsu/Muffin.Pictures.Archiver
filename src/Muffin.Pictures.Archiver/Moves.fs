namespace Muffin.Pictures.Archiver

open Domain
open Paths

module Moves =
    let private getMoveRequest (picture: Picture) basePath =
        let destinationFolder =
            pathCombine basePath picture.formatTakenOn

        let destination =
            pathCombine destinationFolder picture.File.Name

        {Source=picture.File.FullPath; Destination=destination}

    let getMoveRequests (getPictures:string -> seq<Picture>) sourceDir destinationDir =
        getPictures sourceDir
        |> Seq.map (fun picture -> getMoveRequest picture destinationDir)
