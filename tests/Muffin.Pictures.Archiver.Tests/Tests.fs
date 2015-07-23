namespace Muffin.Pictures.Archiver.Tests

open TestHelpers
open Xunit
open Swensen.Unquote

open Muffin.Pictures.Archiver.Domain

module AgeTests =

    open Muffin.Pictures.Archiver.Age

    [<Fact>]
    let ``a picture is old when it is older than one month`` () =
        let picture =
            {
                Picture.File =
                    {
                        FullPath = "";
                        Name = "";
                    };
                TakenOn = dateTimeOffset 2014 12 1
            }

        let timeProvider () = dateTimeOffset 2015 12 31

        test <@ isOld timeProvider picture @>

    [<Fact>]
    let ``a picture is not old when it is older than one month`` () =
        let picture =
            {
                Picture.File =
                    {
                        FullPath = "";
                        Name = "";
                    };
                TakenOn = dateTimeOffset 2014 12 1
            }

        let timeProvider () = dateTimeOffset 2014 12 31

        test <@ not <| isOld timeProvider picture @>

module DomainTests =

    [<Fact>]
    let ``Picture.formatToken with two-digit month returns unpadded month`` () =
        let picture =
            {
                File = stubFile;
                TakenOn = dateTimeOffset 2014 12 31
            }

        test <@ picture.formatTakenOn = "2014-12" @>

    [<Fact>]
    let ``Picture.formatToken with single digit month returns zero-padded month`` () =
        let picture =
            {
                File = stubFile;
                TakenOn = dateTimeOffset 2015 01 01
            }

        test <@ picture.formatTakenOn = "2015-01" @>

module MoveTests =

    open Muffin.Pictures.Archiver.Mover;

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