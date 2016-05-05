namespace Muffin.Pictures.Archiver

open Nessos.UnionArgParser

open Muffin.Pictures.Archiver.Domain

module Arguments =

    type private Arguments =
        | [<Mandatory>][<AltCommandLine("-s")>] SourceDir of string
        | [<Mandatory>][<AltCommandLine("-d")>] DestinationDir of string
        | Fallback
        | MailTo of string
        | ElasticUrl of string
    with
        interface IArgParserTemplate with
            member s.Usage =
                match s with
                | SourceDir _ -> "Specify a source directory"
                | DestinationDir _ -> "Specify a destination directory"
                | Fallback _ -> "Specify whether or not to fallback on last date modified if no \"Date Taken\" tag was found"
                | MailTo _ -> "Specify whether or not to send an email (use system.net.mailsettings in config file to specify sender)"
                | ElasticUrl _ -> "Specify the elasticsearch url if you want to report to elasticsearch"

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
        let mailTo = arguments.TryGetResult <@ MailTo @>
        let elasticUrl =
            match arguments.TryGetResult <@ ElasticUrl @> with
            | Some uri -> Some (new System.Uri(uri))
            | None -> None

        { SourceDir = sourceDir; DestinationDir = destinationDir; Mode = mode; MailTo = mailTo; ElasticUrl = elasticUrl }
