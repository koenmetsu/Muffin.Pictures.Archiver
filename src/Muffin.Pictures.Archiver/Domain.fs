namespace Muffin.Pictures.Archiver

open System

[<AutoOpen>]
module Domain =

    type FilePath = string

    type File = {FullPath:FilePath; Name:string}

    type TimeTakenMode =
    | Strict
    | Fallback

    type TimeTaken = DateTimeOffset

    type Picture = {File:File; TakenOn:TimeTaken} with
        member this.formatTakenOn : string =
            sprintf "%i-%02i" this.TakenOn.Year this.TakenOn.Month

    type MoveRequest = {Source:FilePath; Destination:FilePath}

    type FailureReason =
    | BytesDidNotMatch
    | CouldNotCopyFile of string
    | CouldNotDeleteSource of string

    type FailedMove<'entity> = {Request:'entity; Reason: FailureReason}
    type SuccessfulMove = {Request:MoveRequest}

    type RunnerArguments = {SourceDir: string; DestinationDir: string; Mode: TimeTakenMode; MailTo : string option}

    let notNullOrEmpty = not << System.String.IsNullOrEmpty
