open System

open Muffin.Pictures.Archiver.Arguments
open Muffin.Pictures.Archiver.CompositionRoot
open Muffin.Pictures.Archiver.Runner
open Muffin.Pictures.Archiver.Rop
open Muffin.Pictures.Archiver.TagRetriever
open Muffin.Pictures.Archiver.Logging

open Serilog


[<EntryPoint>]
let main argv =

    System.AppDomain
            .CurrentDomain
            .UnhandledException.Add(
                fun exc ->
                    let excObject = exc.ExceptionObject :?> System.Exception
                    Log.Fatal(excObject, "Unhandled exception: {ExceptionMessage}", excObject.Message)
            )

    let arguments = parseArguments argv

    Log.Logger <- createLogger arguments

    let move = composeMove
    let getMoveRequests = composeGetMoveRequests arguments exifFile DateTimeOffset.UtcNow

    runner move getMoveRequests arguments

    0

