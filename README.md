## Durable Cinema

**This is a work in progress**

The goal of this is to make a demo application of using [Azure Functions](https://azure.microsoft.com/en-us/services/functions/?WT.mc_id=durablecinema-github-aapowell) as a backend and [React](https://reactjs.org/) + [TypeScript](https://www.typescriptlang.org/) as the UI.

## Getting Started

The data for the app is provided by [The Movie DB](https://www.themoviedb.org/) and you'll need an API key for that. This will need to be set in either the `local.settings.json` or as an environment variable, named `ApiKey`.

Build and run the backend and then run the front end using `npm run start` from the `src/booking-form` folder.