using System;
using System.Collections.Generic;
using System.Text;

namespace CommunityBot.Entities
{
    public class EventReminder
    {
        public ulong UserId;
        public DateTimeOffset When;
        public EventListing EventListing;
    }
}
