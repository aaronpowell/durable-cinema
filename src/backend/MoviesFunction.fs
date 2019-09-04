module MoviesFunction

open Microsoft.Azure.WebJobs
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.AspNetCore.Http

open Movies

let gev key = System.Environment.GetEnvironmentVariable(key, System.EnvironmentVariableTarget.Process)

[<FunctionName("Movies")>]
let getMovies ([<HttpTrigger(AuthorizationLevel.Function, "get", Route = "movies")>] req : HttpRequest) =
    let apiKey = gev "ApiKey"
    async {
        let! movies = getNowShowing apiKey
        let! config = getConfig apiKey

        let resultMovies = getMovieSummary movies config

        return OkObjectResult resultMovies
    } |> Async.StartAsTask

[<FunctionName("Movie")>]
let getMoveById ([<HttpTrigger(AuthorizationLevel.Function, "get", Route = "movies/{id}")>] req : HttpRequest) (id : int) =
    let apiKey = gev "ApiKey"
    async {
        let! movie = getMovieById apiKey id
        let! config = getConfig apiKey

        return getMovieDetails movie config |> OkObjectResult
    } |> Async.StartAsTask
