using System;
using System.Collections.Generic;
using System.Text;

namespace CommunityBot.Entities
{
    public class EventChannel
    {
        public ulong ChannelId { get; set; }

        public List<EventListing> EventListings { get; set; }
    }
}
