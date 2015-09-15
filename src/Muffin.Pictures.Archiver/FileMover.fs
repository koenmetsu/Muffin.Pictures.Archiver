namespace Muffin.Pictures.Archiver

open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Rop

open System.IO // eventually this should be moved out.

module FileMover =

    let move moveWithFs compareFiles cleanUp =
        moveWithFs
        >=> compareFiles
        >=> cleanUp

    let moveFile copyToDestination moveRequest =
        try
            copyToDestination moveRequest
            Success moveRequest
        with
        | ex -> CouldNotCopyFile { Request = moveRequest; Message = ex.Message } |> Failure

    let cleanUp deleteSource moveRequest =
        try
            deleteSource moveRequest
            Success moveRequest
        with
        | ex -> CouldNotDeleteSource { Request = moveRequest; Message = ex.Message } |> Failure

    let compareFiles readAllBytes moveRequest =
        let sourceFi = FileInfo moveRequest.Source
        let destinationFi = FileInfo moveRequest.Destination
        if sourceFi.Length <> destinationFi.Length then
            BytesDidNotMatch moveRequest |> Failure
        else
            use fsSource = sourceFi.OpenRead()
            use fsDestination = destinationFi.OpenRead()
            let BYTES_TO_READ = (int)System.Int16.MaxValue 
            let iterations = int <| System.Math.Ceiling((double)fsSource.Length / (double)BYTES_TO_READ);

            let one = Array.zeroCreate<byte> BYTES_TO_READ
            let two = Array.zeroCreate<byte> BYTES_TO_READ

            let areAllEqual =
                [0..iterations]
                |> Seq.map (fun iter ->
                                fsSource.Read(one, 0, BYTES_TO_READ) |> ignore
                                fsDestination.Read(two, 0, BYTES_TO_READ) |> ignore

                                let equal = System.BitConverter.ToInt64(one,0) <> System.BitConverter.ToInt64(two,0)
                                equal)
                |> Seq.exists (fun areEqual -> not <| areEqual)

            if areAllEqual then
                Success moveRequest
            else
                BytesDidNotMatch moveRequest |> Failure

    let copyToDestination ensureDirectoryExists copy moveRequest =
        ensureDirectoryExists moveRequest.Destination
        copy moveRequest.Source moveRequest.Destination true

    let ensureDirectoryExists directoryExists createDirectory path =
        if not (directoryExists path) then
            createDirectory path

