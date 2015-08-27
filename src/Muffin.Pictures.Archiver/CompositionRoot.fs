namespace Muffin.Pictures.Archiver

open System

open Muffin.Pictures.Archiver.Pictures
open Muffin.Pictures.Archiver.FileMover
open Muffin.Pictures.Archiver.Moves
open Muffin.Pictures.Archiver.Files
open Muffin.Pictures.Archiver.TimeTakenRetriever
open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.FileSystem

open Muffin.Pictures.Archiver.Runner

module CompositionRoot =

    let fsOperations = FileSystemOperations

    let composeCleanUp =
        let deleteSource moveRequest = fsOperations.Delete moveRequest.Source
        cleanUp deleteSource

    let composeMoveWithFs =
        let ensureDirectoryExists = ensureDirectoryExists fsOperations.DirectoryExists fsOperations.CreateDirectory
        let copy source destination overwrite = fsOperations.Copy(source, destination, overwrite)
        let copyToDestination moveRequest = copyToDestination ensureDirectoryExists copy moveRequest

        moveFile copyToDestination

    let composeGetPictures arguments =
        let timeProvider () = DateTimeOffset.UtcNow
        let timeTakenProvider = timeTaken arguments.Mode
        getOldPictures timeTakenProvider timeProvider allFilesInPath

    let composeMove =
        let moveWithFs = composeMoveWithFs
        let cleanUp = composeCleanUp
        let composeCompareFiles = compareFiles fsOperations.ReadAllBytes
        move moveWithFs composeCompareFiles cleanUp

    let composeGetMoveRequests arguments =
        let getPictures = composeGetPictures arguments
        getMoveRequests getPictures
