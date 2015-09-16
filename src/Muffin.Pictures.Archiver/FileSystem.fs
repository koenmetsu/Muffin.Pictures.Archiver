namespace Muffin.Pictures.Archiver

open System.IO

module FileSystem =

    type SeqInner<'T> =
    | Ended
    | Item of 'T * SeqInner<'T>

    /// Read file 'fn' in blocks of size 'size'
    /// (returns on-demand asynchronous sequence)
    let readInBlocks (stream:FileStream) size =
        let buffer = Array.zeroCreate size

        /// Returns next block as 'Item' of async seq
        let rec nextBlock () =
          let count = stream.Read(buffer, 0, size)
          if count = 0 then Ended
          else
            // Create buffer with the right size
            let res =
                if count = size then buffer
                else buffer |> Seq.take count |> Array.ofSeq
            Item(res, nextBlock())

        nextBlock()

    /// Function that compares two sequences
    /// item by item. If an item doesn't match, 'false' is returned
    /// immediately without generating the rest of the sequence. If the
    /// lengths don't match, exception is thrown.
    let rec compare seq1 seq2 =
        let item1 = seq1
        let item2 = seq2
        match item1, item2 with
        | Item(b1, ns1), Item(b2, ns2) when b1 <> b2 -> false
        | Item(b1, ns1), Item(b2, ns2) -> compare ns1 ns2
        | Ended, Ended -> true
        | _ -> false

    let pathCombine basePath target =
        Path.Combine(basePath, target)

    type IFileSystemOperations =
        abstract Copy: string * string * bool -> unit
        abstract Delete: string -> unit
        abstract ReadAllBytes: string -> byte[]
        abstract CreateDirectory: string -> unit
        abstract DirectoryExists: string -> bool
        abstract Compare: FilePath -> FilePath -> bool

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

            member this.Compare source destination =
                use sourceStream = File.OpenRead(source)
                use destinationStream = File.OpenRead(destination)
                let s1 = readInBlocks sourceStream  100000
                let s2 = readInBlocks destinationStream 100000
                compare s1 s2
        }