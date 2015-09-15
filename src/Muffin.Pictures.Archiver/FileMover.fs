namespace Muffin.Pictures.Archiver

open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Rop

open System.IO // eventually this should be moved out.

module FileMover =

    type AsyncSeq<'T> = Async<AsyncSeqInner<'T>>
    and AsyncSeqInner<'T> =
    | Ended
    | Item of 'T * AsyncSeq<'T>

    /// Read file 'fn' in blocks of size 'size'
    /// (returns on-demand asynchronous sequence)
    let readInBlocks fn size = async {
        let stream = File.OpenRead(fn)
        let buffer = Array.zeroCreate size

        /// Returns next block as 'Item' of async seq
        let rec nextBlock () = async {
          let! count = stream.AsyncRead(buffer, 0, size)
          if count = 0 then return Ended
          else
            // Create buffer with the right size
            let res =
                if count = size then buffer
                else buffer |> Seq.take count |> Array.ofSeq
            return Item(res, nextBlock()) }

    return! nextBlock() }

    /// Asynchronous function that compares two asynchronous sequences
    /// item by item. If an item doesn't match, 'false' is returned
    /// immediately without generating the rest of the sequence. If the
    /// lengths don't match, exception is thrown.
    let rec compareAsyncSeqs seq1 seq2 = async {
        let! item1 = seq1
        let! item2 = seq2
        match item1, item2 with
        | Item(b1, ns1), Item(b2, ns2) when b1 <> b2 -> return false
        | Item(b1, ns1), Item(b2, ns2) -> return! compareAsyncSeqs ns1 ns2
        | Ended, Ended -> return true
        | _ -> return false }

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
        let s1 = readInBlocks moveRequest.Source 1000
        let s2 = readInBlocks moveRequest.Destination 1000
        let areEqual = Async.RunSynchronously <| compareAsyncSeqs s1 s2

        if areEqual then
            Success moveRequest
        else
            BytesDidNotMatch moveRequest |> Failure

    let copyToDestination ensureDirectoryExists copy moveRequest =
        ensureDirectoryExists moveRequest.Destination
        copy moveRequest.Source moveRequest.Destination true

    let ensureDirectoryExists directoryExists createDirectory path =
        if not (directoryExists path) then
            createDirectory path

