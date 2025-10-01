using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using OpenAI;
using OpenAI.Chat;

// Crea il builder della configurazione
var builder = new ConfigurationBuilder()
    .AddUserSecrets<Program>() // permette di leggere i secrets
    .AddEnvironmentVariables();

var configuration = builder.Build();

// Recupera la chiave (UserSecrets o variabile d'ambiente)
var apiKey = configuration["OpenAI:ApiKey"] ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY");

// Usa la chiave per inizializzare il client OpenAI
if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.Error.WriteLine("Chiave API mancante. Imposta User Secrets 'OpenAI:ApiKey' o la variabile d'ambiente 'OPENAI_API_KEY'.");
    return;
}

// Inizializza il client OpenAI
// Questa riga inizializza il client generico OpenAIClient, che fornisce accesso
// a tutte le API (Chat, Embeddings, Audio, Images, ecc.)
// tramite un’unica istanza.
var api = new OpenAIClient(apiKey);
// Inizializza il client per il modello GPT-5 Nano
var client = api.GetChatClient("gpt-5-nano");


Console.WriteLine("OpenAI client inizializzato.");

// gli LLM sono "stateless", quindi dobbiamo mantenere noi il contesto
// non ricordano la cronologia delle conversazioni precedenti
// devo inviare io il contesto, tutti i messaggi precedenti
List<ChatMessage> messages =
[
    new AssistantChatMessage("Hello, what do you want to do today?") // messaggio iniziale del sistema
];

Console.WriteLine(messages[0].Content[0].Text);

// inizia il ciclo di chat, voglio un ciclo infinito all'interno della applicazione console
while (true)
{
    
}

