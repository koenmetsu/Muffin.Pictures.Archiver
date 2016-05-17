namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("Muffin.Pictures.Archiver")>]
[<assembly: AssemblyProductAttribute("Muffin.Pictures.Archiver")>]
[<assembly: AssemblyDescriptionAttribute("Archiver for Muffin's pictures")>]
[<assembly: AssemblyVersionAttribute("1.3.2")>]
[<assembly: AssemblyFileVersionAttribute("1.3.2")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.3.2"
    let [<Literal>] InformationalVersion = "1.3.2"
