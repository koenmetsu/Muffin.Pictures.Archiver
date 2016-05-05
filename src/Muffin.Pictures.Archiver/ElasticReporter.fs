namespace Muffin.Pictures.Archiver

open Muffin.Pictures.Archiver.Report

module ElasticReporter =
    open Nest
    open System
    open System.Linq.Expressions
    open Microsoft.FSharp.Linq.RuntimeHelpers

    type MoveResult = { Result: string;
                        Date: DateTime }

    type ProcessedMove = { MoveRequest: MoveRequest;
                           Year: int;
                           Month: int;
                           Day: int;
                           Hour: int;
                           DayOfWeek: string}

    let moveResultsIndexName = "move-results"
    let processedMovesIndexName = "processed-moves"

    let private processedMove quotation =
            quotation
            |> LeafExpressionConverter.QuotationToLambdaExpression
            |> unbox<Expression<Func<ProcessedMove, obj>>>

    let private moveResult quotation =
        quotation
        |> LeafExpressionConverter.QuotationToLambdaExpression
        |> unbox<Expression<Func<MoveResult, obj>>>

    let private createProcessedMovesIndex (client:ElasticClient) =
        let moveRequestDestination =
            <@ System.Func<_, _>(fun n -> box n.MoveRequest.Destination) @>

        let dayOfWeek =
            <@ System.Func<_, _>(fun n -> box n.DayOfWeek) @>

        client.CreateIndex(processedMovesIndexName, fun i ->
            i.AddMapping<ProcessedMove>(fun m ->
                m.Properties(fun p ->
                    p.String(fun s ->
                        s.Name(processedMove moveRequestDestination)
                         .Index(Nest.FieldIndexOption.NotAnalyzed)
                    ).String(fun s ->
                        s.Name(processedMove dayOfWeek)
                         .Index(Nest.FieldIndexOption.NotAnalyzed)))))

    let private createMoveResultsIndex (client:ElasticClient) =
        let result =
            <@ System.Func<_, _>(fun n -> box n.Result) @>

        client.CreateIndex(moveResultsIndexName, fun i ->
                i.AddMapping(fun m ->
                    m.Type<MoveResult>()
                        .Properties(fun p ->
                            p.String(fun s ->
                                s.Name(moveResult result)
                                 .Index(Nest.FieldIndexOption.NotAnalyzed)))))

    let private reportToElastic' elasticUri report =
        let node = new Uri(elasticUri.ToString())
        let settings = new ConnectionSettings(node)
        let client = new ElasticClient(settings)

        let date = System.DateTime.Now.Date

        createMoveResultsIndex client |> ignore
        createProcessedMovesIndex client |> ignore

        let indexCases cases =
            cases
            |> List.map(fun i -> {Result = i.GetType().Name; Date = date})
            |> List.map(fun case -> client.Index(case, fun idx -> idx.Index(moveResultsIndexName)))
            |> ignore

        let indexMoves report =
            let create success =
                let taken = success.TimeTaken
                {MoveRequest = success; Year = taken.Year; Month = taken.Month; Day = taken.Day; Hour = taken.Hour; DayOfWeek = Enum.GetName(taken.DayOfWeek.GetType(), taken.DayOfWeek)}

            report.Successes
            |> List.map(fun success -> client.Index(create success, fun idx -> idx.Index(processedMovesIndexName)))
            |> ignore


        indexCases report.Skips
        indexCases report.Successes
        indexCases report.Failures

        indexMoves report
        ()

    let reportToElastic (elasticUri:Uri option) (report:Report) =
        match elasticUri with
        | Some uri -> reportToElastic' uri report
        | None -> ()