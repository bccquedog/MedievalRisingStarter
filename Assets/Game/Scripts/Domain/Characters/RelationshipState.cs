using System;

namespace MedievalRising.Domain.Characters
{
    public sealed class RelationshipState
    {
        public const int Minimum = 0;
        public const int Maximum = 100;

        public RelationshipState(int affection, int trust, int respect)
        {
            Affection = Clamp(affection);
            Trust = Clamp(trust);
            Respect = Clamp(respect);
        }

        public int Affection { get; private set; }

        public int Trust { get; private set; }

        public int Respect { get; private set; }

        public void Adjust(int affectionDelta, int trustDelta, int respectDelta)
        {
            Affection = Clamp(Affection + affectionDelta);
            Trust = Clamp(Trust + trustDelta);
            Respect = Clamp(Respect + respectDelta);
        }

        private static int Clamp(int value) => Math.Max(Minimum, Math.Min(Maximum, value));
    }
}
