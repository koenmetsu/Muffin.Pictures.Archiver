namespace Muffin.Pictures.Archiver

open Muffin.Pictures.Archiver.Report

module ElasticReporter =
    open Nest
    open System

    type report<'T> = {Report: 'T; Date: DateTime}

    let reportToElastic (report:Report) =
        let node = new Uri("http://localhost:9200")
        let settings = new ConnectionSettings(node)
        let client = new ElasticClient(settings)

        let date = System.DateTime.Now.Date.AddDays(-1.)

        client.DeleteIndex("moves") |> ignore
        let index =
            client
                .CreateIndex("moves", fun i ->
                    i.AddMapping(fun m ->
                        m.Type<MoveRequest>()
                         .Properties(fun p -> p.String(fun s -> s.Name("destination").Index(Nest.FieldIndexOption.NotAnalyzed))))
                )

        let indexCases cases =
            cases
            |> List.map(fun i -> {Report= i.GetType().Name; Date = date})
            |> List.map(fun case -> client.Index(case, fun idx -> idx.Index("reports")))
            |> ignore

        let indexMoves report =
            report.Successes
            |> List.map(fun success -> client.Index(success, fun idx -> idx.Index("moves")))
            |> ignore


        indexCases report.Skips
        indexCases report.Successes
        indexCases report.Failures
        indexMoves report
        ()