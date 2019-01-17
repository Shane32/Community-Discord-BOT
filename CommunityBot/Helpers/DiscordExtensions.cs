using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CommunityBot.Extensions;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace CommunityBot.Helpers
{
    public static class DiscordExtensions
    {
        public static Task<SocketMessage> AwaitMessage(this ModuleBase<BotCommandContext> module, Func<SocketMessage, bool> filter = null, int timeoutInMs = 30000)
        {
            return AwaitMessage(module, module.Context.Channel, filter, timeoutInMs);
        }

        /// <summary>
        /// Extended function that creates an awaitable Task which resolves in the first SocketMessage send in this channel 
        /// that matches the provided filter - times out after a set time
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="filter">Optional - if not provided first message in channel is match</param>
        /// <param name="timeoutInMs">Optional - if not provided 30 seconds</param>
        /// <returns>Awaitable Task which resolves in the first SocketMessage that matches the filter</returns>
        public static async Task<SocketMessage> AwaitMessage(this ModuleBase<BotCommandContext> module, IMessageChannel channel, Func<SocketMessage, bool> filter = null, int timeoutInMs = 30000)
        {
            SocketMessage responseMessage = null;
            var cancler = new CancellationTokenSource();
            var waiter = Task.Delay(timeoutInMs, cancler.Token);

            // Adding function that handles filtering and 
            // assigning the respondMessage the correct value
            module.Context.Client.MessageReceived += OnMessageReceived;
            // Waiting for the timeout to run out or the task.Delay to be canceled due to a matched message
            try { await waiter; }
            catch (TaskCanceledException) { }
            finally
            {
                // Remove the function from the event handlers list
                module.Context.Client.MessageReceived -= OnMessageReceived;
            }
            return responseMessage;

            Task OnMessageReceived(SocketMessage message)
            {
                if (message.Channel.Id != channel.Id || message.Author.IsBot)
                    return Task.CompletedTask;
                if (filter != null && !filter(message))
                    return Task.CompletedTask;
                responseMessage = message;
                cancler.Cancel();
                return Task.CompletedTask;
            }
        }
    }
}
