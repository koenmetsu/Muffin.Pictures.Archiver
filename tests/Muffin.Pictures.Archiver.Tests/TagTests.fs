namespace Muffin.Pictures.Archiver.Tests

open Swensen.Unquote
open NUnit.Framework
open FSharp.Data

open Muffin.Pictures.Archiver.Tests.TestHelpers
open Muffin.Pictures.Archiver.Domain

module TagTests =
    open System.IO
    open Muffin.Pictures.Archiver.TagRetriever

    let private location name =
        System.Reflection.Assembly.GetExecutingAssembly().Location
        |> Directory.GetParent
        |> fun dir -> Path.Combine(dir.FullName, name)

    [<Test>]
    let ``Call exifTool and try to parse results`` () =
        let tags = callExifTool
                        (location exifFile)
                        (location @"TestData")

        test <@ Seq.length tags = 3 @>

    [<Test>]
    let ``Try parsing test`` () =
        let tags = Tags.Load("example_exiftool_output.json")

        test <@ Seq.length tags = 5 @>
