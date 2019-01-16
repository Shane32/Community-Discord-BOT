using Discord.WebSocket;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using CommunityBot.Configuration;
using CommunityBot.Handlers;
using Microsoft.Extensions.DependencyInjection;
using CommunityBot.Features;
using Discord.Commands;
using CommunityBot.Features.GlobalAccounts;
using CommunityBot.Helpers;
using CommunityBot.Features.RepeatedTasks;

namespace CommunityBot
{
    class Program
    {
        private static DiscordSocketClient _client;
        private static IServiceProvider _serviceProvider;

        private static async Task Main(string[] args)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, args);
            _serviceProvider = serviceCollection.BuildServiceProvider();

            BlackBox.Initialize();

            using (_client = _serviceProvider.GetRequiredService<DiscordSocketClient>())
            {
                _serviceProvider.GetRequiredService<DiscordEventHandler>().InitDiscordEvents();
                await _serviceProvider.GetRequiredService<CommandHandler>().InitializeAsync();

                await AttemptLogin();

                await _client.StartAsync();

                do
                {
                    Console.WriteLine("Press Q to exit");
                }
                while (Console.ReadKey(true).Key != ConsoleKey.Q);
            }

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

        private static async Task AttemptLogin()
        {
            BotSettings botSettings = _serviceProvider.GetRequiredService<BotSettings>();
            try
            {
                await _client.LoginAsync(TokenType.Bot, botSettings.config.Token);
            }
            catch (HttpRequestException e)
            {
                if (e.InnerException == null)
                {
                    Global.WriteColoredLine($"An HTTP Request exception occurred.\nMessage:\n{e.Message}", ConsoleColor.Red);
                }
                else
                {
                    Global.WriteColoredLine($"An HTTP request ran into a problem:\n{e.InnerException.Message}",
                        ConsoleColor.Red);
                }

                Environment.Exit(0);
            }
            catch (Exception e)
            {
                Global.WriteColoredLine($"An exception occurred.\nMessage:\n{e.Message}\n\nYour token might not be configured, or it might be wrong.", ConsoleColor.Red);

                Environment.Exit(0);
            }
        }

    }
}
