# Repository Guidelines

This repository contains a .NET multi-project console solution. Use the guidance below to develop, test, and contribute efficiently.

## Project Structure & Module Organization
- Projects
  - `AIproject25/` - entry point: `AIproject25/Program.cs`
  - `ArrayAiMessages02/` - entry point: `ArrayAiMessages02/Program.cs`
  - `ConsoleAgent/` - entry point: `ConsoleAgent/Program.cs`
- Solution: `AIproject25.sln` (includes all three projects)
- Build outputs: `*/bin/`, `*/obj/` (generated; do not edit)
- Packages: managed via NuGet in each project (`AIproject25/AIproject25.csproj`, `ArrayAiMessages02/ArrayAiMessages02.csproj`, `ConsoleAgent/ConsoleAgent.csproj`)
  - Notable:
    - AIproject25: `OpenAI`, `Microsoft.Extensions.Configuration.UserSecrets`, `Microsoft.Extensions.Configuration.*`
    - ArrayAiMessages02: `dotenv.net`
    - ConsoleAgent: `Microsoft.Extensions.AI`, `Microsoft.Extensions.AI.OpenAI`, `Anthropic.SDK`, `GeminiDotnet.Extensions.AI`, `dotenv.net`, `Microsoft.Extensions.Hosting`

## Build, Run, and Format
- Restore: `dotnet restore AIproject25.sln`
- Build: `dotnet build AIproject25.sln -c Debug`
- Run (AIproject25): `dotnet run --project AIproject25`
- Run (ArrayAiMessages02): `dotnet run --project ArrayAiMessages02`
- Run (ConsoleAgent):
  - Basic: `dotnet run --project ConsoleAgent -- --provider openai --model gpt-5-mini`
  - Other examples: `--provider gemini --model gemini-2.0-flash-lite`, `--provider claude --model claude-3-5-haiku-latest`
- Format: `dotnet format` (install dotnet-format if needed)

## Configuration & Secrets
- Never commit secrets or keys. Prefer User Secrets (local) and environment variables (CI/CD).
- AIproject25 (uses .NET configuration):
  - Preferred: .NET User Secrets key `OpenAI:ApiKey` in the `AIproject25` project directory:
    - `dotnet user-secrets init`
    - `dotnet user-secrets set "OpenAI:ApiKey" "<your_key>"`
  - Env var fallbacks supported by the app: `OpenAI__ApiKey` or `OPENAI_API_KEY`.
- ArrayAiMessages02 (uses dotenv):
  - Reads `OPENAI_API_KEY` from environment or a local `.env` file. Example `.env` line: `OPENAI_API_KEY=...`
  - `.env` is already ignored by `.gitignore`. Do not commit `.env`. If committed previously, rotate the key and purge from history.
- ConsoleAgent (uses dotenv + multiple providers):
  - Create `ConsoleAgent/.env` with the keys you plan to use:
    - `OPENAI_API_KEY=...`
    - `CLAUDE_API_KEY=...`
    - `GEMINI_API_KEY=...`
    - `WEATHER_API_DOTCOM_KEY=...`
  - Alternatively, set env vars in your shell without a file.
- Quick env examples:
  - PowerShell: `$env:OPENAI_API_KEY="<your_key>"`; `$env:CLAUDE_API_KEY="<your_key>"`
  - Bash/zsh: `export OPENAI_API_KEY="<your_key>"`; `export CLAUDE_API_KEY="<your_key>"`

## Testing Guidelines
- Framework: xUnit (recommended)
- Structure: separate test project `AIproject25.Tests/`; files named `*Tests.cs`, classes `ClassNameTests`
- Create tests: `dotnet new xunit -n AIproject25.Tests -o AIproject25.Tests` then `dotnet sln add AIproject25.Tests/AIproject25.Tests.csproj`
- Run tests: `dotnet test`
- Aim for meaningful unit tests around public APIs; target >=80% statement coverage where practical

## Coding Style & Naming Conventions
- C# 12 on .NET 8/9 (solution mixes targets); nullable enabled and implicit usings on
- Indentation: 4 spaces; max line length ~120
- Naming: PascalCase (types, methods, properties), camelCase (locals, parameters), `_camelCase` (private fields), `I` prefix for interfaces
- Use expression-bodied members for simple getters; prefer `var` when the type is obvious

## Commit & Pull Request Guidelines
- Commits: imperative, scoped; prefer Conventional Commits (e.g., `feat(core): add CLI args`)
- PRs: clear description, linked issues (`Fixes #123`), steps to validate, and any relevant logs/output. Ensure CI builds and tests pass.

## Agent-Specific Instructions
- Keep edits minimal and focused; avoid unrelated refactors
- Follow this guide's conventions; update this file when adding commands or structure
- Do not add license headers; keep changes consistent with existing style

