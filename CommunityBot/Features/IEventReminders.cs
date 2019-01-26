using CommunityBot.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunityBot.Features
{
    public interface IEventReminders
    {
        IEnumerable<EventReminder> GetPendingReminders();

        void ResetReminders(EventListing eventListing);

        void DeleteReminders(EventListing eventListing);
        void ResetRemindersForUser(ulong userId);
    }
}
