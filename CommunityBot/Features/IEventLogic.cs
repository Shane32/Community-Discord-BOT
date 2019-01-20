using CommunityBot.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CommunityBot.Features
{
    public interface IEventLogic
    {
        Task<bool> TryAddEvent(ulong guildId, ulong channelId, EventListing eventListing);
        Task<IEnumerable<ulong>> GetChannels(ulong guildId);
        Task<bool> TryAddChannel(ulong guildId, ulong channelId);
        Task<bool> TryRemoveChannel(ulong guildId, ulong channelId);
        Task<(EventListing EventListing, bool Deleted)> ReactToEvent(ulong guildId, ulong channelId, ulong messageId, ulong userId, ReactionTypes reactionType);
        Task<IEnumerable<EventListing>> GetEventListings(ulong guildId, ulong channelId);
    }
}
