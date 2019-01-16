using CommunityBot.Entities;
using CommunityBot.Features.GlobalAccounts;
using Discord.Commands;
using Discord.WebSocket;

namespace CommunityBot.Extensions
{
    public class BotCommandContext : SocketCommandContext
    {
        public GlobalUserAccount UserAccount { get; }
        private readonly GlobalUserAccounts _globalUserAccounts;
        
        public BotCommandContext(DiscordSocketClient client, SocketUserMessage msg, GlobalUserAccounts globalUserAccounts) : base(client, msg)
        {
            this._globalUserAccounts = globalUserAccounts;

            UserAccount = User == null ? null : globalUserAccounts.GetUserAccount(User);
        }

    }
}
