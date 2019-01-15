﻿using CommunityBot.Entities;
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

            if (User is null) { return; }

            UserAccount = globalUserAccounts.GetUserAccount(User);
        }

        public void RegisterCommandUsage()
        {
            var commandUsedInformation = new CommandInformation(Message.Content, Message.CreatedAt.DateTime);
            
            UserAccount.AddCommandToHistory(commandUsedInformation);

            _globalUserAccounts.SaveAccounts(UserAccount.Id);
        }
    }
}
