module HttpTrigger

open Microsoft.Azure.WebJobs
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.AspNetCore.Http
open System.IO
open System.Text

[<FunctionName("HttpTrigger")>]
let httpTriggerFunction ([<HttpTrigger(AuthorizationLevel.Function, "post", Route = null)>] req : HttpRequest) =
    async {
        use reader = new StreamReader(req.Body, Encoding.UTF8)
        let! body = reader.ReadToEndAsync() |> Async.AwaitTask
        return OkObjectResult body
    } |> Async.StartAsTask
