namespace Muffin.Pictures.Archiver

open Nessos.UnionArgParser

open Muffin.Pictures.Archiver.Domain

module Arguments =

    type private Arguments =
        | [<Mandatory>][<AltCommandLine("-s")>] SourceDir of string
        | [<Mandatory>][<AltCommandLine("-d")>] DestinationDir of string
    with
        interface IArgParserTemplate with
            member s.Usage =
                match s with
                | SourceDir _ -> "Specify a source directory"
                | DestinationDir _ -> "Specify a destination directory"

    let parseArguments argv =
        let parser = UnionArgParser.Create<Arguments>()
        let arguments = parser.Parse argv

        let sourceDir = arguments.GetResult <@ SourceDir @>
        let destinationDir = arguments.GetResult <@ DestinationDir @>

        {SourceDir = sourceDir; DestinationDir = destinationDir}