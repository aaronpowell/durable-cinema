module Sessions

open System
open Movies
open Theater

type Session =
    { Id: Guid
      StarTime: DateTimeOffset
      Movie: MovieDetails
      Theater: Theater }
