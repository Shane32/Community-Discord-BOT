﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CommunityBot.Entities
{
    public class GlobalUserAccount : IGlobalAccount
    {
        public GlobalUserAccount(ulong id)
        {
            Id = id;
        }

        public ulong Id { get; }

        public List<ReminderEntry> Reminders { get; internal set; } = new List<ReminderEntry>();

        public string TimeZone { get; set; } // Please note, TimeZone ID works for LINUX, but for windows we need TimeZone NAME
        /* Add more values to store */

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

    public struct ReminderEntry
    {
        public DateTime DueDate;
        public string Description;

        public ReminderEntry(DateTime dueDate, string description)
        {
            DueDate = dueDate;
            Description = description;
        }
    }
}
