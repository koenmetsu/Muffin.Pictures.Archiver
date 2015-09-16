namespace Muffin.Pictures.Archiver

open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.ConsoleReporter
open Muffin.Pictures.Archiver.Report
open Muffin.Pictures.Archiver.MailReporter
open Muffin.Pictures.Archiver.Rop

module Runner =

    let runner move getMoveRequests arguments =
        let runInParallel =
            let asyncMove request = async { return move request }

            List.map asyncMove
            >> Async.Parallel
            >> Async.RunSynchronously
            >> List.ofArray

        let reportToMailIfNecessary report =
            if arguments.MailTo.IsSome then
                reportToMail report arguments.MailTo.Value

        let watch = System.Diagnostics.Stopwatch()

        let moveRequests =
            getMoveRequests arguments.SourceDir arguments.DestinationDir

        watch.Start()
        moveRequests
            |> List.choose isSuccess
            |> runInParallel
            |> tee (fun _ -> watch.Stop())
            |> createReport moveRequests
            |> tee reportToConsole
            |> reportToMailIfNecessary

        System.Console.WriteLine watch.ElapsedMilliseconds