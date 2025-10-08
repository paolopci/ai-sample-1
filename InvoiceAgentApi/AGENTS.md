# Repository Guidelines

## Project Structure & Module Organization
`AIproject25.sln` ties together three console projects: `AIproject25/` (main configuration-driven entry point), `ArrayAiMessages02/`, and `ConsoleAgent/`. Source files live under each project folder; avoid editing generated `bin/` or `obj/`. Place shared utilities in the project that consumes them. Add tests under `AIproject25.Tests/` (create via `dotnet new xunit -n AIproject25.Tests -o AIproject25.Tests`) and name files `*Tests.cs`.

## Build, Test, and Development Commands
- `dotnet restore AIproject25.sln` – install NuGet dependencies.
- `dotnet build AIproject25.sln -c Debug` – compile all projects with DEBUG symbols.
- `dotnet run --project AIproject25` (or `ArrayAiMessages02`, `ConsoleAgent -- --provider openai --model gpt-5-mini`) – execute the chosen console app.
- `dotnet format` – enforce style conventions before committing.
- `dotnet test` – run the solution test suite once `AIproject25.Tests` exists.

## Coding Style & Naming Conventions
Target C# 12 on .NET 8/9 with nullable and implicit usings enabled. Use four-space indentation, keep lines ≤120 chars, and prefer `var` when the inferred type is obvious. Follow PascalCase for types/methods, camelCase for locals/parameters, and `_camelCase` for private fields. Expression-bodied members are encouraged for simple accessors.

## Testing Guidelines
Adopt xUnit for unit coverage; aim for ≥80% statements on public APIs. Test classes should end with `Tests`, with one fixture per logical unit. Use descriptive method names like `MethodName_WhenCondition_ShouldOutcome`. Execute `dotnet test` locally before submitting.

## Commit & Pull Request Guidelines
Write commits in imperative mood, ideally in Conventional Commit form (`feat(console-agent): ...`). Each PR should describe scope, reference related issues (`Fixes #123`), and include validation steps (build, tests, sample runs). Attach relevant logs or screenshots for behavioral changes.

## Security & Configuration Tips
Store secrets in .NET User Secrets (`OpenAI:ApiKey`) or project-specific `.env` files ignored by Git. Do not commit API keys; rotate and purge if leaked. Verify environment variables (`OPENAI_API_KEY`, `CLAUDE_API_KEY`, etc.) before running ConsoleAgent.
