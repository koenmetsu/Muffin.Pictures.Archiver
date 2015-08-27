namespace Muffin.Pictures.Archiver

open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.ConsoleReporter
open Muffin.Pictures.Archiver.Report
open Muffin.Pictures.Archiver.MailReporter

module Runner =

    let runner move getMoveRequests arguments =
        let moveRequests = getMoveRequests arguments.SourceDir arguments.DestinationDir
        let moveResults =
            moveRequests
            |> List.choose isSuccess
            |> List.map move

        let report = createReport moveRequests moveResults

        reportToConsole report

        if arguments.MailTo.IsSome then
            reportToMail report arguments.MailTo.Value
