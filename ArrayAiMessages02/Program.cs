
using ArrayAiMessages02.Models;
using ArrayAiMessages02.Services;
using dotenv.net;

// Load environment variables from .env in a robust way.
// Try common locations when running from bin/ or solution root.
var candidateEnvPaths = new[]
{
    Path.Combine(Directory.GetCurrentDirectory(), ".env"),
    Path.Combine(AppContext.BaseDirectory, ".env"),
    Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".env"))
};

var existingEnvPath = candidateEnvPaths.FirstOrDefault(File.Exists);
if (existingEnvPath != null)
{
    // Ensure .env values override any pre-set process variables for this run
    DotEnv.Load(new DotEnvOptions(envFilePaths: new[] { existingEnvPath }, overwriteExistingVars: true, trimValues: true));
}

var openAiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
static string Mask(string? s)
{
    if (string.IsNullOrEmpty(s)) return "<empty>";
    if (s.Length <= 12) return new string('*', s.Length);
    return s.Substring(0, 7) + new string('*', s.Length - 11) + s.Substring(s.Length - 4);
}
Console.ForegroundColor = ConsoleColor.DarkGray;
Console.WriteLine($"OPENAI_API_KEY source: {(existingEnvPath != null ? $".env -> {existingEnvPath}" : "Environment only")}, value: {Mask(openAiKey)}");
Console.ResetColor();
if (string.IsNullOrWhiteSpace(openAiKey))
{
    throw new InvalidOperationException("Missing OPENAI_API_KEY. Set the env var or create a .env file with OPENAI_API_KEY=<key>.");
}

List<ChatMessage> messages =
[
    new ChatMessage
    {
        Role = ChatRole.Assistant,
        Content = "Hello! How can I assist you today?"
    }
];

Console.WriteLine(messages[0].Content);
var aiService = new OpenAiService(openAiKey);

while (true)
{
    Console.ForegroundColor = ConsoleColor.Blue;
    var input = Console.ReadLine();
    if (input == null || input?.ToLower() == "exit")
    {
        break;
    }
    Console.ResetColor();

    messages.Add(new ChatMessage
    {
        Role = ChatRole.User,
        Content = input
    });

    var response = await aiService.CompleteChat(messages);
    messages.Add(response);
    Console.WriteLine(messages.Last().Content);
}
