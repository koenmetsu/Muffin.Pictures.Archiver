namespace Muffin.Pictures.Archiver.Tests

open TestHelpers
open NUnit.Framework
open Swensen.Unquote

open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Pictures
open Muffin.Pictures.Archiver.Rop

module AgeTests =

    [<Test>]
    let ``a picture is old when it is older than one month`` () =
        let picture =
            { File = stubFile
              TakenOn = dateTimeOffset 2014 12 1 }

        let timeProvider = dateTimeOffset 2015 12 31

        test <@ isOld timeProvider picture = Success picture @>

    [<Test>]
    let ``a picture is not old when it is older than one month`` () =
        let picture =
            { File = stubFile
              TakenOn = dateTimeOffset 2014 12 1 }

        let timeProvider = dateTimeOffset 2014 12 31

        test <@ isOld timeProvider picture = (Failure <| Skip.PictureWasNotOldEnough picture) @>
