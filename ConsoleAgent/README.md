# Getting Started: AI Agents in C#

## Console Agent

A CLI agent that can help you plan what to wear - whatever the weather! Integrates with [weatherapi.com](https://www.weatherapi.com) to fetch the live weather in any city and works with three different AI providers via [Microsoft.Extensions.AI](https://learn.microsoft.com/en-us/dotnet/ai/microsoft-extensions-ai).

## API Keys
This project reads API keys from a local `.env` file (via `dotenv.net`) or environment variables. Create a file named `.env` in this directory with any providers you plan to use:

```
OPENAI_API_KEY=sk-proj-xxxxxxxxxxxxxxxxxxxxxxxx
CLAUDE_API_KEY=sk-ant-api03-xxxxxxxxxxxxxxxxx
GEMINI_API_KEY=xxxxxxxxxxxxxxxxxxxxxxxx
WEATHER_API_DOTCOM_KEY=xxxxxxxxxxxxxxxxxxxx
```

Notes:
- Do not commit `.env` to source control.
- You can also set these variables in your shell instead of using a file.

## Running Locally

```sh
cd ConsoleAgent

dotnet restore

# Run with CLI arguments to choose a provider and model.
# Use `--` to separate app args from `dotnet run` options.

dotnet run -- --provider openai --model gpt-4.1-mini

dotnet run -- --provider gemini --model gemini-2.0-flash-lite

dotnet run -- --provider claude --model claude-3-5-haiku-latest

# From the solution root, you can also run:
# dotnet run --project ConsoleAgent -- --provider openai --model gpt-4.1-mini
```

Defaults:
- Provider: `openai`
- Model: `gpt-4.1-mini`

## Using The Agent

Enter your messages at the prompt after running the app. The agent can call tools to fetch weather and suggest outfits.
