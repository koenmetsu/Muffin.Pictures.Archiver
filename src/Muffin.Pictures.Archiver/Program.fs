open System
open Nessos.UnionArgParser
open Muffin.Pictures.Archiver.Pictures
open Muffin.Pictures.Archiver.FileMover
open Muffin.Pictures.Archiver.Moves
open Muffin.Pictures.Archiver.Paths
open Muffin.Pictures.Archiver.Files
open Muffin.Pictures.Archiver.TimeTakenRetriever
open Muffin.Pictures.Archiver.Age
open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.FileSystem


type Arguments =
        | [<Mandatory>][<AltCommandLine("-s")>] SourceDir of string
        | [<Mandatory>][<AltCommandLine("-d")>] DestinationDir of string
with
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | SourceDir _ -> "Specify a source directory"
            | DestinationDir _ -> "Specify a destination directory"

let composeMoveWithFs =
    let fsOperations = FileSystemOperations
    let ensureDirectoryExists = ensureDirectoryExists fsOperations.DirectoryExists fsOperations.CreateDirectory
    let copy source destination overwrite = fsOperations.Copy(source, destination, overwrite)
    let copyToDestination moveRequest = copyToDestination ensureDirectoryExists copy moveRequest
    let compareFiles moveRequest = compareFiles fsOperations.ReadAllBytes moveRequest
    let deleteSource moveRequest = fsOperations.Delete moveRequest.Source

    moveFile copyToDestination compareFiles deleteSource

let composeGetPictures =
    let timeProvider () = DateTimeOffset.UtcNow

    getOldPictures timeTaken timeProvider

[<EntryPoint>]
let main argv =

    let parser = UnionArgParser.Create<Arguments>()
    let arguments = parser.Parse argv
    let sourceDir = arguments.GetResult <@ SourceDir @>
    let destinationDir = arguments.GetResult <@ DestinationDir @>


    let getPictures = composeGetPictures
    let moveWithFs = composeMoveWithFs

    let printResult (moveResult:MoveResult) =
        printf "Move: %A" moveResult
        ignore()

    let moves =
        allFilesInPath sourceDir
        |> getPictures
        |> getMoveRequests destinationDir
        |> Seq.map moveWithFs
        |> List.ofSeq

    let isSuccess move = match move.Result with
                           | Success -> Some move
                           | _ -> None
    let isFailure move = match move.Result with
                         | Failure f -> Some move
                         | _ -> None

    let successes = moves |> List.choose (isSuccess)
    let failures = moves |> List.choose (isFailure)

    printfn "Successes:"
    successes
    |> List.iter (fun {Request = request; Result = _ } ->
                    printfn "%s -> %s" request.Source request.Destination)

    printfn ""

    printfn "Failures:"
    match failures with
    | [] -> printfn "None"
    | xs -> xs
                |> List.iter (fun {Request = request; Result = result} ->
                                match result with
                                 | Failure failure -> match failure with
                                                      | BytesDidNotMatch -> printfn "Reason: Bytes did not match in source and destination."
                                                                            printfn "%s -> %s" request.Source request.Destination)
                                 | _ -> ignore()

    Console.WriteLine("Done archiving!") |> ignore

    0
