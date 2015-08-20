namespace Muffin.Pictures.Archiver

open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Report

open System

module ConsoleReporter =

    let reportToConsole report =

        let printSuccess request =
            printfn "%s -> %s" request.Source request.Destination

        let failureStr {Request = request; Reason = failure} =
            match failure with
            | BytesDidNotMatch ->
                sprintf "Reason: Bytes did not match in source and destination.\n%s -> %s" request.Source request.Destination
            | CouldNotCopyFile msg ->
                sprintf "Reason: Could not copy file: %s.\n%s -> %s" msg request.Source request.Destination
            | CouldNotDeleteSource msg ->
                sprintf "Reason: Could not delete source file: %s.\n%s -> %s" msg request.Source request.Destination

        let consoleWrite text =
            printfn "%s" text

        let printFailure = failureStr >> consoleWrite

        printfn "Successes:"
        report.Successes
        |> List.iter printSuccess

        printfn ""

        printfn "Failures:"
        match report.Failures with
        | [] -> printfn "None"
        | xs ->
            xs
            |> List.iter printFailure


        printfn "Done archiving!"
