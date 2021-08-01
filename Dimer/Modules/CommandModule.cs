using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dimer.Models;
using Discord.Commands;

namespace Dimer.Modules
{
    public class CommandModule : ModuleBase
    {
        private const string TimerInvalidMessage = ":x: `!timer 180 [message]`";
        [Command("timer")]
        public async Task Timer(params string[] args)
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

            await ReplyAsync(time.ToString());

            var message = "";
            if(args.Length > 1)
                message = args.Skip(1).Aggregate((a, b) => $"{a} {b}");

            var timer = new MessageTimer(time)
            {
                Message = message
            };
            timer.Start(async x => await ReplyAsync($"{Context.User.Mention} {x}"));
        }
    }
}
