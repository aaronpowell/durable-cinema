module Theater
open Microsoft.Azure.WebJobs
open System.Threading.Tasks

type Theater =
    { Id : int
      Name : string
      Capacity : int }

let private theaters =
    [| { Id = 1
         Name = "Mega"
         Capacity = 200 }
       { Id = 2
         Name = "Standard 1"
         Capacity = 100 }
       { Id = 3
         Name = "Standard 2"
         Capacity = 100 }
       { Id = 4
         Name = "Standard 3"
         Capacity = 100 }
       { Id = 5
         Name = "Mini"
         Capacity = 50 } |]

let getTheaters = async { return theaters }
let getTheater id =
    async { return theaters |> Array.tryFind (fun c -> c.Id = id) }

type ITheatersForMovie =
    abstract member Init: Theater array -> unit
    abstract member Destroy: unit -> Task

type TheatersForMovie() =
    member val Theaters = Array.empty<Theater> with get, set

    [<FunctionName("TheatersForMovie")>]
    member __.Run([<EntityTrigger>] ctx : IDurableEntityContext) =
        ctx.DispatchAsync<TheatersForMovie>()

    interface ITheatersForMovie with
        member this.Init theaters =
            this.Theaters <- theaters

        member __.Destroy() =
            Task.CompletedTask

    member this.Init theaters =
        (this :> ITheatersForMovie).Init theaters

    member this.Destroy() =
        (this :> ITheatersForMovie).Destroy()

let getMovieEntityId (movieId: int) =
    EntityId("TheatersForMovie", movieId.ToString())