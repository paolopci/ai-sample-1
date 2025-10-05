using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsoleAgent
{
    public static class Startup
    {
        // Metodo per configurare i servizi
        public static void ConfigurationServices(HostApplicationBuilder builder, string provider, string model)
        {
            // Aggiungi i servizi necessari al contenitore di dipendenze
            builder.Services.AddLogging(logging => logging.AddConsole().SetMinimumLevel(LogLevel.Information));

            // da qualsiasi punto dell'applicazione puoi ottenere un'istanza di ILoggerFactory
            builder.Services.AddSingleton<ILoggerFactory>(sp =>
                LoggerFactory.Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Information)));
        }
    }
}
