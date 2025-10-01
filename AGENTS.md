# Repository Guidelines

This repository contains a .NET 8 console application. Use the guidance below to develop, test, and contribute efficiently.

## Project Structure & Module Organization
- Source: `AIproject25/` (entry point: `AIproject25/Program.cs`)
- Solution: `AIproject25.sln`
- Build outputs: `AIproject25/bin/`, `AIproject25/obj/` (generated; do not edit)
- Packages: managed via NuGet in `AIproject25/AIproject25.csproj` (e.g., `OpenAI`, `Microsoft.Extensions.Configuration.UserSecrets`)

## Build, Test, and Development Commands
- Restore: `dotnet restore AIproject25.sln` — restore NuGet packages
- Build: `dotnet build AIproject25.sln -c Debug` — compile the solution
- Run: `dotnet run --project AIproject25` — start the console app
- Format: `dotnet format` — apply code style (install dotnet-format if needed)
- Tests: `dotnet test` — runs tests when a test project exists (see below)

## Coding Style & Naming Conventions
- C# 12 on .NET 8; nullable enabled and implicit usings on
- Indentation: 4 spaces; max line length ~120
- Naming: PascalCase (types, methods, properties), camelCase (locals, parameters), `_camelCase` (private fields), `I` prefix for interfaces
- Use expression-bodied members for simple getters; prefer `var` when the type is obvious

## Testing Guidelines
- Framework: xUnit (recommended)
- Structure: separate test project `AIproject25.Tests/`; files named `*Tests.cs`, classes `ClassNameTests`
- Create tests: `dotnet new xunit -n AIproject25.Tests -o AIproject25.Tests` then `dotnet sln add AIproject25.Tests/AIproject25.Tests.csproj`
- Run tests: `dotnet test`
- Aim for meaningful unit tests around public APIs; target ≥80% statement coverage where practical

## Commit & Pull Request Guidelines
- Commits: imperative, scoped; prefer Conventional Commits (e.g., `feat(core): add CLI args`)
- PRs: clear description, linked issues (`Fixes #123`), steps to validate, and any relevant logs/output. Ensure CI builds and tests pass.

## Security & Configuration Tips
- Never commit secrets or keys. Use .NET User Secrets for local dev: `dotnet user-secrets set OpenAI:ApiKey "..."` in the `AIproject25` project
- Prefer environment variables in CI/CD; document required keys in PRs that introduce them

## Agent-Specific Instructions
- Keep edits minimal and focused; avoid unrelated refactors
- Follow this guide’s conventions; update this file when adding commands or structure
- Do not add license headers; keep changes consistent with existing style

