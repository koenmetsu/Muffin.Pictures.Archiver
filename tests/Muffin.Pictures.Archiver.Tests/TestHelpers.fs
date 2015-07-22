namespace Muffin.Pictures.Archiver.Tests

open System
open Muffin.Pictures.Archiver.Domain

module TestHelpers =
    let dateTimeOffset year month day =
        new DateTimeOffset(new DateTime(year, month, day, 0, 0, 0))

    let stubFile = {FullPath = @"\path\to\anyPicture.jpg"; Name = "anyPicture.jpg"}
    let anotherStubFile = {FullPath = @"\path\to\anOtherPicture.jpg"; Name = "anOtherPicture.jpg"}

