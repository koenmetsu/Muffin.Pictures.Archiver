namespace Muffin.Pictures.Archiver.Tests

open TestHelpers
open NUnit.Framework
open Swensen.Unquote

open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Rop

module MovesTests =

    open Muffin.Pictures.Archiver.MoveRequests;

    [<Test>]
    let ``getMoveRequests returns the correct moves `` () =
        let getPictures _ =
                [
                    Success { File = { FullPath = @"/path/to/originDir/pic.jpg"; File.Name = "pic.jpg" };
                              TakenOn = dateTimeOffset 2014 01 01; Location = None }
                    Success { File = { FullPath = @"/path/to/originDir/pic2.jpg"; File.Name = "pic2.jpg" };
                              TakenOn = dateTimeOffset 2014 12 31; Location = None }
                ]

        let actual = getMoveRequests getPictures "" @"/path/to/destinationDir" |> List.ofSeq
        let expected : Result<MoveRequest,Failure> list =
            [
                    Success { Source = @"/path/to/originDir/pic.jpg";
                              Destination = System.String.Format(@"/path/to/destinationDir{0}2014-01{0}pic.jpg", System.IO.Path.DirectorySeparatorChar); TimeTaken = dateTimeOffset 2014 01 01; Location = None }
                    Success { Source = @"/path/to/originDir/pic2.jpg";
                              Destination = System.String.Format(@"/path/to/destinationDir{0}2014-12{0}pic2.jpg", System.IO.Path.DirectorySeparatorChar); TimeTaken = dateTimeOffset 2014 12 31; Location = None }
            ]
        test <@ expected = actual @>
