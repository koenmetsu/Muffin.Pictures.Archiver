namespace Muffin.Pictures.Archiver

open Domain
open Paths
open Moves

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

    let moveFile copyToDestination compareFiles deleteSource moveRequest =
        copyToDestination moveRequest
        let filesMatch = compareFiles moveRequest

        if filesMatch then
            deleteSource moveRequest
            {Request = moveRequest; Result = Success}
        else
            {Request = moveRequest; Result = Failure BytesDidNotMatch}

    let compareFiles readAllBytes moveRequest =
        let sourceStream = readAllBytes moveRequest.Source
        let destinationStream = readAllBytes moveRequest.Destination
        sourceStream = destinationStream

    let copyToDestination ensureDirectoryExists copy moveRequest =
        ensureDirectoryExists moveRequest.Destination
        copy moveRequest.Source moveRequest.Destination true

    let ensureDirectoryExists directoryExists createDirectory path =
        if not (directoryExists path) then
            createDirectory path