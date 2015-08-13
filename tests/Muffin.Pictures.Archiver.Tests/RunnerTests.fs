namespace Muffin.Pictures.Archiver.Tests

open Swensen.Unquote;
open Xunit;

open Muffin.Pictures.Archiver.Tests.TestHelpers
open Muffin.Pictures.Archiver.Domain

module RunnerTests  =
    
    open Muffin.Pictures.Archiver.FileMover
    open Muffin.Pictures.Archiver.Runner

//    [<Fact>]
//    let ``when the source and destination files match byte contents, the source file gets deleted`` () =
//        let compareFiles _ = true
//        let mutable deleteSourceWasCalled = false
//        let deleteSource _ = deleteSourceWasCalled <- true
//
//        moveFile copyToDestination compareFiles deleteSource {Source = ""; Destination = ""} |> ignore
//
//        test <@ true = deleteSourceWasCalled @>
//
//    [<Fact>]
//    let ``when the source and destination files do NOT match byte contents, the source file does NOT get deleted`` () =
//        let compareFiles _ = false
//        let mutable deleteSourceWasCalled = false
//        let deleteSource _ =
//            deleteSourceWasCalled <- true
//
//        moveFile copyToDestination compareFiles deleteSource {Source = ""; Destination = ""} |> ignore
//
//        test <@ false = deleteSourceWasCalled @>