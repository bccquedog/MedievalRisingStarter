using System;
using System.Collections.Generic;
using MedievalRising.Domain.Primitives;

namespace MedievalRising.Domain.Characters
{
    public sealed class DailySchedule
    {
        private readonly List<Entry> _entries;

        public DailySchedule(IEnumerable<Entry> entries)
        {
            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            _entries = new List<Entry>(entries);
            if (_entries.Count == 0)
            {
                throw new ArgumentException("Schedule requires at least one entry.", nameof(entries));
            }

            _entries.Sort((left, right) => left.StartHour.CompareTo(right.StartHour));
            for (int index = 0; index < _entries.Count; index++)
            {
                Entry entry = _entries[index];
                if (entry.StartHour < 0 || entry.StartHour > 23)
                {
                    throw new ArgumentOutOfRangeException(nameof(entries), "Schedule hours must be 0-23.");
                }

                if (string.IsNullOrWhiteSpace(entry.ActivityId))
                {
                    throw new ArgumentException("Schedule activity IDs are required.", nameof(entries));
                }
            }
        }

        public string ResolveActivity(int hourOfDay)
        {
            if (hourOfDay < 0 || hourOfDay > 23)
            {
                throw new ArgumentOutOfRangeException(nameof(hourOfDay));
            }

            string activity = _entries[_entries.Count - 1].ActivityId;
            for (int index = 0; index < _entries.Count; index++)
            {
                if (_entries[index].StartHour <= hourOfDay)
                {
                    activity = _entries[index].ActivityId;
                }
            }

            return activity;
        }

        public readonly struct Entry
        {
            public Entry(int startHour, string activityId)
            {
                StartHour = startHour;
                ActivityId = activityId;
            }

            public int StartHour { get; }

            public string ActivityId { get; }
        }
    }

    public static class StarterNpcRoster
    {
        public static readonly EntityId MiraId = new EntityId(2);

        public static DailySchedule MiraSchedule { get; } = new DailySchedule(new[]
        {
            new DailySchedule.Entry(0, "home"),
            new DailySchedule.Entry(7, "market"),
            new DailySchedule.Entry(12, "well"),
            new DailySchedule.Entry(18, "home")
        });
    }
}
