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
                Picture.File = stubFile
                TakenOn = dateTimeOffset 2014 12 1
            }

        let timeProvider () = dateTimeOffset 2015 12 31

        test <@ isOld timeProvider picture @>

    [<Fact>]
    let ``a picture is not old when it is older than one month`` () =
        let picture =
            {
                Picture.File = stubFile
                TakenOn = dateTimeOffset 2014 12 1
            }

        let timeProvider () = dateTimeOffset 2014 12 31

        test <@ not <| isOld timeProvider picture @>