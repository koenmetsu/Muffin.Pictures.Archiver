module Muffin.Pictures.Archiver.Tests

open System
open Xunit
open Swensen.Unquote

open Domain

let dateTimeOffset year month day = 
    new DateTimeOffset(new DateTime(year, month, day, 0, 0, 0))

let stubFile = {FullPath = @"\path\to\anyPicture.jpg"; Name = "anyPicture.jpg"}

module AgeTests =

    open Age

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

module OtherTests =

    open Muffin.Pictures.Archiver.Pictures

    [<Fact>]
    let ``toPicture returns Picture with same File and timeTaken provided by timeTakenRetriever`` () =
        let timeTaken = dateTimeOffset 2015 12 31
        let file = stubFile
        let timeTakenRetriever = fun (_ : string) -> timeTaken

        let expected = {File = file; TakenOn = timeTaken}

        let actual = toPicture timeTakenRetriever file

        test <@ expected = actual @>

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