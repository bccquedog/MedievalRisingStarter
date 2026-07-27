using System;

namespace MedievalRising.Domain.Characters
{
    public sealed class NeedsState
    {
        public const int Minimum = 0;
        public const int Maximum = 100;

        public NeedsState(int hunger, int energy)
        {
            Hunger = Clamp(hunger);
            Energy = Clamp(energy);
        }

        public int Hunger { get; private set; }

        public int Energy { get; private set; }

        public void AdjustHunger(int delta)
        {
            Hunger = Clamp(Hunger + delta);
        }

        public void AdjustEnergy(int delta)
        {
            Energy = Clamp(Energy + delta);
        }

        private static int Clamp(int value) => Math.Max(Minimum, Math.Min(Maximum, value));
    }
}

