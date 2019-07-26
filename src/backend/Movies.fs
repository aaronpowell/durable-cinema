module Movies

open TMDbLib.Client
open TMDbLib.Objects.General
open TMDbLib.Objects.Search

let private getClient apiKey =
    new TMDbClient(apiKey, true)

let getNowShowing apiKey =
    let client = getClient apiKey
    client.GetMovieNowPlayingListAsync(null, 1, "AU") |> Async.AwaitTask

let getConfig apiKey =
    let client = getClient apiKey
    client.GetConfigAsync() |> Async.AwaitTask

type MovieDetails =
    { Title: string
      Id: int
      Overview: string
      Poster: string }

let getMovieDetails (movies: SearchContainerWithDates<SearchMovie>) (config: TMDbConfig) =
    movies.Results
    |> Seq.map (fun movie ->
        { Title = movie.Title
          Id = movie.Id
          Overview = movie.Overview
          Poster = sprintf "%s%s%s" config.Images.SecureBaseUrl config.Images.PosterSizes.[0] movie.PosterPath })