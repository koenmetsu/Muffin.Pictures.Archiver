namespace Muffin.Pictures.Archiver

open Muffin.Pictures.Archiver.Rop
open Muffin.Pictures.Archiver.Domain

module Report =
    type Report = { Skips : Skip list; Successes : MoveRequest list; Failures : Failure list }

    let isSuccess move =
        match move with
        | Success success -> Some success
        | _ -> None

    let isFailure move =
        match move with
        | Failure f -> Some f
        | _ -> None

    let createReport requests moves =
        let skips = requests |> List.choose isFailure
        let successes = moves |> List.choose isSuccess
        let failures = moves |> List.choose isFailure

        { Skips = skips; Successes = successes; Failures = failures }

    let formatSkippedFile skipReason =
        match skipReason with
        | FileHasNoTimeTaken file ->
            sprintf "Reason: File has no time taken.\n\t%s" file.Name
        | PictureWasNotOldEnough pic ->
            sprintf "Reason: Picture was not old enough.\n\t%s at %s" pic.File.Name (pic.TakenOn.ToString("yyyy-MM-dd"))

    let formatSuccess request =
        sprintf "\t%s -> %s" request.Source request.Destination

    let formatFailure failure =
        match failure with
        | BytesDidNotMatch request ->
            sprintf "Reason: Bytes did not match in source and destination.\n\t%s -> %s" request.Source request.Destination
        | CouldNotCopyFile { Request = request; Message = msg } ->
            sprintf "Reason: Could not copy file: %s.\n\t%s -> %s" msg request.Source request.Destination
        | CouldNotDeleteSource { Request = request; Message = msg } ->
            sprintf "Reason: Could not delete source file: %s.\n\t%s -> %s" msg request.Source request.Destination

    let reportTo report writer =
        let reportOrNone onAny results =
            match results with
            | [] -> ignore ()
            | xs ->
                xs |> List.iter (onAny >> writer)

        writer "Skipped files:"
        writer (sprintf "%i files were skipped." report.Skips.Length)
        reportOrNone formatSkippedFile report.Skips
        writer "Successes:"
        writer (sprintf "%i files were successfully moved." report.Successes.Length)
        reportOrNone formatSuccess report.Successes
        writer ""
        writer "Failures:"
        writer (sprintf "%i files could not be moved." report.Failures.Length)
        reportOrNone formatFailure report.Failures
        writer "Done archiving!"
