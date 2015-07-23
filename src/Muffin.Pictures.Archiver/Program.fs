open System
open Nessos.UnionArgParser
open Muffin.Pictures.Archiver.Pictures
open Muffin.Pictures.Archiver.FileMover
open Muffin.Pictures.Archiver.Paths
open Muffin.Pictures.Archiver.Files
open Muffin.Pictures.Archiver.TimeTakenRetriever
open Muffin.Pictures.Archiver.Age
open Muffin.Pictures.Archiver.Domain


type Arguments =
        | [<Mandatory>][<AltCommandLine("-s")>] SourceDir of string
        | [<Mandatory>][<AltCommandLine("-d")>] DestinationDir of string
with
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | SourceDir _ -> "Specify a source directory"
            | DestinationDir _ -> "Specify a destination directory"

[<EntryPoint>]
let main argv =

    let parser = UnionArgParser.Create<Arguments>()
    let arguments = parser.Parse argv
    let sourceDir = arguments.GetResult <@ SourceDir @>
    let destinationDir = arguments.GetResult <@ DestinationDir @>

    let timeProvider () = DateTimeOffset.UtcNow
    let fileProvider = allFilesInPath sourceDir

    let getPictures = getOldPictures timeTaken timeProvider fileProvider

    getPictures
        |> move destinationDir
        |> ignore

    Console.WriteLine("Done archiving!") |> ignore

    0
