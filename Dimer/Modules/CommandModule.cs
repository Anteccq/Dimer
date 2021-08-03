using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dimer.Models;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Logging;

namespace Dimer.Modules
{
    public class CommandModule : ModuleBase
    {
        private const string TimerInvalidMessage = ":x: `!timer 180 [message]`";
        private const string TimerNotFoundMessage = "Timer Not Found.";

        private readonly ITimeManager _timerManager;
        private readonly ILogger _logger;

        public CommandModule(ITimeManager timerManager, ILogger<CommandModule> logger)
        {
            _timerManager = timerManager;
            _logger = logger;
        }

        [Command("dimer")]
        public async Task Dimer(params string[] args)
        {
            if (args is null || args.Length < 1)
            {
                await ReplyAsync(TimerInvalidMessage);
                return;
            }

            if (!TimeParser.TryParse(args[0], out var time))
            {
                await ReplyAsync(TimerInvalidMessage);
                return;
            }

            var message = "";
            if(args.Length > 1)
                message = args.Skip(1).Aggregate((a, b) => $"{a} {b}");

            var timer = new MessageTimer(time)
            {
                Message = message
            };
            var timerId = _timerManager.Add(timer);
            var now = DateTime.UtcNow;
            _logger.LogDebug($"Receipt: {now} {now.Millisecond}");
            await ReplyAsync($"{new Emoji("⏲️")} {timerId}");
            timer.Start(async x =>
            {
                var eventTime = DateTime.UtcNow;
                _logger.LogDebug($"Send: {eventTime} {eventTime.Millisecond}");
                await ReplyAsync($"{Context.User.Mention} {x}");
            });
            timer.Finished += () => _timerManager.TryRemoveIfExists(timerId);
        }

        [Command("dimer-r")]
        public async Task RemoveTimer([Summary("TimerId (number)")]int id)
        {
            if (!_timerManager.Exists(id)) await ReplyAsync(TimerNotFoundMessage);
            var timer = _timerManager.Find(id);
            timer?.Cancel();
            _timerManager.TryRemove(id);
            await Context.Message.AddReactionAsync(new Emoji("👌"));
        }
    }
}
