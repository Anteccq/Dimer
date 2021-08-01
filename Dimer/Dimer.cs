using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ConsoleAppFramework;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dimer
{
    public class Dimer : ConsoleAppBase
    {
        private readonly IOptions<Config> _config;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _command;
        private readonly ILogger _logger;
        private const char CommandPrefix = '!';

        public Dimer(IOptions<Config> config, DiscordSocketClient client, CommandService command, ILogger<Dimer> logger)
        {
            _config = config;
            _client = client;
            _command = command;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _client.Log += m =>
            {
                _logger.Log(ToLogLevel(m.Severity), m.Exception, m.Message);
                return Task.CompletedTask;
            };
            _client.MessageReceived += MessageHandle;
            await _command.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            await _client.LoginAsync(TokenType.Bot, _config.Value.DiscordToken);
            await _client.StartAsync();

            await Task.Delay(-1, Context.CancellationToken);

            await _client.StopAsync();
        }

        private async Task MessageHandle(SocketMessage message)
        {
            if (message is not SocketUserMessage msg || msg.Author.IsBot) return;
            var argPos = 0;
            if(!(msg.HasCharPrefix(CommandPrefix, ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;
            var context = new CommandContext(_client, msg);
            await _command.ExecuteAsync(context, argPos, null);
        }

        private LogLevel ToLogLevel(LogSeverity severity)
           => severity switch
            {
                LogSeverity.Critical => LogLevel.Critical,
                LogSeverity.Warning => LogLevel.Warning,
                LogSeverity.Error => LogLevel.Error,
                LogSeverity.Debug => LogLevel.Debug,
                LogSeverity.Info => LogLevel.Information,
                LogSeverity.Verbose => LogLevel.Trace,
                _ => LogLevel.None
            };
    }
}
