namespace Muffin.Pictures.Archiver

open Muffin.Pictures.Archiver.Rop
open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.ConsoleReporter
open Muffin.Pictures.Archiver.Report
open Muffin.Pictures.Archiver.MailReporter

module Runner =

    let moveRequests getMoveRequests arguments =
        getMoveRequests arguments.SourceDir arguments.DestinationDir

    let move (moveWithFs:MoveRequest -> Result<MoveRequest>) (compareFiles:MoveRequest -> Result<MoveRequest>) (cleanUp:MoveRequest -> Result<MoveRequest>) =
        moveWithFs
        >=> compareFiles
        >=> cleanUp

    let runner move getMoveRequests arguments =
        let report =
            getMoveRequests arguments.SourceDir arguments.DestinationDir
            |> List.map move
            |> createReport

        reportToConsole report

        if arguments.MailTo.IsSome then
            reportToMail report arguments.MailTo.Value
