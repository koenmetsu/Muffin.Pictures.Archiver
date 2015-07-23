namespace Muffin.Pictures.Archiver.Tests

open TestHelpers
open Xunit
open Swensen.Unquote

open Muffin.Pictures.Archiver.Domain

module MovesTests =

    open Muffin.Pictures.Archiver.Moves;

    [<Fact>]
    let ``getMoves returns the correct moves `` () =
        let pictures : seq<Picture> =
            seq [
                    { Picture.File = { FullPath = @"c:\path\to\originDir\pic.jpg"; File.Name = "pic.jpg" };
                      TakenOn = dateTimeOffset 2014 01 01 }
                    { Picture.File = { FullPath = @"c:\path\to\originDir\pic2.jpg"; File.Name = "pic2.jpg" };
                      TakenOn = dateTimeOffset 2014 12 31 }
                ]

        let actual = getMoves @"c:\path\to\destinationDir" pictures |> List.ofSeq
        let expected =
            [
                    { Source = @"c:\path\to\originDir\pic.jpg";
                      Destination = @"c:\path\to\destinationDir\2014-01\pic.jpg" }
                    { Source = @"c:\path\to\originDir\pic2.jpg";
                      Destination = @"c:\path\to\destinationDir\2014-12\pic2.jpg" }
                ]
        test <@ expected = actual @>