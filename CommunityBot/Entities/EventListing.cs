using System;
using System.Collections.Generic;
using System.Text;

namespace CommunityBot.Entities
{
    class EventListing
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime When { get; set; }
        public int NumPlayers { get; set; }
        public ulong CreatorUserId { get; set; }
        public ulong EventMessageId { get; set; }
        public List<ulong> JoinedUserIds { get; set; }
        public List<ulong> DeclinedUserIds { get; set; }
        public List<ulong> ReserveUserIds { get; set; }
    }
}
