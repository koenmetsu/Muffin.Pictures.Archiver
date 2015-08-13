namespace Muffin.Pictures.Archiver.Tests

open Swensen.Unquote;
open Xunit;

open Muffin.Pictures.Archiver.Tests.TestHelpers
open Muffin.Pictures.Archiver.Domain

module FileMoverTests =

    open Muffin.Pictures.Archiver.FileMover
    open Muffin.Pictures.Archiver.FileSystem

    let copyToDestination _ = ()

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
        let compareFiles _ = true

        let moveRequest = {MoveRequest.Source = "source"; Destination = "destination"}

        let move =
            moveFile copyToDestination compareFiles moveRequest

        test <@ SuccessfulMove {Request = moveRequest} = move @>

    [<Fact>]
    let ``when the move was NOT successful, it returns a failed move with a failure reason`` () =
        let compareFiles _ = false
        let deleteSource _ = ()

        let moveRequest = {MoveRequest.Source = "source"; Destination = "destination"}

        let move =
            moveFile copyToDestination compareFiles moveRequest

        test <@ FailedMove {Request = moveRequest; Reason = BytesDidNotMatch} = move @>
