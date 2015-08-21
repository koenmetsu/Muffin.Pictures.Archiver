namespace Muffin.Pictures.Archiver

open Muffin.Pictures.Archiver.Report

module ConsoleReporter =

    let reportToConsole report =
        let consoleWriter text =
            printfn "%s" text

        reportTo report consoleWriter
