namespace Muffin.Pictures.Archiver

open System

[<AutoOpen>]
module Domain =

    type File = {FullPath:string; Name:string}

    type TimeTaken = DateTimeOffset

    type Picture = {File:File; TakenOn:TimeTaken} with
        member this.formatTakenOn : string =
            sprintf "%i-%02i" this.TakenOn.Year this.TakenOn.Month

    type Move = {Source:string; Destination:string}
