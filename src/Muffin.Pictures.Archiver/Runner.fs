namespace Muffin.Pictures.Archiver

open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.ConsoleReporter
open Muffin.Pictures.Archiver.Report
open Muffin.Pictures.Archiver.MailReporter

module Runner =

    let runner move getMoveRequests arguments =
        let moveRequests =
            getMoveRequests arguments.SourceDir arguments.DestinationDir
        let watch = System.Diagnostics.Stopwatch()
        watch.Start()
        let asyncMove request = async { return move request }
        let successfulMoveRequests = moveRequests |> List.choose isSuccess

        let moveResults =
            successfulMoveRequests
            |> List.map asyncMove
            |> Async.Parallel
            |> Async.RunSynchronously
            |> List.ofArray

        watch.Stop()

        let report = createReport moveRequests moveResults

        reportToConsole report
        System.Console.WriteLine watch.ElapsedMilliseconds
        if arguments.MailTo.IsSome then
            reportToMail report arguments.MailTo.Value

