namespace Muffin.Pictures.Archiver.Tests

open TestHelpers
open Xunit
open Swensen.Unquote

open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Rop

module MovesTests =

    open Muffin.Pictures.Archiver.MoveRequests;

    [<Fact>]
    let ``getMoveRequests returns the correct moves `` () =
        let getPictures _ =
                [
                    Success {
                              Picture.File = { FullPath = @"c:\path\to\originDir\pic.jpg"; File.Name = "pic.jpg" };
                              TakenOn = dateTimeOffset 2014 01 01 }
                    Success {
                              Picture.File = { FullPath = @"c:\path\to\originDir\pic2.jpg"; File.Name = "pic2.jpg" };
                              TakenOn = dateTimeOffset 2014 12 31 }
                ]

        let actual = getMoveRequests getPictures "" @"c:\path\to\destinationDir" |> List.ofSeq
        let expected : Result<MoveRequest,Failure> list =
            [
                    Success { Source = @"c:\path\to\originDir\pic.jpg";
                              Destination = @"c:\path\to\destinationDir\2014-01\pic.jpg" }
                    Success { Source = @"c:\path\to\originDir\pic2.jpg";
                              Destination = @"c:\path\to\destinationDir\2014-12\pic2.jpg" }
            ]
        test <@ expected = actual @>