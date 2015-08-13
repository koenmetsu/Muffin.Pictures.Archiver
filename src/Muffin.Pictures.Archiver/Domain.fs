namespace Muffin.Pictures.Archiver

open System

[<AutoOpen>]
module Domain =

    type FilePath = string

    type File = {FullPath:FilePath; Name:string}

    type TimeTaken = DateTimeOffset

    type Picture = {File:File; TakenOn:TimeTaken} with
        member this.formatTakenOn : string =
            sprintf "%i-%02i" this.TakenOn.Year this.TakenOn.Month

    type MoveRequest = {Source:FilePath; Destination:FilePath}

    type FailureReason =
    | BytesDidNotMatch

    type FailedMove = {Request:MoveRequest; Reason: FailureReason}
    type SuccessfulMove = {Request:MoveRequest}

    type Move =
    | SuccessfulMove of SuccessfulMove
    | FailedMove of FailedMove

    type RunnerArguments = {SourceDir:string; DestinationDir:string}