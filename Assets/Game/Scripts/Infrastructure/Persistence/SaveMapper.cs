using System;
using System.Collections.Generic;
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
                schemaVersion = 2,
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

            foreach (KeyValuePair<(ulong, ulong), RelationshipState> pair in world.Relationships)
            {
                dto.relationships.Add(new RelationshipDto
                {
                    leftCharacterId = pair.Key.Item1,
                    rightCharacterId = pair.Key.Item2,
                    affection = pair.Value.Affection,
                    trust = pair.Value.Trust,
                    respect = pair.Value.Respect
                });
            }

            dto.characters.Sort((a, b) => a.id.CompareTo(b.id));
            dto.relationships.Sort((a, b) =>
            {
                int left = a.leftCharacterId.CompareTo(b.leftCharacterId);
                return left != 0 ? left : a.rightCharacterId.CompareTo(b.rightCharacterId);
            });
            return dto;
        }

        public static WorldState FromDto(SaveGameDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.schemaVersion != 1 && dto.schemaVersion != 2)
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

            if (dto.schemaVersion >= 2 && dto.relationships != null)
            {
                foreach (RelationshipDto relationship in dto.relationships)
                {
                    world.SetRelationship(
                        new EntityId(relationship.leftCharacterId),
                        new EntityId(relationship.rightCharacterId),
                        new RelationshipState(
                            relationship.affection,
                            relationship.trust,
                            relationship.respect));
                }
            }

            return world;
        }
    }
}
