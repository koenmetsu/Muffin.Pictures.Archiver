namespace Muffin.Pictures.Archiver.Tests

open Swensen.Unquote;
open Xunit;

open Muffin.Pictures.Archiver.Tests.TestHelpers
open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Rop

module FileMoverTests =

    open Muffin.Pictures.Archiver.FileMover
    open Muffin.Pictures.Archiver.FileSystem

    [<Fact>]
    let ``when directory does not exist, it gets created`` () =
        let directoryExists _ = false
        let mutable createDirectoryCalls = []
        let createDirectory path =
            createDirectoryCalls <- List.append [path] createDirectoryCalls
            ignore()

        ensureDirectoryExists directoryExists createDirectory "c:\some\path"

        test <@ ["c:\some\path"] = createDirectoryCalls @>

    [<Fact>]
    let ``when directory exists, it does not get created`` () =
        let directoryExists _ = true
        let mutable createDirectoryCalls = []
        let createDirectory path =
            createDirectoryCalls <- List.append [path] createDirectoryCalls
            ignore()

        ensureDirectoryExists directoryExists createDirectory "c:\some\path"

        test <@ List.isEmpty createDirectoryCalls @>

    [<Fact>]
    let ``when the move was successful, it returns a successful move`` () =
        let moveRequest = {MoveRequest.Source = "source"; Destination = "destination"}
        let copyToDestination _ = ()

        let move =
            moveFile copyToDestination moveRequest

        test <@ Success moveRequest = move @>

    [<Fact>]
    let ``when the move was NOT successful, it returns a failed move with a failure reason`` () =
        let compareFiles _ = false
        let copyToDestination _ = failwith "File in use or something"

        let moveRequest = {MoveRequest.Source = "source"; Destination = "destination"}

        let move =
            moveFile copyToDestination moveRequest

        test <@ Failure <| CouldNotCopyFile {Request = moveRequest; Message = "File in use or something"} = move @>
