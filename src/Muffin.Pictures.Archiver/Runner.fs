namespace Muffin.Pictures.Archiver

open System

open Muffin.Pictures.Archiver.Moves
open Muffin.Pictures.Archiver.Files
open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Reporter

module Runner =

    let runner arguments getPictures moveWithFs deleteSource =

        let moves =
            allFilesInPath arguments.SourceDir
            |> getPictures
            |> getMoveRequests arguments.DestinationDir
            |> Seq.map moveWithFs
            |> List.ofSeq

        let cleanUp move =
            match move with
            | SuccessfulMove success -> deleteSource success.Request
            | FailedMove failure -> ignore()

        moves |> Seq.iter cleanUp

        moves
            |> report
            |> ignore
