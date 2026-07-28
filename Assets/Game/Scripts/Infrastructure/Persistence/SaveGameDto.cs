using System;
using System.Collections.Generic;

namespace MedievalRising.Infrastructure.Persistence
{
    [Serializable]
    public sealed class SaveGameDto
    {
        public int schemaVersion = 2;
        public long totalMinutes;
        public ulong randomState;
        public ulong playerCharacterId;
        public List<CharacterDto> characters = new List<CharacterDto>();
        public List<RelationshipDto> relationships = new List<RelationshipDto>();
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

    [Serializable]
    public sealed class RelationshipDto
    {
        public ulong leftCharacterId;
        public ulong rightCharacterId;
        public int affection;
        public int trust;
        public int respect;
    }
}
