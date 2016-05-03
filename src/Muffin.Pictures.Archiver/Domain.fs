namespace Muffin.Pictures.Archiver

open System

[<AutoOpen>]
module Domain =

    type DirectoryPath = string
    type FilePath = string

    type File = { FullPath:FilePath; Name:string }

    type TimeTakenMode =
        | Strict
        | Fallback

    type TimeTaken = DateTimeOffset

    type Picture = { File:File; TakenOn:TimeTaken } with
        member this.formatTakenOn : string =
            sprintf "%i-%02i" this.TakenOn.Year this.TakenOn.Month

    type MoveRequest = { Source:FilePath; Destination:FilePath; TimeTaken: TimeTaken }
    type FailedMove = { Request:MoveRequest; Message : string }

    type Skip =
        | FileHasNoTimeTaken of File
        | PictureWasNotOldEnough of Picture

    type Failure =
        | BytesDidNotMatch of MoveRequest
        | CouldNotCopyFile of FailedMove
        | CouldNotDeleteSource of FailedMove

    type RunnerArguments = { SourceDir: string; DestinationDir: string; Mode: TimeTakenMode; MailTo : string option }

    let notNullOrEmpty = not << System.String.IsNullOrEmpty
