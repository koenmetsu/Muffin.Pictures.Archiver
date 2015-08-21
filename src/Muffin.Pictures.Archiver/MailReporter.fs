namespace Muffin.Pictures.Archiver

open System.Net.Mail
open Printf

open Muffin.Pictures.Archiver.Report

module MailReporter =

    let sendMail body mailTo =
        let msg = new MailMessage(mailTo, mailTo, @"Muffin Archiver Mail!", body)

        let client = new SmtpClient()
        client.DeliveryMethod <- SmtpDeliveryMethod.Network
        client.EnableSsl <- true

        client.Send(msg)

    let reportToMail report mailTo =
        let builder = new System.Text.StringBuilder()
        let builderWriter text =
            bprintf builder "%s\n" text

        reportTo report builderWriter

        let mailBody = builder.ToString()
        sendMail mailBody mailTo
