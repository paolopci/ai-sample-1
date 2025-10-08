# InvoiceApp Web Application

InvoiceApp is a C# ASP.NET Core web application for submitting and reviewing invoices. It acts as the web front end for the [InvoiceAgentApi](../InvoiceAgentApi/README.md) AI agent. When the API is running at `http://localhost:5000`, the UI delegates invoice classification tasks directly to the agent.

## Prerequisites
- .NET 8 SDK or later
- Entity Framework Core CLI: `dotnet tool install --global dotnet-ef`
- LibMan CLI (installs automatically with the .NET SDK, but can be restored explicitly with `dotnet tool restore`)
- SQLite installed locally (optional; the bundled provider will create `Invoices.db` automatically)

> Tip: run all commands from the `InvoiceApp` directory.

## Getting Started

### 1. Restore Dependencies
- Backend dependencies are resolved automatically during `dotnet build` or `dotnet run`.
- Frontend libraries (Alpine.js) are downloaded with [LibMan](https://learn.microsoft.com/aspnet/core/client-side/libman):

```sh
libman restore
```

`wwwroot/lib` stays out of source control (aside from the provided placeholders) and is repopulated through LibMan when needed.

### 2. Apply Database Migrations

```sh
dotnet ef database update
```

This creates (or upgrades) the local SQLite database at `Invoices.db`.

### 3. Run the App

```sh
dotnet run
```

The development server listens on [http://localhost:5000](http://localhost:5000). Use `dotnet watch run` for automatic recompilation while developing.

### 4. Connect to InvoiceAgentApi
Make sure the companion API project is running locally. When both apps are active, the UI surfaces an agent assistant that calls the API for invoice insights.

## Project Structure
- `Pages/` – Razor pages for the UI
- `Controllers/` – REST endpoints consumed by the frontend
- `Models/` – Entity Framework Core models
- `Data/` – EF Core context and migrations
- `wwwroot/` – Static assets (CSS, JS, LibMan-managed libraries)
- `libman.json` – LibMan configuration for frontend packages

## Database Tips
- Add new EF migrations with `dotnet ef migrations add <MigrationName>`.
- Remove a bad migration using `dotnet ef migrations remove` before it is applied.
- Keep `Invoices.db` out of source control; it is local-only data.

## Testing
The solution does not ship with automated tests yet. If you add tests, organise them in a sibling test project (e.g., `InvoiceApp.Tests`) and run them with:

```sh
dotnet test
```

## Troubleshooting
- Agent popup missing: refresh the page after the InvoiceAgentApi is reachable.
- `SqliteException: SQLite Error 1: 'no such table: Invoices'`: run `dotnet ef database update`.
- LibMan errors: ensure the CLI is installed (`dotnet tool list --global`) and re-run `libman restore`.

## Notes
- Tailwind CSS is pulled via CDN in development. For production hardening, consider bundling Tailwind with a purge pipeline.
- Alpine.js is restored via LibMan for lightweight interactivity.
- Configuration and secrets remain local; do not commit API keys or connection strings.
