open System

open NLog

open Muffin.Pictures.Archiver.Arguments
open Muffin.Pictures.Archiver.CompositionRoot
open Muffin.Pictures.Archiver.Runner


[<EntryPoint>]
let main argv =
    let logger = LogManager.GetCurrentClassLogger()

    System.AppDomain.CurrentDomain.UnhandledException.Add(fun exc ->
        let excObject = exc.ExceptionObject :?> System.Exception
        logger.Fatal(excObject, (sprintf "Unhandled exception: %s" excObject.Message)))

    logger.Info "Starting Archiver"
    let arguments = parseArguments argv

    let move = composeMove
    let getMoveRequests = composeGetMoveRequests arguments

    runner move getMoveRequests arguments

    0
