module Movies

open TMDbLib.Client
open TMDbLib.Objects.General
open TMDbLib.Objects.Movies
open TMDbLib.Objects.Search
open System.Threading
open System

let private getClient apiKey = new TMDbClient(apiKey, true)

let getNowShowing apiKey =
    let client = getClient apiKey
    client.GetMovieNowPlayingListAsync(null, 1, "AU")

let getMovieById apiKey (id : int) =
    let client = getClient apiKey
    client.GetMovieAsync
        (id, MovieMethods.Credits ||| MovieMethods.Videos,
         CancellationToken.None)

let getConfig apiKey =
    let client = getClient apiKey
    client.GetConfigAsync()

type MovieSummary =
    { Title : string
      Id : int
      Overview : string
      Poster : string }

let getMovieSummary (movies : SearchContainerWithDates<SearchMovie>)
    (config : TMDbConfig) =
    movies.Results
    |> Seq.map
           (fun movie ->
           { Title = movie.Title
             Id = movie.Id
             Overview = movie.Overview
             Poster =
                 sprintf "%s%s%s" config.Images.SecureBaseUrl
                     config.Images.PosterSizes.[0] movie.PosterPath })

type Credited =
    { Id : int
      Character : string
      Actor : string
      Picture : string }

type Video =
    { Key : string
      Name : string
      Site : string }

type MovieDetails =
    { Id : int
      Title : string
      Overview : string
      Poster : string
      Genres : string seq
      Website : string
      Cast : Credited seq
      Videos : Video seq
      Runtime: Nullable<int> }

let getMovieDetails (movie : Movie) (config : TMDbConfig) =
    let posterSize sizes = sizes |> Seq.last
    let ps = posterSize config.Images.PosterSizes
    { Id = movie.Id
      Title = movie.Title
      Overview = movie.Overview
      Poster = sprintf "%s%s%s" config.Images.SecureBaseUrl ps movie.PosterPath
      Genres = movie.Genres |> Seq.map (fun g -> g.Name)
      Website = movie.Homepage
      Runtime = movie.Runtime
      Cast =
          movie.Credits.Cast
          |> Seq.map
                 (fun cast ->
                 let ps = posterSize config.Images.ProfileSizes
                 { Id = cast.Id
                   Character = cast.Character
                   Actor = cast.Name
                   Picture =
                       sprintf "%s%s%s" config.Images.SecureBaseUrl ps
                           cast.ProfilePath })
      Videos =
          movie.Videos.Results
          |> Seq.map (fun v ->
                 { Key = v.Key
                   Name = v.Name
                   Site = v.Site }) }
