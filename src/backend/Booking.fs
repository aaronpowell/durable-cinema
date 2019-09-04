module Booking

open System
open Sessions

type Ticket =
    { Id: Guid
      Session: Session
      Seat: string }

type Booking =
    { Id: Guid
      Tickets: Ticket array
      Name: string
      Email: string }