namespace Muffin.Pictures.Archiver

open Muffin.Pictures.Archiver.Rop
open Muffin.Pictures.Archiver.Domain

module Report =
    type Report = {Successes : MoveRequest list; Failures : FailedMove<MoveRequest> list}

    let private isSuccess move =
        match move with
        | Success success -> Some success
        | _ -> None

    let private isFailure move =
        match move with
        | Failure f -> Some f
        | _ -> None

    let createReport moves =
        let successes = moves |> List.choose (isSuccess)
        let failures = moves |> List.choose (isFailure)
        {Successes = successes; Failures = failures}

    let formatSuccess request =
        sprintf "%s -> %s" request.Source request.Destination

    let formatFailure {Request = request; Reason = failure} =
        match failure with
        | BytesDidNotMatch ->
            sprintf "Reason: Bytes did not match in source and destination.\n%s -> %s" request.Source request.Destination
        | CouldNotCopyFile msg ->
            sprintf "Reason: Could not copy file: %s.\n%s -> %s" msg request.Source request.Destination
        | CouldNotDeleteSource msg ->
            sprintf "Reason: Could not delete source file: %s.\n%s -> %s" msg request.Source request.Destination

    let reportTo report writer =
        let reportOrNone onAny results =
            match results with
            | [] -> writer "None"
            | xs ->
                xs |> List.iter (onAny >> writer)

        writer "Successes:"
        writer (sprintf "%i files were successfully moved." report.Successes.Length)
        reportOrNone formatSuccess report.Successes
        writer ""
        writer "Failures:"
        writer (sprintf "%i files could not be moved." report.Successes.Length)
        reportOrNone formatFailure report.Failures
        writer "Done archiving!"
