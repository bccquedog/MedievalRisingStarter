using System;

namespace MedievalRising.Domain.Primitives
{
    public struct DeterministicRandom
    {
        private const ulong DefaultSeed = 0x9E3779B97F4A7C15UL;
        private ulong _state;

        public DeterministicRandom(ulong seed)
        {
            _state = seed == 0 ? DefaultSeed : seed;
        }

        public ulong State => _state;

        public ulong NextUInt64()
        {
            // xorshift64*: compact, deterministic, and sufficient for simulation streams.
            ulong value = _state;
            value ^= value >> 12;
            value ^= value << 25;
            value ^= value >> 27;
            _state = value;
            return value * 2685821657736338717UL;
        }

        public int NextInt(int minInclusive, int maxExclusive)
        {
            if (maxExclusive <= minInclusive)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxExclusive),
                    "Maximum must be greater than minimum.");
            }

            uint range = (uint)(maxExclusive - minInclusive);
            return minInclusive + (int)(NextUInt64() % range);
        }
    }
}

