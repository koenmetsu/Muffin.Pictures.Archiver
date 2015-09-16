open NLog

open Muffin.Pictures.Archiver.Arguments
open Muffin.Pictures.Archiver.CompositionRoot
open Muffin.Pictures.Archiver.Runner


[<EntryPoint>]
let main argv =
    let logger = LogManager.GetCurrentClassLogger()

    logger.Info "Starting Archiver"
    let arguments = parseArguments argv

    let move = composeMove
    let getMoveRequests = composeGetMoveRequests arguments

    runner move getMoveRequests arguments

    0
