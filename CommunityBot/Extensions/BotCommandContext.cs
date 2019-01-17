using CommunityBot.Entities;
using CommunityBot.Features.GlobalAccounts;
using Discord.Commands;
using Discord.WebSocket;

namespace CommunityBot.Extensions
{
    public class BotCommandContext : SocketCommandContext
    {
        public GlobalUserAccount UserAccount { get; }
        public DiscordSocketClient Client { get; }
        
        public BotCommandContext(DiscordSocketClient client, SocketUserMessage msg, GlobalUserAccounts globalUserAccounts) : base(client, msg)
        {
            this.Client = client;

            UserAccount = User == null ? null : globalUserAccounts.GetUserAccount(User);
        }

    }
}
