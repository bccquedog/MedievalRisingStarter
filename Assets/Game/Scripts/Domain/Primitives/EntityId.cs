using System;

namespace MedievalRising.Domain.Primitives
{
    public readonly struct EntityId : IEquatable<EntityId>, IComparable<EntityId>
    {
        public static readonly EntityId None = new EntityId(0);

        public EntityId(ulong value)
        {
            Value = value;
        }

        public ulong Value { get; }

        public bool IsNone => Value == 0;

        public static EntityId Create(ref DeterministicRandom random)
        {
            ulong value;
            do
            {
                value = random.NextUInt64();
            }
            while (value == 0);

            return new EntityId(value);
        }

        public bool Equals(EntityId other) => Value == other.Value;

        public override bool Equals(object obj) => obj is EntityId other && Equals(other);

        public override int GetHashCode() => Value.GetHashCode();

        public int CompareTo(EntityId other) => Value.CompareTo(other.Value);

        public override string ToString() => Value.ToString("X16");

        public static bool operator ==(EntityId left, EntityId right) => left.Equals(right);

        public static bool operator !=(EntityId left, EntityId right) => !left.Equals(right);
    }
}

