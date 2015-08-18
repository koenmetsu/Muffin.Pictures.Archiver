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
open Muffin.Pictures.Archiver.Rop

let fsOperations = FileSystemOperations

let composeCleanUp =
    let deleteSource moveRequest = fsOperations.Delete moveRequest.Source
    cleanUp deleteSource

let composeCompareFiles = compareFiles fsOperations.ReadAllBytes

let composeMoveWithFs =
    let ensureDirectoryExists = ensureDirectoryExists fsOperations.DirectoryExists fsOperations.CreateDirectory
    let copy source destination overwrite = fsOperations.Copy(source, destination, overwrite)
    let copyToDestination moveRequest = copyToDestination ensureDirectoryExists copy moveRequest

    moveFile copyToDestination

let composeGetPictures =
    let timeProvider () = DateTimeOffset.UtcNow

    getOldPictures timeTaken timeProvider allFilesInPath

[<EntryPoint>]
let main argv =

    let getPictures = composeGetPictures
    let getMoveRequests = getMoveRequests getPictures
    let moveWithFs = composeMoveWithFs
    let cleanUp = composeCleanUp

    let arguments = parseArguments argv

    runner moveWithFs composeCompareFiles cleanUp getMoveRequests arguments

    0
