using CommunityBot.Entities;
using CommunityBot.Features.GlobalAccounts;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityBot.Features
{
    public class EventLogic: IEventLogic
    {
        private readonly GlobalGuildAccounts _globalGuildAccounts;
        private readonly DiscordSocketClient _client;

        public EventLogic(GlobalGuildAccounts globalGuildAccounts, DiscordSocketClient client)
        {
            _globalGuildAccounts = globalGuildAccounts;
            _client = client;
        }

        public async Task<bool> TryAddEvent(ulong guildId, ulong channelId, EventListing eventListing) {
            var guild = _globalGuildAccounts.GetGuildAccount(guildId);
            var channel = guild.EventChannels?.FirstOrDefault(x => x.ChannelId == channelId);
            if (channel == null) return false;
            if (channel.EventListings == null) channel.EventListings = new List<EventListing>();
            channel.EventListings.Add(eventListing);
            _globalGuildAccounts.SaveAccounts(guildId);
            return true;
        }
        
        public async Task<IEnumerable<ulong>> GetChannels(ulong guildId)
        {
            var guild = _globalGuildAccounts.GetGuildAccount(guildId);
            if (guild.EventChannels == null) return new ulong[] { };
            return guild.EventChannels.Select(x => x.ChannelId);
        }

        public async Task<bool> TryRemoveChannel(ulong guildId, ulong channelId)
        {
            var guild = _globalGuildAccounts.GetGuildAccount(guildId);
            if (guild.EventChannels == null) return false;
            guild.EventChannels.RemoveAll(x => x.ChannelId == channelId);
            _globalGuildAccounts.SaveAccounts(guildId);
            return true;
        }

        public async Task<bool> TryAddChannel(ulong guildId, ulong channelId)
        {
            var guild = _globalGuildAccounts.GetGuildAccount(guildId);
            if (guild.EventChannels == null) guild.EventChannels = new List<EventChannel>();
            guild.EventChannels.Add(new EventChannel() { ChannelId = channelId });
            return true;
        }

        public async Task<(EventListing EventListing, bool Deleted)> ReactToEvent(ulong guildId, ulong channelId, ulong messageId, ulong userId, ReactionTypes reactionType)
        {
            var guild = _globalGuildAccounts.GetGuildAccount(guildId);
            if (guild.EventChannels == null) return (null, false);
            var channel = guild.EventChannels.FirstOrDefault(x => x.ChannelId == channelId);
            if (channel == null) return (null, false);
            var message = channel.EventListings.FirstOrDefault(x => x.EventMessageId == messageId);
            if (message == null) return (null, false);
            switch (reactionType)
            {
                case ReactionTypes.Accept:
                    if (message.JoinedUserIds == null) message.JoinedUserIds = new List<ulong>();
                    message.JoinedUserIds.Add(userId);
                    message.DeclinedUserIds?.Remove(userId);
                    message.ReserveUserIds?.Remove(userId);
                    break;
                case ReactionTypes.Decline:
                    if (message.DeclinedUserIds == null) message.DeclinedUserIds = new List<ulong>();
                    message.DeclinedUserIds.Add(userId);
                    message.JoinedUserIds?.Remove(userId);
                    message.ReserveUserIds?.Remove(userId);
                    break;
                case ReactionTypes.Reserve:
                    if (message.ReserveUserIds == null) message.ReserveUserIds = new List<ulong>();
                    message.ReserveUserIds.Add(userId);
                    message.JoinedUserIds?.Remove(userId);
                    message.DeclinedUserIds?.Remove(userId);
                    break;
                case ReactionTypes.Delete:
                    channel.EventListings.Remove(message);
                    break;
            }
            _globalGuildAccounts.SaveAccounts(guildId);
            return (message, reactionType == ReactionTypes.Delete);
        }

        public async Task<IEnumerable<EventListing>> GetEventListings(ulong guildId, ulong channelId)
        {
            var guild = _globalGuildAccounts.GetGuildAccount(guildId);
            var channel = guild.EventChannels?.FirstOrDefault(x => x.ChannelId == channelId);
            return channel?.EventListings ?? Enumerable.Empty<EventListing>();
        }
    }
}
