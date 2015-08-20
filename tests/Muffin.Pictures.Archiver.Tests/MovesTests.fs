namespace Muffin.Pictures.Archiver.Tests

open TestHelpers
open Xunit
open Swensen.Unquote

open Muffin.Pictures.Archiver.Domain

module MovesTests =

    open Muffin.Pictures.Archiver.Moves;

    [<Fact>]
    let ``getMoveRequests returns the correct moves `` () =
        let getPictures _ =
                [
                    { Picture.File = { FullPath = @"c:\path\to\originDir\pic.jpg"; File.Name = "pic.jpg" };
                      TakenOn = dateTimeOffset 2014 01 01 }
                    { Picture.File = { FullPath = @"c:\path\to\originDir\pic2.jpg"; File.Name = "pic2.jpg" };
                      TakenOn = dateTimeOffset 2014 12 31 }
                ]

        let actual = getMoveRequests getPictures "" @"c:\path\to\destinationDir" |> List.ofSeq
        let expected =
            [
                    { Source = @"c:\path\to\originDir\pic.jpg";
                      Destination = @"c:\path\to\destinationDir\2014-01\pic.jpg" }
                    { Source = @"c:\path\to\originDir\pic2.jpg";
                      Destination = @"c:\path\to\destinationDir\2014-12\pic2.jpg" }
                ]
        test <@ expected = actual @>