using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ArrayAiMessages02.Models;

namespace ArrayAiMessages02.Services
{
    public class OpenAiService
    {
        // Creiamo un'istanza di HttpClient per fare richieste HTTP
        private readonly HttpClient _httpClient = new();

        public OpenAiService(string apiKey)
        {
            // Impostiamo l'URL base per le richieste all'API di OpenAI, per un API aperta è "https://api.openai.com/v1/"
            _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
            // Impostiamo l'header di autorizzazione con il token API
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            // Impostiamo l'header per accettare risposte in formato JSON, l'API di OpenAI risponde in JSON
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // Aggiungiamo il convertitore per serializzare/deserializzare gli enum come stringhe in camelCase
            // Esempio: ChatRole.User diventa "user", ChatRole.System diventa "system"
            _jsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

        }

        public async Task<ChatMessage> CompleteChat(List<ChatMessage> messages,
            CancellationToken cancellationToken = default)
        {
            var openAiRequest = new ChatRequest()
            {
                Model = "gpt-5-mini",
                Messages = messages
            };
            var jsonRequest = JsonSerializer.Serialize(openAiRequest, _jsonOptions);

            using var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("chat/completions", content, cancellationToken);
                // Leggiamo la risposta come stringa
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                if (!response.IsSuccessStatusCode)
                { // Se la risposta non è andata a buon fine, lanciamo un'eccezione
                    throw new InvalidOperationException($"Errore nella chiamata all'API di OpenAI: {response.StatusCode}, {responseContent}");
                }
                // Deserializziamo la risposta JSON in un oggetto ChatResponse
                var result = JsonSerializer.Deserialize<ChatResponse>(responseContent, _jsonOptions) ??
                             throw new InvalidOperationException("Risposta non valida dall'API di OpenAI");

                // Prendiamo la prima scelta dalla risposta (se ce n'è più di una)
                var firstChoise = result.Choices?.FirstOrDefault();
                if (firstChoise == null || firstChoise.Message == null)
                {
                    throw new InvalidOperationException("Nessuna scelta valida nella risposta dell'API di OpenAI");
                }

                return new ChatMessage
                {
                    Role = firstChoise.Message.Role,
                    Content = firstChoise.Message.Content,
                };

            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException("Error calling OpenAI API", ex);
            }
        }


        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            // Trasforma i nomi delle proprietà in snake_case (cioè con underscore _ tra le parole).
            // Tutte le lettere diventano minuscole. Esempio: "myPropertyName" diventa "my_property_name"
            // perché l'API di OpenAI usa questo formato per i nomi delle proprietà
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            // Ignoriamo le proprietà con valore null durante la serializzazione
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }
}