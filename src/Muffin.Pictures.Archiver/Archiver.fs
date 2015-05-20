namespace Muffin.Pictures.Archiver

open System.IO
open System

module Files =
    let isOld (file:FileInfo) = file.LastWriteTimeUtc < DateTime.UtcNow.AddMonths(-1)

    let allFilesInPath path = 
        Directory.EnumerateFiles path
        |> Seq.map (fun fileName -> new FileInfo(fileName))
        
    let onlyOldFiles files = 
        files 
        |> Seq.filter<FileInfo> isOld
    
    let printFile (file:FileInfo) =
        printfn "%s with date %s" file.FullName (file.LastWriteTimeUtc.ToShortDateString())

    
