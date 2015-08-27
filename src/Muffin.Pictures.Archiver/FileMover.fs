namespace Muffin.Pictures.Archiver

open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Rop

module FileSystem =

    open System.IO

    type IFileSystemOperations =
        abstract Copy: string * string * bool -> unit
        abstract Delete: string -> unit
        abstract ReadAllBytes: string -> byte[]
        abstract CreateDirectory: string -> unit
        abstract DirectoryExists: string -> bool

    let FileSystemOperations =
        { new IFileSystemOperations with
            member this.Copy(source, destination, overwrite) =
                File.Copy(source, destination, overwrite)

            member this.Delete path =
                File.Delete path

            member this.ReadAllBytes path =
                File.ReadAllBytes path

            member this.CreateDirectory path =
                let fileInfo = FileInfo path
                let directoryPath = fileInfo.Directory
                Directory.CreateDirectory directoryPath.FullName |> ignore

            member this.DirectoryExists path =
                let fileInfo = FileInfo path
                fileInfo.Directory.Exists
        }

module FileMover =

    let moveFile copyToDestination moveRequest =
        try
            copyToDestination moveRequest
            Success moveRequest
        with
        | ex -> CouldNotCopyFile {Request = moveRequest; Message = ex.Message} |> Failure

    let cleanUp deleteSource moveRequest =
        try
            deleteSource moveRequest
            Success moveRequest
        with
        | ex -> CouldNotDeleteSource {Request = moveRequest; Message = ex.Message} |> Failure

    let compareFiles readAllBytes moveRequest =
        let sourceStream = readAllBytes moveRequest.Source
        let destinationStream = readAllBytes moveRequest.Destination
        if sourceStream = destinationStream then
            Success moveRequest
        else
            BytesDidNotMatch moveRequest |> Failure

    let copyToDestination ensureDirectoryExists copy moveRequest =
        ensureDirectoryExists moveRequest.Destination
        copy moveRequest.Source moveRequest.Destination true

    let ensureDirectoryExists directoryExists createDirectory path =
        if not (directoryExists path) then
            createDirectory path
