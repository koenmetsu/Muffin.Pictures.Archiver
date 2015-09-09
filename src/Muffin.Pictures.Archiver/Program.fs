open System

open Muffin.Pictures.Archiver.Arguments
open Muffin.Pictures.Archiver.CompositionRoot
open Muffin.Pictures.Archiver.Runner

open System.Diagnostics

[<EntryPoint>]
let main argv =
    let arguments = parseArguments argv

    let move = composeMove
    let getMoveRequests = composeGetMoveRequests arguments

    runner move getMoveRequests arguments

    0
