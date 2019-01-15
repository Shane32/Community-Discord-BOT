﻿using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CommunityBot.Extensions;
using Discord.Commands;
using Discord.WebSocket;
using CommunityBot.Features.GlobalAccounts;

namespace CommunityBot.Handlers
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _cmdService;
        private readonly IServiceProvider _serviceProvider;
        private readonly GlobalGuildAccounts _globalGuildAccounts;
        private readonly GlobalUserAccounts _globalUserAccounts;

        public CommandHandler(DiscordSocketClient client, CommandService cmdService, IServiceProvider serviceProvider, GlobalGuildAccounts globalGuildAccounts, GlobalUserAccounts globalUserAccounts)
        {
            _client = client;
            _cmdService = cmdService;
            _serviceProvider = serviceProvider;
            _globalGuildAccounts = globalGuildAccounts;
            _globalUserAccounts = globalUserAccounts;
        }

        public async Task InitializeAsync()
        {
            await _cmdService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
            Global.Client = _client;
        }

        public async Task HandleCommandAsync(SocketMessage s)
        {
            if (!(s is SocketUserMessage msg)) { return; }
            if (msg.Channel is SocketDMChannel) { return; }
            if (msg.Author.IsBot) { return; }
            var context = new BotCommandContext(_client, msg, _globalUserAccounts);

            var argPos = 0;
            if (msg.HasMentionPrefix(_client.CurrentUser, ref argPos) || CheckPrefix(ref argPos, context))
            {
                var cmdSearchResult = _cmdService.Search(context, argPos);
                if (!cmdSearchResult.IsSuccess) { return; }
                
                context.RegisterCommandUsage();
                
                var executionTask = _cmdService.ExecuteAsync(context, argPos, _serviceProvider);

                #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                executionTask.ContinueWith(task =>
                {
                    if (task.Result.IsSuccess || task.Result.Error == CommandError.UnknownCommand) return;
                    const string errTemplate = "{0}, Error: {1}.";
                    var errMessage = string.Format(errTemplate, context.User.Mention, task.Result.ErrorReason);
                    context.Channel.SendMessageAsync(errMessage);
                });
                #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

        private bool CheckPrefix(ref int argPos, SocketCommandContext context)
        {
            if (context.Guild is null) return false;
            var prefixes = _globalGuildAccounts.GetGuildAccount(context.Guild.Id).Prefixes;
            var tmpArgPos = 0;
            var success = prefixes.Any(pre =>
            {
                if (!context.Message.Content.StartsWith(pre)) return false;
                tmpArgPos = pre.Length;
                return true;
            });
            argPos = tmpArgPos;
            return success;
        }
    }
}
