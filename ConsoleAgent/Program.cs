using ConsoleAgent;
using dotenv.net;
using Microsoft.Extensions.Hosting;


DotEnv.Load();
// provider predefinito e model predefinito
string provider = "openai";
string model = "gpt-4.1-mini";

for (int i = 0; i < args.Length; i++)
{
    // Controlla se l'argomento è --provider o --model e assegna il valore successivo
    if (args[i] == "--provider" && i + 1 < args.Length)
    {
        provider = args[i + 1].ToLower();
    }

    if (args[i] == "--model" && i + 1 < args.Length)
    {
        model = args[i + 1];
    }

    var builder = Host.CreateApplicationBuilder(args);
    Startup.ConfigurationServices(builder, provider, model);

    var host = builder.Build();

    // Avvia l'agente di chat
    await ChatAgent.RunAsync(host.Services);
}