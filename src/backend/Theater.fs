module Theater

type Theater =
    { Id: int
      Name: string
      Capacity: int }

let private theaters = [|
    { Id = 1
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
      Capacity = 50 }
|]

let getTheaters() = theaters

let getTheater id =
    theaters |> Array.tryFind (fun c -> c.Id = id)