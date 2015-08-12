open System

open Muffin.Pictures.Archiver.Pictures
open Muffin.Pictures.Archiver.FileMover
open Muffin.Pictures.Archiver.Moves
open Muffin.Pictures.Archiver.Paths
open Muffin.Pictures.Archiver.Files
open Muffin.Pictures.Archiver.TimeTakenRetriever
open Muffin.Pictures.Archiver.Age
open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.FileSystem
open Muffin.Pictures.Archiver.Runner
open Muffin.Pictures.Archiver.Arguments


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

    let getPictures = composeGetPictures
    let moveWithFs = composeMoveWithFs

    let arguments = parseArguments argv

    runner arguments getPictures moveWithFs

    0
