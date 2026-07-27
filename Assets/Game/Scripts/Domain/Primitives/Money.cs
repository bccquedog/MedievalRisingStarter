using System;

namespace MedievalRising.Domain.Primitives
{
    public readonly struct Money : IEquatable<Money>, IComparable<Money>
    {
        public static readonly Money Zero = new Money(0);

        public Money(long minorUnits)
        {
            MinorUnits = minorUnits;
        }

        public long MinorUnits { get; }

        public Money Add(Money amount) => new Money(checked(MinorUnits + amount.MinorUnits));

        public bool TrySubtract(Money amount, out Money result)
        {
            if (amount.MinorUnits < 0 || amount.MinorUnits > MinorUnits)
            {
                result = this;
                return false;
            }

            result = new Money(MinorUnits - amount.MinorUnits);
            return true;
        }

        public bool Equals(Money other) => MinorUnits == other.MinorUnits;

        public override bool Equals(object obj) => obj is Money other && Equals(other);

        public override int GetHashCode() => MinorUnits.GetHashCode();

        public int CompareTo(Money other) => MinorUnits.CompareTo(other.MinorUnits);

        public override string ToString() => $"{MinorUnits / 100m:0.00}";
    }
}

