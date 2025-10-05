using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleAgent
{
    public static class ChatAgent
    {
        public static async Task RunAsync(IServiceProvider sp)
        {
            // Ottieni un'istanza di IChatClient dal contenitore di dipendenze
            var client = sp.GetRequiredService<IChatClient>();

            var chatOptions = sp.GetRequiredService<ChatOptions>();

            var history = new List<ChatMessage>
            {
                new(ChatRole.System, "You are a helpful CLI assistant")

            };

            Console.WriteLine($"Ask me anything (empty = exit).");

            while (true)
            {
                // i messaggi dell'utente sono di colore blu
                Console.ForegroundColor = ConsoleColor.Blue;
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    // Esci dal ciclo se l'input è vuoto
                    break;
                }
                Console.ResetColor();

                // Aggiungi il messaggio dell'utente alla cronologia
                history.Add(new ChatMessage(ChatRole.User, input));

                // Ottieni la risposta dal modello di chat
                var response = await client.GetResponseAsync(history, chatOptions);

                // Aggiungi la risposta del modello alla cronologia
                Console.WriteLine(response.Text);
                // posso ricevere più messaggi di risposta, quando i modelli userano dei tools questi
                // tools possono restituire più messaggi
                // ma la serie di messaggi di risposta terminera con un messaggio di ruolo assistant
                history.AddRange(response.Messages);
            }

        }
    }
}
