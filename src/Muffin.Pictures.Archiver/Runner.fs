namespace Muffin.Pictures.Archiver

open System

open Muffin.Pictures.Archiver.Moves
open Muffin.Pictures.Archiver.Rop
open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Reporter

module Runner =

    let runner (moveWithFs:MoveRequest -> Result<MoveRequest>) (compareFiles:MoveRequest -> Result<MoveRequest>) (cleanUp:MoveRequest -> Result<MoveRequest>) getMoveRequests arguments =

        let moveIt =
            moveWithFs
            >=> compareFiles
            >=> cleanUp

        getMoveRequests arguments.SourceDir arguments.DestinationDir
            |> Seq.map moveIt
            |> List.ofSeq
            |> report
            |> ignore
