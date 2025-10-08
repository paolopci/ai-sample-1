using dotenv.net;
using GeminiDotnet.ContentGeneration;
using InvoiceAgentApi;
using Microsoft.Extensions.AI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    // Consente richieste da localhost/127.0.0.1 per agevolare lo sviluppo front-end locale.
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
        {
            if (string.IsNullOrWhiteSpace(origin)) return false;
            try
            {
                var uri = new Uri(origin);
                return uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase) ||
                       uri.Host.Equals("127.0.0.1");
            }
            catch
            {
                return false;
            }
        })
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.WebHost.ConfigureKestrel(options =>
{
    // Forza Kestrel ad ascoltare su qualsiasi interfaccia alla porta 5001.
    options.ListenAnyIP(5001);
});

// Carica le variabili d'ambiente definite in .env (se presente).
DotEnv.Load();

string provider = "openai";
string model = "gpt-5";
for (int i = 0; i < args.Length; i++)
{
    if (args[i] == "--provider" && i + 1 < args.Length)
        provider = args[i + 1].ToLower();
    if (args[i] == "--model" && i + 1 < args.Length)
        model = args[i + 1];
}

Startup.ConfigureServices(builder, provider, model);

var systemPromptPath = Path.Combine(AppContext.BaseDirectory, "SystemPrompt.txt");
var systemPrompt = File.ReadAllText(systemPromptPath);

var app = builder.Build();

// Applica la policy CORS per consentire le chiamate dal client autorizzato.
app.UseCors("AllowLocalhost");


app.MapPost("/chat", async (
    List<ChatMessage> messages,
    IChatClient client,
    ChatOptions chatOptions) =>
{
    // Inietta la data corrente nel prompt di sistema prima di inviare la richiesta al modello.
    var systemPromptWithDate = systemPrompt + "\n By the way today's date is " + DateTime.Now.ToLongDateString();
    var withSystemPrompt = (new[] { new ChatMessage(ChatRole.System, systemPromptWithDate) })
                            .Concat(messages)
                            .ToList();

    // Ottiene la risposta dal provider AI e restituisce i messaggi generati.
    var response = await client.GetResponseAsync(withSystemPrompt, chatOptions);
    return Results.Ok(response.Messages);
});

app.Run();
