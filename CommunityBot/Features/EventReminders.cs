using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunityBot.Entities;

namespace CommunityBot.Features
{
    public class EventReminders : IEventReminders
    {
        private List<EventReminder> _reminders = new List<EventReminder>();
        private object _remindersSync = new object();

        public void DeleteReminders(EventListing eventListing)
        {
            lock (_remindersSync)
            {
                _reminders.RemoveAll(x => x.EventListing == eventListing);
            }
        }

        public IEnumerable<EventReminder> GetPendingReminders()
        {
            IEnumerable<EventReminder> ret;
            var now = DateTimeOffset.Now;
            lock (_remindersSync)
            {
                ret = _reminders.Where(x => x.When <= now).ToList();
                _reminders.RemoveAll(x => x.When <= now);
            }
            return ret;
        }

        public void ResetReminders(EventListing eventListing)
        {
            throw new NotImplementedException();
        }

        public void ResetRemindersForUser(ulong userId)
        {
            throw new NotImplementedException();
        }
    }
}
