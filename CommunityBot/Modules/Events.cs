using CommunityBot.Extensions;
using CommunityBot.Features.GlobalAccounts;
using CommunityBot.Helpers;
using CommunityBot.Preconditions;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityBot.Modules
{
    [Group("event"), Summary("Event management")]
    [RequireContext(ContextType.Guild)]
    public class Events : ModuleBase<BotCommandContext>
    {
        private readonly GlobalGuildAccounts _globalGuildAccounts;
        public Events(GlobalGuildAccounts globalGuildAccounts)
        {
            _globalGuildAccounts = globalGuildAccounts;
        }

        [Command("", RunMode = RunMode.Async), Alias("create"), Summary("Create an event")]
        public async Task Create()
        {
            await ReplyAsync("Event creation instructions have been messaged to you.");

            //await Context.Channel.SendMessageAsync("Check your DMs.");

            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();

            await dmChannel.SendMessageAsync("Do you like this?");
            var temp = (await dmChannel.AwaitMessage(msg => EvaluateResponse(msg, "yes", "no"))).Content;
            await dmChannel.SendMessageAsync("Answer: " + temp);

            bool EvaluateResponse(SocketMessage arg, params String[] options)
                => options.Any(option => arg.Content.ToLower().Contains(option.ToLower()) && arg.Author.Id == Context.User.Id);

            await dmChannel.SendMessageAsync("Enter the event title:");

            Console.WriteLine("Waiting for reply");
            var reply = await dmChannel.AwaitMessage(null, 60000);
            Console.WriteLine("Reply received");
            await dmChannel.SendMessageAsync("You entered: " + reply.Content);
            Console.WriteLine("Reply: " + reply.Content);
        }

    }
}
