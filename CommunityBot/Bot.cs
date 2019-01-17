using CommunityBot.Configuration;
using CommunityBot.Features.GlobalAccounts;
using CommunityBot.Features.RepeatedTasks;
using CommunityBot.Handlers;
using CommunityBot.Helpers;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CommunityBot
{
    public class Bot: IDisposable
    {
        public Bot(string[] args)
        {
            _commandLineArgs = args;
        }

        private string[] _commandLineArgs;
        private DiscordSocketClient _client;
        private IServiceProvider _serviceProvider;

        public async Task Run()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, _commandLineArgs);
            _serviceProvider = serviceCollection.BuildServiceProvider();

            _client = _serviceProvider.GetRequiredService<DiscordSocketClient>();
            _serviceProvider.GetRequiredService<DiscordEventHandler>().InitDiscordEvents();
            await _serviceProvider.GetRequiredService<CommandHandler>().InitializeAsync();

            BotSettings botSettings = _serviceProvider.GetRequiredService<BotSettings>();
            await _client.LoginAsync(TokenType.Bot, botSettings.config.Token);

            await _client.StartAsync();

        }

        public void Dispose()
        {
            _client?.Dispose();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection, string[] args)
        {
            // no objects are created within this function; nor are any initializers executed
            // BotSettings and ApplicationSettings are created once they are first requested, not before

            serviceCollection.AddSingleton<Logger>();
            serviceCollection.AddSingleton<DiscordEventHandler>();
            serviceCollection.AddSingleton<CommandHandler>();
            serviceCollection.AddSingleton<CommandService>();
            serviceCollection.AddSingleton<ApplicationSettings>((s) =>
            {
                BotSettings botSettings = s.GetRequiredService<BotSettings>();
                return new ApplicationSettings(args, botSettings);
            });
            serviceCollection.AddSingleton<DiscordSocketClient>((s) =>
            {
                ApplicationSettings appSettings = s.GetRequiredService<ApplicationSettings>();
                return DiscordClientFactory.GetBySettings(appSettings);
            });
            serviceCollection.AddSingleton<IDataStorage, JsonDataStorage>();
            serviceCollection.AddSingleton<IGlobalUserAccounts, GlobalUserAccounts>(); //todo: delete

            serviceCollection.AddSingleton<GlobalGuildAccounts>();
            serviceCollection.AddSingleton<GlobalUserAccounts>();
            serviceCollection.AddSingleton<RepeatedTaskFunctions>();
            serviceCollection.AddSingleton<BotSettings>();
            serviceCollection.AddSingleton<JsonDataStorage>();
            serviceCollection.AddSingleton<RepeatedTaskHandler>();
        }

    }
}
