using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityBot.Features.RepeatedTasks;
using Discord;
using Discord.WebSocket;
using System.Reflection;

namespace CommunityBot
{
    public static class Global
    {
        //internal static DiscordSocketClient Client { get; set; }
        internal static readonly String Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        // Global Helper methods

        public static async Task<string> SendWebRequest(string requestUrl)
        {
            using (var client = new HttpClient(new HttpClientHandler()))
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Event-Discord-BOT");
                using (var response = await client.GetAsync(requestUrl))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        return response.StatusCode.ToString();
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        internal static void WriteColoredLine(string text, ConsoleColor color, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
