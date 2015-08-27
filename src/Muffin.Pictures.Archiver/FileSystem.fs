namespace Muffin.Pictures.Archiver

open System.IO

module FileSystem =

    let pathCombine basePath target =
        Path.Combine(basePath, target)

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