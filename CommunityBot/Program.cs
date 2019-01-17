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
            BlackBox.Initialize();

            Bot bot = null;
            try
            {
                bot = new Bot(args);
                await bot.Run();

                do
                {
                    Console.WriteLine("Press Q to exit");
                }
                while (Console.ReadKey(true).Key != ConsoleKey.Q);
            }
            catch (Exception ex)
            {
                Global.WriteColoredLine(ex.ToString(), ConsoleColor.Red);
            }
            finally
            {
                bot?.Dispose();
            }

        }

    }
}
