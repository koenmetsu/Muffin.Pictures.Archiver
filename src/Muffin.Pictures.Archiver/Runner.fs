namespace Muffin.Pictures.Archiver

open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.ConsoleReporter
open Muffin.Pictures.Archiver.Report
open Muffin.Pictures.Archiver.MailReporter
open Muffin.Pictures.Archiver.Rop
open Serilog

module Runner =
    open ElasticReporter


    let private moveInParallel move =
        let asyncMove (request:MoveRequest) = async {
            return move request
                |> tee (fun _ -> Log.Information("Moved Request: {@Request}", request))
        }

        List.map asyncMove
        >> Async.Parallel
        >> Async.RunSynchronously
        >> List.ofArray

    let private reportToMailIfNecessary arguments report =
        if arguments.MailTo.IsSome then
            reportFailuresToMail report arguments.MailTo.Value

    let runner move getMoveRequests arguments =
        let watch = System.Diagnostics.Stopwatch()

        let moveRequests =
            getMoveRequests arguments.SourceDir arguments.DestinationDir

        watch.Start()
        moveRequests
            |> List.choose isSuccess
            |> moveInParallel move
            |> tee (fun _ -> watch.Stop())
            |> createReport moveRequests
            |> tee reportToConsole
            |> tee (reportToElastic arguments.ElasticUrl)
            |> reportToMailIfNecessary arguments

        Log.Verbose("Time elapsed: {TimeInMilliseconds}", watch.ElapsedMilliseconds)
