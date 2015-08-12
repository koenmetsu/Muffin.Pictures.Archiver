namespace Muffin.Pictures.Archiver

open System

module Reporter =
    let private isSuccess move = match move with
                                 | SuccessfulMove success -> Some success
                                 | _ -> None

    let private isFailure move = match move with
                                 | FailedMove f -> Some f
                                 | _ -> None

    let report moves =
        let successes = moves |> List.choose (isSuccess)
        let failures = moves |> List.choose (isFailure)

        let printSuccess (success:SuccessfulMove) =
            let request = success.Request
            printfn "%s -> %s" request.Source request.Destination

        let printFailure {Request = request; Reason = failure} =
            match failure with
            | BytesDidNotMatch -> printfn "Reason: Bytes did not match in source and destination."
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
