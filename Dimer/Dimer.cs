using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ConsoleAppFramework;
using Dimer.Extensions;
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
            if (string.IsNullOrEmpty(_config.Value.DiscordToken))
            {
                _logger.LogCritical("Invalid Discord Token.");
                return;
            }
            _client.Log += m =>
            {
                _logger.Log(m.Severity.ToLogLevel(), m.Exception, m.Message);
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
            var result = await _command.ExecuteAsync(context, argPos, null);
            if (!result.IsSuccess && result is ExecuteResult executeResult)
                _logger.LogError(executeResult.Exception, executeResult.Exception.Message);
        }
    }
}
