namespace Muffin.Pictures.Archiver

module Logging =

    open Muffin.Pictures.Archiver.Rop

    open System.Configuration
    open System.Net
    open System.Net.Configuration

    open Serilog
    open Serilog.Sinks
    open Serilog.Sinks.Elasticsearch
    open Serilog.Sinks.Email
    open Serilog.Exceptions

    let basicLogConfig =
        LoggerConfiguration()
            //.Enrich.WithExceptionDetails() // does not work on Mono
            .WriteTo.RollingFile("log-{Date}.log")

    let enhanceLogConfig (loggerConfig:LoggerConfiguration) arguments =
        if Option.isSome arguments.MailTo then
            let smtpSettings = ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            match smtpSettings with
            | :? SmtpSection as section ->
                loggerConfig.WriteTo.Email(
                    fromEmail = section.From,
                    toEmail = arguments.MailTo.Value,
                    restrictedToMinimumLevel = Events.LogEventLevel.Fatal)
                |> ignore
            | _ -> ()

        if arguments.ElasticUrl |> Option.isSome then
            Option.get(arguments.ElasticUrl)
            |> ElasticsearchSinkOptions
            |> tee (fun o -> o.AutoRegisterTemplate <- true)
            |> tee (fun o -> o.BufferBaseFilename <- "./logs/buffer")
            |> loggerConfig.WriteTo.Elasticsearch
            |> ignore

        loggerConfig


