module MoviesFunction

open Microsoft.Azure.WebJobs
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.AspNetCore.Http

open Movies

let gev key = System.Environment.GetEnvironmentVariable(key, System.EnvironmentVariableTarget.Process)

[<FunctionName("Movies")>]
let httpTriggerFunction ([<HttpTrigger(AuthorizationLevel.Function, "get", Route = "movies")>] req : HttpRequest) =
    let apiKey = gev "ApiKey"
    async {
        let! movies = getNowShowing apiKey
        let! config = getConfig apiKey

        let resultMovies = getMovieDetails movies config

        return OkObjectResult resultMovies
    } |> Async.StartAsTask
