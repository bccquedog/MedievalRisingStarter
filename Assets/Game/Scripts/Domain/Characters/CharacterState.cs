using System;
using MedievalRising.Domain.Primitives;

namespace MedievalRising.Domain.Characters
{
    public sealed class CharacterState
    {
        public CharacterState(EntityId id, string displayName, NeedsState needs, Money money)
        {
            if (id.IsNone)
            {
                throw new ArgumentException("Character ID cannot be None.", nameof(id));
            }

            Id = id;
            DisplayName = string.IsNullOrWhiteSpace(displayName) ? "Unnamed" : displayName.Trim();
            Needs = needs ?? throw new ArgumentNullException(nameof(needs));
            Money = money;
        }

        public EntityId Id { get; }

        public string DisplayName { get; }

        public NeedsState Needs { get; }

        public Money Money { get; private set; }

        public void SetMoney(Money value)
        {
            Money = value;
        }
    }
}

