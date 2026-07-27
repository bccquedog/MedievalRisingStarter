using System;
using MedievalRising.Domain.Characters;
using MedievalRising.Domain.Primitives;
using MedievalRising.Domain.Time;
using MedievalRising.Domain.World;

namespace MedievalRising.Infrastructure.Persistence
{
    public static class SaveMapper
    {
        public static SaveGameDto ToDto(WorldState world)
        {
            if (world == null)
            {
                throw new ArgumentNullException(nameof(world));
            }

            var dto = new SaveGameDto
            {
                totalMinutes = world.Now.TotalMinutes,
                randomState = world.Random.State,
                playerCharacterId = world.PlayerCharacterId.Value
            };

            foreach (CharacterState character in world.Characters)
            {
                dto.characters.Add(new CharacterDto
                {
                    id = character.Id.Value,
                    displayName = character.DisplayName,
                    hunger = character.Needs.Hunger,
                    energy = character.Needs.Energy,
                    moneyMinorUnits = character.Money.MinorUnits
                });
            }

            dto.characters.Sort((a, b) => a.id.CompareTo(b.id));
            return dto;
        }

        public static WorldState FromDto(SaveGameDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.schemaVersion != 1)
            {
                throw new InvalidOperationException($"Unsupported save schema {dto.schemaVersion}.");
            }

            var world = new WorldState(
                new GameInstant(dto.totalMinutes),
                new DeterministicRandom(dto.randomState));

            foreach (CharacterDto character in dto.characters)
            {
                var state = new CharacterState(
                    new EntityId(character.id),
                    character.displayName,
                    new NeedsState(character.hunger, character.energy),
                    new Money(character.moneyMinorUnits));

                world.AddCharacter(state, character.id == dto.playerCharacterId);
            }

            return world;
        }
    }
}
