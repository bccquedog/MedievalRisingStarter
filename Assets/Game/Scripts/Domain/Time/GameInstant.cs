using System;

namespace MedievalRising.Domain.Time
{
    public readonly struct GameInstant : IEquatable<GameInstant>, IComparable<GameInstant>
    {
        public static readonly GameInstant Epoch = new GameInstant(0);

        public GameInstant(long totalMinutes)
        {
            if (totalMinutes < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(totalMinutes));
            }

            TotalMinutes = totalMinutes;
        }

        public long TotalMinutes { get; }

        public int MinuteOfHour => (int)(TotalMinutes % 60);

        public int HourOfDay => (int)((TotalMinutes / 60) % 24);

        public int DayIndex => (int)(TotalMinutes / 1440);

        public GameInstant AddMinutes(long minutes)
        {
            if (minutes < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minutes));
            }

            return new GameInstant(checked(TotalMinutes + minutes));
        }

        public bool IsDivisibleByMinutes(int intervalMinutes)
        {
            if (intervalMinutes <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(intervalMinutes));
            }

            return TotalMinutes % intervalMinutes == 0;
        }

        public bool Equals(GameInstant other) => TotalMinutes == other.TotalMinutes;

        public override bool Equals(object obj) => obj is GameInstant other && Equals(other);

        public override int GetHashCode() => TotalMinutes.GetHashCode();

        public int CompareTo(GameInstant other) => TotalMinutes.CompareTo(other.TotalMinutes);

        public override string ToString() =>
            $"Day {DayIndex + 1}, {HourOfDay:00}:{MinuteOfHour:00}";
    }
}

