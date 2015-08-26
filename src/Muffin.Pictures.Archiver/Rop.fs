namespace Muffin.Pictures.Archiver

module Rop =
    type Result<'success, 'failure> =
    | Success of 'success
    | Failure of 'failure

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
