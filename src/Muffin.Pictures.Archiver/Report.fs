namespace Muffin.Pictures.Archiver

open Muffin.Pictures.Archiver.Rop

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
