using System;
using System.Collections.Generic;
using CommunityBot.Features.GlobalAccounts;
using Discord;

namespace CommunityBot.Entities
{
    public class GlobalGuildAccount : IGlobalAccount
    {
        public GlobalGuildAccount(ulong id)
        {
            Id = id;
        }
        public ulong Id { get; }

        public IReadOnlyList<string> Prefixes { get; set; } = new List<string>();

        public int ServerActivityLog { get; set; }

        public ulong LogChannelId { get; set; }

        public List<EventChannel> EventChannels { get; set; };

        /* Add more values to store */
        
        public GlobalGuildAccount Modify(Action<GuildAccountSettings> func, GlobalGuildAccounts globalGuildAccounts)
        {
            var settings = new GuildAccountSettings();
            func(settings);

            if (settings.Prefixes.IsSpecified)
                Prefixes = settings.Prefixes.Value;
            globalGuildAccounts.SaveAccounts(Id);
            return this;
        }
        
        // override object.Equals
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return Equals(obj as IGlobalAccount);
        }

        // implementation for IEquatable
        public bool Equals(IGlobalAccount other)
        {
            return Id == other.Id;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return unchecked((int)Id);
        }
    }
    public class GuildAccountSettings
    {
        public Optional<List<string>> Prefixes { get; private set; }
        public GuildAccountSettings SetPrefixes(List<string> prefixes) { Prefixes = prefixes; return this; }

        
    }
}
