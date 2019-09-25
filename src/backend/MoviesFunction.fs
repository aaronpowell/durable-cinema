module MoviesFunction

open Microsoft.Azure.WebJobs
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.AspNetCore.Http
open System.Threading
open Movies
open FSharp.Control.Tasks.V2
open Microsoft.Extensions.Logging

let gev key =
    System.Environment.GetEnvironmentVariable
        (key, System.EnvironmentVariableTarget.Process)

type INowShowing =
    abstract RefreshMovies : MovieSummary seq -> unit

type NowShowing() =
    member val Movies = Seq.empty<MovieSummary> with get, set

    interface INowShowing with
        member this.RefreshMovies movies = this.Movies <- movies

    [<FunctionName("NowShowingEntityTrigger")>]
    member __.Run([<EntityTrigger>] ctx : IDurableEntityContext) =
        ctx.DispatchAsync<NowShowing>()

    member this.RefreshMovies movies =
        (this :> INowShowing).RefreshMovies movies

[<Literal>]
let MOVIE_ORCHESTRATOR_NAME = "NowShowing"

[<FunctionName("Movies")>]
let getMovies ([<HttpTrigger(AuthorizationLevel.Function, "get",
                             Route = "movies")>] req : HttpRequest)
    ([<DurableClient>] client : IDurableClient) =
    let apiKey = gev "ApiKey"
    let entityId = EntityId("NowShowing", "Cache")
    task {
        let! nowShowing = client.ReadEntityStateAsync<NowShowing>(entityId)
        match nowShowing.EntityExists with
        | true -> return OkObjectResult nowShowing.EntityState
        | false ->
            let! movies = getNowShowing apiKey
            let! config = getConfig apiKey
            let resultMovies = getMovieSummary movies config
            let! _ = client.StartNewAsync(MOVIE_ORCHESTRATOR_NAME, resultMovies)
            return OkObjectResult resultMovies
    }

[<FunctionName("Movie")>]
let getMoveById ([<HttpTrigger(AuthorizationLevel.Function, "get",
                               Route = "movies/{id}")>] req : HttpRequest)
    (id : int) =
    let apiKey = gev "ApiKey"
    task { let! movie = getMovieById apiKey id
           let! config = getConfig apiKey
           return getMovieDetails movie config |> OkObjectResult }

[<FunctionName(MOVIE_ORCHESTRATOR_NAME)>]
let manageNowShowingTimeout ([<OrchestrationTrigger>] ctx : IDurableOrchestrationContext)
    ([<DurableClient>] client : IDurableClient) (log : ILogger) =
    task {
        log.LogInformation "Starting background job"
        let movies = ctx.GetInput<seq<MovieSummary>>()
        let entityId = EntityId("NowShowing", "Cache")
        do! client.SignalEntityAsync<INowShowing>
                (entityId,
                 fun (proxy : INowShowing) -> proxy.RefreshMovies movies)
        let timer =
            ctx.CreateTimer
                (ctx.CurrentUtcDateTime.AddMinutes(1.), CancellationToken.None)
        do! timer
        log.LogInformation "Orchestrator shutting down"
        do! client.TerminateAsync(entityId.ToString(), "Cache clearing")
        return ignore()
    }
