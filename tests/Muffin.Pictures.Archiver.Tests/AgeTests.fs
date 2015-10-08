namespace Muffin.Pictures.Archiver.Tests

open TestHelpers
open NUnit.Framework
open Swensen.Unquote

open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Pictures
open Muffin.Pictures.Archiver.Rop

module AgeTests =

    [<Test>]
    let ``a picture is old when it is older than one day`` () =
        let olderThan1Day = (dateTimeOffset 2014 12 31).AddTicks(-1L)
        let picture =
            { File = stubFile
              TakenOn = olderThan1Day }

        let timeProvider = dateTimeOffset 2015 01 01

        test <@ isOld timeProvider picture = Success picture @>

    [<Test>]
    let ``a picture is old when it is exactly one day old`` () =
        let picture =
            { File = stubFile
              TakenOn = dateTimeOffset 2014 12 31 }

        let timeProvider = dateTimeOffset 2015 01 01

        test <@ isOld timeProvider picture = Success picture @>

    [<Test>]
    let ``a picture is not old when it is not older than one day`` () =
        let picture =
            { File = stubFile
              TakenOn = System.DateTimeOffset.Parse("2014-12-01 00:00") }

        let timeProvider = System.DateTimeOffset.Parse("2014-12-01 23:59")

        test <@ isOld timeProvider picture = (Failure <| Skip.PictureWasNotOldEnough picture) @>
