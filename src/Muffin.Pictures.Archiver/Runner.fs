namespace Muffin.Pictures.Archiver

open System

open Muffin.Pictures.Archiver.Pictures
open Muffin.Pictures.Archiver.FileMover
open Muffin.Pictures.Archiver.Moves
open Muffin.Pictures.Archiver.Paths
open Muffin.Pictures.Archiver.Files
open Muffin.Pictures.Archiver.TimeTakenRetriever
open Muffin.Pictures.Archiver.Age
open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Arguments

module Runner =

    let printResult (moveResult:MoveResult) =
        printf "Move: %A" moveResult
        ignore()

    let runner (arguments:RunnerArguments) getPictures moveWithFs =

        let moves =
                allFilesInPath arguments.SourceDir
                |> getPictures
                |> getMoveRequests arguments.DestinationDir
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

