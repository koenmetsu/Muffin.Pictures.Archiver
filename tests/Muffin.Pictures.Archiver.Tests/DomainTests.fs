namespace Muffin.Pictures.Archiver.Tests

open TestHelpers
open Xunit
open Swensen.Unquote

open Muffin.Pictures.Archiver.Domain

module DomainTests =

    [<Fact>]
    let ``Picture.formatToken with two-digit month returns unpadded month`` () =
        let picture =
            {
                File = stubFile
                TakenOn = dateTimeOffset 2014 12 31
            }

        test <@ picture.formatTakenOn = "2014-12" @>

    [<Fact>]
    let ``Picture.formatToken with single digit month returns zero-padded month`` () =
        let picture =
            {
                File = stubFile
                TakenOn = dateTimeOffset 2015 01 01
            }

        test <@ picture.formatTakenOn = "2015-01" @>
