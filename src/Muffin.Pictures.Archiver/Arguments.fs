namespace Muffin.Pictures.Archiver

open Nessos.UnionArgParser

open Muffin.Pictures.Archiver.Domain
open System

module Arguments =

    type private Arguments =
        | [<Mandatory>][<AltCommandLine("-s")>] SourceDir of string
        | [<Mandatory>][<AltCommandLine("-d")>] DestinationDir of string
        | Fallback
    with
        interface IArgParserTemplate with
            member s.Usage =
                match s with
                | SourceDir _ -> "Specify a source directory"
                | DestinationDir _ -> "Specify a destination directory"
                | Fallback _ -> "Specify whether or not to fallback on last date modified if no \"Date Taken\" tag was found"

    let parseArguments argv =
        let parser = UnionArgParser.Create<Arguments>()
        let arguments = parser.Parse argv

        let sourceDir = arguments.GetResult <@ SourceDir @>
        let destinationDir = arguments.GetResult <@ DestinationDir @>
        let mode =
            if arguments.Contains <@ Fallback @> then
                TimeTakenMode.Fallback
            else
                TimeTakenMode.Strict

        {SourceDir = sourceDir; DestinationDir = destinationDir; Mode = mode}
