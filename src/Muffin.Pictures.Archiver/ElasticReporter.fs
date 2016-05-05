namespace Muffin.Pictures.Archiver

open Muffin.Pictures.Archiver.Report
open Nest
open System
open System.Linq.Expressions
open Microsoft.FSharp.Linq.RuntimeHelpers

module MoveResultsIndex =

    type MoveResult = { Result: string;
                        Date: DateTime;
                        Count: int }

    let moveResultsIndexName = "files-in-src-dir"

    let private moveResult quotation =
        quotation
        |> LeafExpressionConverter.QuotationToLambdaExpression
        |> unbox<Expression<Func<MoveResult, obj>>>

    let createMoveResultsIndex (client:ElasticClient) =
        let result =
            <@ System.Func<_, _>(fun n -> box n.Result) @>

        client.CreateIndex(moveResultsIndexName, fun i ->
                i.AddMapping(fun m ->
                    m.Type<MoveResult>()
                        .Properties(fun p ->
                            p.String(fun s ->
                                s.Name(moveResult result)
                                 .Index(Nest.FieldIndexOption.NotAnalyzed)))))

    let private index (client:ElasticClient) (date:DateTime) (items:list<'a>) =
        if not (Seq.isEmpty items) then
            let name = items.Head.GetType().Name

            {Result = name; Date = date; Count = items.Length}
            |> fun case -> client.Index(case, fun idx -> idx.Index(moveResultsIndexName))
            |> ignore

    let indexFilesInSourceDir (client:ElasticClient) (date:DateTime) report =
        index client date report.Skips
        index client date report.Successes
        index client date report.Failures

module ProcessedMovesIndex =

    type ProcessedMove = { MoveRequest: MoveRequest;
                           Year: int;
                           Month: int;
                           Day: int;
                           Hour: int;
                           DayOfWeek: string}

    let processedMovesIndexName = "processed-moves"

    let private processedMove quotation =
            quotation
            |> LeafExpressionConverter.QuotationToLambdaExpression
            |> unbox<Expression<Func<ProcessedMove, obj>>>


    let createProcessedMovesIndex (client:ElasticClient) =
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

    let indexMoves (client:ElasticClient) report =
        let create success =
            let taken = success.TimeTaken
            {MoveRequest = success; Year = taken.Year; Month = taken.Month; Day = taken.Day; Hour = taken.Hour; DayOfWeek = Enum.GetName(taken.DayOfWeek.GetType(), taken.DayOfWeek)}

        report.Successes
        |> List.map(fun success -> client.Index(create success, fun idx -> idx.Index(processedMovesIndexName)))
        |> ignore


module ElasticReporter =

    open ProcessedMovesIndex
    open MoveResultsIndex
    open Rop

    let private reportToElastic' elasticUri report =
        let node = new Uri(elasticUri.ToString())
        let settings = new ConnectionSettings(node)
        let client = new ElasticClient(settings)

        let date = System.DateTime.Now

        createMoveResultsIndex client |> ignore
        createProcessedMovesIndex client |> ignore

        indexFilesInSourceDir client date report
        indexMoves client report

        ()

    let reportToElastic (elasticUri:Uri option) (report:Report) =
        match elasticUri with
        | Some uri -> reportToElastic' uri report
        | None -> ()