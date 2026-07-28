using System;
using System.Collections.Generic;
using MedievalRising.Domain.Characters;
using MedievalRising.Domain.Primitives;
using MedievalRising.Domain.Time;

namespace MedievalRising.Domain.World
{
    public sealed class WorldState
    {
        private readonly Dictionary<EntityId, CharacterState> _characters =
            new Dictionary<EntityId, CharacterState>();

        private readonly Dictionary<(ulong, ulong), RelationshipState> _relationships =
            new Dictionary<(ulong, ulong), RelationshipState>();

        public WorldState(GameInstant now, DeterministicRandom random)
        {
            Now = now;
            Random = random;
        }

        public GameInstant Now { get; private set; }

        public DeterministicRandom Random { get; private set; }

        public EntityId PlayerCharacterId { get; private set; }

        public IReadOnlyCollection<CharacterState> Characters => _characters.Values;

        public IReadOnlyCollection<KeyValuePair<(ulong, ulong), RelationshipState>> Relationships =>
            _relationships;

        public void SetNow(GameInstant value)
        {
            if (value.CompareTo(Now) < 0)
            {
                throw new InvalidOperationException("Authoritative time cannot move backwards.");
            }

            Now = value;
        }

        public void AddCharacter(CharacterState character, bool isPlayer = false)
        {
            if (character == null)
            {
                throw new ArgumentNullException(nameof(character));
            }

            if (_characters.ContainsKey(character.Id))
            {
                throw new InvalidOperationException($"Duplicate entity ID: {character.Id}");
            }

            _characters.Add(character.Id, character);
            if (isPlayer)
            {
                PlayerCharacterId = character.Id;
            }
        }

        public CharacterState GetCharacter(EntityId id)
        {
            if (!_characters.TryGetValue(id, out CharacterState character))
            {
                throw new KeyNotFoundException($"Character not found: {id}");
            }

            return character;
        }

        public RelationshipState GetOrCreateRelationship(EntityId left, EntityId right)
        {
            (ulong, ulong) key = NormalizePair(left, right);
            if (!_relationships.TryGetValue(key, out RelationshipState relationship))
            {
                relationship = new RelationshipState(20, 20, 20);
                _relationships[key] = relationship;
            }

            return relationship;
        }

        public void SetRelationship(EntityId left, EntityId right, RelationshipState relationship)
        {
            if (relationship == null)
            {
                throw new ArgumentNullException(nameof(relationship));
            }

            _relationships[NormalizePair(left, right)] = relationship;
        }

        public void RestoreRandomState(ulong state)
        {
            Random = new DeterministicRandom(state);
        }

        private static (ulong, ulong) NormalizePair(EntityId left, EntityId right)
        {
            if (left.IsNone || right.IsNone)
            {
                throw new ArgumentException("Relationship endpoints cannot be None.");
            }

            if (left.Value == right.Value)
            {
                throw new ArgumentException("A character cannot have a relationship with itself.");
            }

            return left.Value < right.Value
                ? (left.Value, right.Value)
                : (right.Value, left.Value);
        }
    }
}
