using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;

// Crea il builder della configurazione
// Nota: mettiamo prima le variabili d'ambiente e poi i UserSecrets
// così, in locale, i secrets hanno priorità sulle env (override esplicito).
var builder = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(); // permette di leggere i secrets

var configuration = builder.Build();

// Recupera la chiave (UserSecrets o variabile d'ambiente), con lookup robusto
// Ordine: UserSecrets "OpenAI:ApiKey" -> Env "OpenAI__ApiKey" (doppio underscore)
// -> Env "OPENAI_API_KEY". Viene applicato Trim.
var source = "";
var apiKey = configuration["OpenAI:ApiKey"]; // UserSecrets o config gerarchica
source = "UserSecrets/OpenAI:ApiKey";
if (string.IsNullOrWhiteSpace(apiKey))
{
    apiKey = configuration["OpenAI__ApiKey"]; // Env per chiave gerarchica
    source = "Env:OpenAI__ApiKey";
}
if (string.IsNullOrWhiteSpace(apiKey))
{
    apiKey = configuration["OPENAI_API_KEY"] ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY");
    source = "Env:OPENAI_API_KEY";
}
apiKey = apiKey?.Trim();

// Usa la chiave per inizializzare il client OpenAI
if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.Error.WriteLine(
        "Chiave API mancante. Imposta User Secrets 'OpenAI:ApiKey' (consigliato) con: \n" +
        "  dotnet user-secrets set \"OpenAI:ApiKey\" \"<chiave>\"\n" +
        "Oppure usa una variabile d'ambiente: 'OpenAI__ApiKey' (preferita) o 'OPENAI_API_KEY'.");
    return;
}

// Inizializza il client OpenAI
// Questa riga inizializza il client generico OpenAIClient, che fornisce accesso
// a tutte le API (Chat, Embeddings, Audio, Images, ecc.)
// tramite un'unica istanza.
var api = new OpenAIClient(apiKey);
// Inizializza il client per un modello supportato
var client = api.GetChatClient("gpt-4o-mini");


Console.WriteLine($"OpenAI client inizializzato. Sorgente chiave: {source}.");

// gli LLM sono "stateless", quindi dobbiamo mantenere noi il contesto
// non ricordano la cronologia delle conversazioni precedenti
// devo inviare io il contesto, tutti i messaggi precedenti
// Nota: qui inizializziamo la conversazione con un messaggio dell'assistente.
// Se vuoi definire istruzioni di sistema (ruolo "system"), usa invece SystemChatMessage.
List<ChatMessage> messages =
[
    new AssistantChatMessage("Hello, what do you want to do today?") // messaggio iniziale dell'assistente (greeting)
];

Console.WriteLine(messages[0].Content[0].Text);

// inizia il ciclo di chat, voglio un ciclo infinito all'interno della applicazione console
while (true)
{
    Console.ForegroundColor = ConsoleColor.Blue;
    var input = Console.ReadLine();
    if (input == null || input.ToLower() == "exit")
    {
        break;
    }
    Console.ResetColor();
    // aggiungo questo messaggio
    messages.Add(new UserChatMessage(input));

    ChatCompletion completion = client.CompleteChat(messages);
    var response = completion.Content[0].Text;

    messages.Add(new AssistantChatMessage(response));
    Console.WriteLine(response);
}
