using System;
using System.Threading.Tasks;
using ConsoleAppFramework;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dimer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<Config>(hostContext.Configuration.GetSection("Config"));
                    services.AddSingleton<DiscordSocketClient>();
                    services.AddSingleton<CommandService>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ReplaceToSimpleConsole();
                    logging.SetMinimumLevel(LogLevel.Information);
                })
                .RunConsoleAppFrameworkAsync<Dimer>(args);
        }
    }
}
