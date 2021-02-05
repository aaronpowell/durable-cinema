## Durable Cinema

**This is a work in progress**

The goal of this is to make a demo application of using [Azure Functions](https://azure.microsoft.com/services/functions/?WT.mc_id=javascript-0000-aapowell) as a backend and [React](https://reactjs.org/) + [TypeScript](https://www.typescriptlang.org/) as the UI.

## Getting Started

The data for the app is provided by [The Movie DB](https://www.themoviedb.org/) and you'll need an API key for that. This will need to be set in either the `local.settings.json` or as an environment variable, named `ApiKey`.

Build and run the backend and then run the front end using `npm run start` from the `src/booking-form` folder.

## TODO

* Show the theaters which a movie is shown in
* Ticket booking process
  * Use a Durable Function to manage the process
  * Have the Durable Function time out if the ticket isn't booked in a particular time window
  * Simulate already-selected seats
  * Fake a payment flow