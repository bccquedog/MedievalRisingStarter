using System;
using System.Collections.Generic;

namespace MedievalRising.Infrastructure.Persistence
{
    [Serializable]
    public sealed class SaveGameDto
    {
        public int schemaVersion = 1;
        public long totalMinutes;
        public ulong randomState;
        public ulong playerCharacterId;
        public List<CharacterDto> characters = new List<CharacterDto>();
    }

    [Serializable]
    public sealed class CharacterDto
    {
        public ulong id;
        public string displayName;
        public int hunger;
        public int energy;
        public long moneyMinorUnits;
    }
}
