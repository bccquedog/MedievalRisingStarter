using System;
using MedievalRising.Domain.Characters;
using MedievalRising.Domain.Primitives;

namespace MedievalRising.Application
{
    public sealed class SocialService
    {
        public const int TalkMinutes = 10;
        public const int TalkAffectionDelta = 4;
        public const int TalkTrustDelta = 3;
        public const int TalkRespectDelta = 2;

        private readonly GameSession _session;

        public SocialService(GameSession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public SocialTalkResult TalkTo(EntityId targetId)
        {
            EntityId playerId = _session.World.PlayerCharacterId;
            if (targetId.Equals(playerId))
            {
                return new SocialTalkResult(false, "Cannot talk to yourself.");
            }

            CharacterState target = _session.World.GetCharacter(targetId);
            RelationshipState relationship = _session.World.GetOrCreateRelationship(playerId, targetId);
            relationship.Adjust(TalkAffectionDelta, TalkTrustDelta, TalkRespectDelta);
            _session.AdvanceMinutes(TalkMinutes);

            return new SocialTalkResult(
                true,
                $"Talked with {target.DisplayName}.",
                relationship.Affection,
                relationship.Trust,
                relationship.Respect);
        }
    }

    public readonly struct SocialTalkResult
    {
        public SocialTalkResult(bool success, string message, int affection = 0, int trust = 0, int respect = 0)
        {
            Success = success;
            Message = message;
            Affection = affection;
            Trust = trust;
            Respect = respect;
        }

        public bool Success { get; }

        public string Message { get; }

        public int Affection { get; }

        public int Trust { get; }

        public int Respect { get; }
    }
}
