namespace Muffin.Pictures.Archiver.IntegrationTests

open NUnit.Framework
open System.IO
open System

open Muffin.Pictures.Archiver.CompositionRoot
open Muffin.Pictures.Archiver.Runner
open Muffin.Pictures.Archiver.Domain

module RunOnActualData =

    let assertExists paths =
        let filePath = Path.Combine(paths) |> FileInfo |> fun fi -> fi.FullName
        Assert.IsTrue(File.Exists(filePath), "Verify that file \"{0}\" exists.", filePath)

    let copyDirectory src dest =
        if not <| System.IO.Directory.Exists(src) then
            let msg = System.String.Format("Source directory does not exist or could not be found: {0}", src)
            raise (System.IO.DirectoryNotFoundException(msg))

        if not <| System.IO.Directory.Exists(dest) then
            System.IO.Directory.CreateDirectory(dest) |> ignore

        let srcDir = new System.IO.DirectoryInfo(src)

        for file in srcDir.GetFiles() do
            let temppath = System.IO.Path.Combine(dest, file.Name)
            file.CopyTo(temppath, true) |> ignore

    let runInCopy folder f =
        if Directory.Exists folder then
            Directory.Delete(folder, true)

        copyDirectory "testdata" folder
        f folder
        Directory.Delete(folder, true)

    let fileExif20141202 = "exif20141202.jpg" // has no tag for time taken
    let fileXmp201503 = "xmp20150316.jpg" // has time taken
    let fileNoExif = "no_exif.jpg" // has time taken


    [<Test>]
    let ``Run in strict mode`` () =
        runInCopy "testData1" <|
        (fun testFolder ->
            let arguments = { SourceDir = testFolder; DestinationDir = testFolder; Mode = TimeTakenMode.Strict; MailTo = None}
            let move = composeMove
            let getMoveRequests = composeGetMoveRequests' arguments <| DateTimeOffset.UtcNow.AddYears 1

            runner move getMoveRequests arguments

            assertExists [| testFolder; "2014-12"; fileExif20141202 |]
            assertExists [| testFolder; fileNoExif |]
            assertExists [| testFolder; "2015-03"; fileXmp201503 |]
        )

    [<Test>]
    let ``Run in fallback mode`` () =
        runInCopy "testData2" <|
        (fun testFolder ->
            File.SetLastWriteTimeUtc(Path.Combine(testFolder, fileNoExif), DateTime.Parse("2015-01-01"))

            let arguments = { SourceDir = testFolder; DestinationDir = testFolder; Mode = TimeTakenMode.Fallback; MailTo = None}
            let move = composeMove
            let getMoveRequests = composeGetMoveRequests' arguments <| DateTimeOffset.UtcNow.AddYears 1

            runner move getMoveRequests arguments


            assertExists [| testFolder; "2014-12"; fileExif20141202 |]
            assertExists [| testFolder; "2015-01" ; fileNoExif |]
            assertExists [| testFolder; "2015-03"; fileXmp201503 |]
        )

