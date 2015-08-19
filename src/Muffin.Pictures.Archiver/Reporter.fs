namespace Muffin.Pictures.Archiver

open Muffin.Pictures.Archiver.Rop

open System

module Reporter =
    let private isSuccess move =
        match move with
        | Success success -> Some success
        | _ -> None

    let private isFailure move =
        match move with
        | Failure f -> Some f
        | _ -> None

    let report (moves:Result<MoveRequest> list) =
        let successes = moves |> List.choose (isSuccess)
        let failures = moves |> List.choose (isFailure)

        let printSuccess request =
            printfn "%s -> %s" request.Source request.Destination

        let printFailure {Request = request; Reason = failure} =
            match failure with
            | BytesDidNotMatch ->
                printfn "Reason: Bytes did not match in source and destination."
                printfn "%s -> %s" request.Source request.Destination
            | CouldNotCopyFile msg ->
                printfn "Reason: Could not copy file: %s." msg
                printfn "%s -> %s" request.Source request.Destination
            | CouldNotDeleteSource msg ->
                printfn "Reason: Could not delete source file: %s." msg
                printfn "%s -> %s" request.Source request.Destination

        printfn "Successes:"
        successes
        |> List.iter printSuccess

        printfn ""

        printfn "Failures:"
        match failures with
        | [] -> printfn "None"
        | xs -> xs |> List.iter printFailure


        Console.WriteLine("Done archiving!") |> ignore
