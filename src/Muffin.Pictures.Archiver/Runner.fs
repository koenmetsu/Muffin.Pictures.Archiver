namespace Muffin.Pictures.Archiver

open System

open Muffin.Pictures.Archiver.Moves
open Muffin.Pictures.Archiver.Rop
open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Reporter

module Runner =

    let moveRequests getMoveRequests arguments =
        getMoveRequests arguments.SourceDir arguments.DestinationDir

    let move (moveWithFs:MoveRequest -> Result<MoveRequest>) (compareFiles:MoveRequest -> Result<MoveRequest>) (cleanUp:MoveRequest -> Result<MoveRequest>) =
        moveWithFs
        >=> compareFiles
        >=> cleanUp

    let runner move moveRequests =
        moveRequests
            |> Seq.map move
            |> List.ofSeq
            |> report
            |> ignore
