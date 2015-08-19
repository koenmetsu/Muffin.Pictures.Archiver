namespace Muffin.Pictures.Archiver.Tests

open TestHelpers
open Xunit
open Swensen.Unquote

open Muffin.Pictures.Archiver.Pictures
open Muffin.Pictures.Archiver.Domain

module PicturesTests =

    [<Fact>]
    let ``toPicture returns Picture with same File and timeTaken provided by timeTakenRetriever`` () =
        let file = stubFile
        let timeTakenRetriever = fun (_ : File) -> Some <| dateTimeOffset 2015 12 31

        let expected = Some {File = file; TakenOn = dateTimeOffset 2015 12 31}

        let actual = toPicture timeTakenRetriever file

        test <@ expected = actual @>

    [<Fact>]
    let ``getOldPictures returns only pictures older than 1 month`` () =
        let oldFile = stubFile
        let newFile = anotherStubFile
        let files _ = seq [ oldFile
                            newFile ]

        let oldDate = dateTimeOffset 2014 12 01

        let timeTakenRetriever file =
            if file = oldFile then
                Some oldDate
            else
                Some <| dateTimeOffset 2014 12 31

        let timeProvider () = dateTimeOffset 2015 01 05

        let actual = List.ofSeq <| getOldPictures timeTakenRetriever timeProvider files "path"
        let expected = [ {File = oldFile; TakenOn = oldDate} ]

        test <@ expected = actual @>
