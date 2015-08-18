namespace Muffin.Pictures.Archiver

open Muffin.Pictures.Archiver.Domain

module Rop =
    type Result<'a> =
    | Success of 'a
    | Failure of FailedMove<'a>

    let fail request reason =
        Failure {FailedMove.Request = request; FailedMove.Reason = reason}

    let bind switchFunction =
        function
        | Success s -> switchFunction s
        | Failure f -> Failure f

    let (>>=) twoTrackInput switchFunction =
        bind switchFunction twoTrackInput

    let (>=>) switch1 switch2 x =
        match switch1 x with
        | Success s -> switch2 s
        | Failure f -> Failure f
