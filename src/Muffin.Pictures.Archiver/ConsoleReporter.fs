namespace Muffin.Pictures.Archiver

open Muffin.Pictures.Archiver.Domain
open Muffin.Pictures.Archiver.Report

open System

module ConsoleReporter =

    let reportToConsole report =
        let consoleWriter text =
            printfn "%s" text

        reportTo report consoleWriter
