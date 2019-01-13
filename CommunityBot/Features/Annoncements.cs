using System.Threading.Tasks;
using CommunityBot.Features.GlobalAccounts;
using Discord.WebSocket;

namespace CommunityBot.Features
{
    public class Announcements
    {
        private readonly GlobalGuildAccounts _globalGuildAccounts;

        public Announcements(GlobalGuildAccounts globalGuildAccounts)
        {
            _globalGuildAccounts = globalGuildAccounts;
        }

        public async Task UserJoined(SocketGuildUser user)
        {
            var dmChannel = await user.GetOrCreateDMChannelAsync();
            var possibleMessages = _globalGuildAccounts.GetGuildAccount(user.Guild.Id).WelcomeMessages;
            var messageString = possibleMessages[Global.Rng.Next(possibleMessages.Count)];
            messageString = messageString.ReplacePlacehoderStrings(user);
            if (string.IsNullOrEmpty(messageString)) return;
            await dmChannel.SendMessageAsync(messageString);
        }

        public async Task UserLeft(SocketGuildUser user, DiscordSocketClient client)
        {
            var guildAcc = _globalGuildAccounts.GetGuildAccount(user.Guild.Id);
            if (guildAcc.AnnouncementChannelId == 0) return;
            if (!(client.GetChannel(guildAcc.AnnouncementChannelId) is SocketTextChannel channel)) return;
            var possibleMessages = guildAcc.LeaveMessages;
            var messageString = possibleMessages[Global.Rng.Next(possibleMessages.Count)];
            messageString = messageString.ReplacePlacehoderStrings(user);
            if (string.IsNullOrEmpty(messageString)) return;
            await channel.SendMessageAsync(messageString);
        }
    }
}
