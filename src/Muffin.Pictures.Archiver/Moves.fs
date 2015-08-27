namespace Muffin.Pictures.Archiver

open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Paths
open Muffin.Pictures.Archiver.Rop

module Moves =
    let private getMoveRequest (picture: Picture) basePath =
        let destinationFolder =
            pathCombine basePath picture.formatTakenOn

        let destination =
            pathCombine destinationFolder picture.File.Name

        {Source=picture.File.FullPath; Destination=destination}

    let getMoveRequests getPictures sourceDir destinationDir =
        let toMoveRequest picture =
            match picture with
            | Success pic ->
                let moveRequest = getMoveRequest pic destinationDir
                Success moveRequest
            | Failure f -> Failure f

        getPictures sourceDir
        |> List.map toMoveRequest
