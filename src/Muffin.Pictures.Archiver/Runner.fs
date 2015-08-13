namespace Muffin.Pictures.Archiver

open System

open Muffin.Pictures.Archiver.Moves
open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Reporter

module Runner =

    let runner getMoveRequests moveWithFs cleanUp arguments =

        let moves =
            getMoveRequests arguments.SourceDir arguments.DestinationDir
            |> Seq.map moveWithFs
            |> List.ofSeq

        moves |> Seq.iter cleanUp

        moves
            |> report
            |> ignore
