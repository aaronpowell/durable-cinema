module Sessions

open System

type Session =
    { Id: Guid
      StarTime: DateTimeOffset
      MovieId: int
      TheaterId: int }
