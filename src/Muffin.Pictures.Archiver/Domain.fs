namespace Muffin.Pictures.Archiver

open System

[<AutoOpen>]
module Domain =

    type File = {FullPath:string; Name:string}

    type TimeTaken = DateTimeOffset

    type Picture = {File:File; TakenOn:TimeTaken} with
        member this.formatTakenOn : string =
            sprintf "%i-%02i" this.TakenOn.Year this.TakenOn.Month

    type MoveRequest = {Source:string; Destination:string}

    type FailureReason =
    | BytesDidNotMatch

    type FailedMove = {Request:MoveRequest; Reason: FailureReason}
    type SuccessfulMove = {Request:MoveRequest}

    type Move =
    | SuccessfulMove of SuccessfulMove
    | FailedMove of FailedMove

    type RunnerArguments = {SourceDir:string; DestinationDir:string}