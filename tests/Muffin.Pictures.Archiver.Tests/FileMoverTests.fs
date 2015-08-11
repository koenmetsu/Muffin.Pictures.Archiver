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
    let ``when the source and destination files match byte contents, the source file gets deleted`` () =
        let compareFiles _ = true
        let mutable deleteSourceWasCalled = false
        let deleteSource _ = deleteSourceWasCalled <- true

        moveFile copyToDestination compareFiles deleteSource {MoveRequest.Source = ""; Destination = ""} |> ignore

        test <@ true = deleteSourceWasCalled @>

    [<Fact>]
    let ``when the source and destination files do NOT match byte contents, the source file does NOT get deleted`` () =
        let compareFiles _ = false
        let mutable deleteSourceWasCalled = false
        let deleteSource _ =
            deleteSourceWasCalled <- true

        moveFile copyToDestination compareFiles deleteSource {MoveRequest.Source = ""; Destination = ""} |> ignore

        test <@ false = deleteSourceWasCalled @>



//    [<Fact>]
//    let ``Blablabla`` () =
//        let pictures : seq<Picture> =
//            seq [
//                    { Picture.File = { FullPath = @"c:\path\to\originDir\pic.jpg"; File.Name = "pic.jpg" };
//                      TakenOn = dateTimeOffset 2014 01 01 }
//                    { Picture.File = { FullPath = @"c:\path\to\originDir\pic2.jpg"; File.Name = "pic2.jpg" };
//                      TakenOn = dateTimeOffset 2014 12 31 }
//                ]
//
//        let mutable copyCalled = false
//
//        let copyImpl = fun () -> copyCalled <- true
//
//        let fileSystem = new MockFileSystem(copyImpl, (fun _ -> ()), (fun _ -> byte(0)),  (fun _ -> ()), (fun _ -> false))
//
//        move fileSystem ".\temp" pictures

