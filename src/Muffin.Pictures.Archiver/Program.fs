open System

open Muffin.Pictures.Archiver.Arguments
open Muffin.Pictures.Archiver.CompositionRoot
open Muffin.Pictures.Archiver.Runner
open Muffin.Pictures.Archiver.Rop
open Muffin.Pictures.Archiver.TagRetriever

open Serilog
open Serilog.Sinks
open Serilog.Sinks.Elasticsearch
open Serilog.Exceptions


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

    let loggerConfig =
        LoggerConfiguration()
            .Enrich.WithExceptionDetails()
            .WriteTo.RollingFile("log-{Date}.log")

    if arguments.ElasticUrl |> Option.isSome then
        Option.get(arguments.ElasticUrl)
        |> ElasticsearchSinkOptions
        |> tee (fun o -> o.AutoRegisterTemplate <- true)
        |> tee (fun o -> o.BufferBaseFilename <- "./logs/buffer")
        |> loggerConfig.WriteTo.Elasticsearch
        |> ignore

    Log.Logger <- loggerConfig.CreateLogger()

    let move = composeMove
    let getMoveRequests = composeGetMoveRequests arguments exifFile DateTimeOffset.UtcNow

    runner move getMoveRequests arguments

    0

