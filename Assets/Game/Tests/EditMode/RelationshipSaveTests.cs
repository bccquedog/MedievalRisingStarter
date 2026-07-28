using MedievalRising.Application;
using MedievalRising.Domain.Characters;
using MedievalRising.Domain.Primitives;
using MedievalRising.Domain.Time;
using MedievalRising.Domain.World;
using MedievalRising.Infrastructure.Persistence;
using NUnit.Framework;

namespace MedievalRising.Tests
{
    public sealed class RelationshipSaveTests
    {
        [Test]
        public void SchemaV2_RoundTripsRelationships()
        {
            GameSession session = GameSessionFactory.CreateNew("Aldric", 99);
            new SocialService(session).TalkTo(StarterNpcRoster.MiraId);

            WorldState restored = SaveMapper.FromDto(SaveMapper.ToDto(session.World));
            RelationshipState relationship = restored.GetOrCreateRelationship(
                restored.PlayerCharacterId,
                StarterNpcRoster.MiraId);

            Assert.That(SaveMapper.ToDto(session.World).schemaVersion, Is.EqualTo(2));
            Assert.That(relationship.Affection, Is.EqualTo(20 + SocialService.TalkAffectionDelta));
            Assert.That(relationship.Trust, Is.EqualTo(20 + SocialService.TalkTrustDelta));
            Assert.That(relationship.Respect, Is.EqualTo(20 + SocialService.TalkRespectDelta));
            Assert.That(restored.GetCharacter(StarterNpcRoster.MiraId).DisplayName, Is.EqualTo("Mira"));
        }

        [Test]
        public void SchemaV1_StillLoadsWithoutRelationships()
        {
            var dto = new SaveGameDto
            {
                schemaVersion = 1,
                totalMinutes = 30,
                randomState = 7,
                playerCharacterId = 1,
                characters =
                {
                    new CharacterDto
                    {
                        id = 1,
                        displayName = "Aldric",
                        hunger = 80,
                        energy = 90,
                        moneyMinorUnits = 250
                    }
                }
            };

            WorldState world = SaveMapper.FromDto(dto);

            Assert.That(world.Now.TotalMinutes, Is.EqualTo(30));
            Assert.That(world.GetCharacter(new EntityId(1)).Needs.Hunger, Is.EqualTo(80));
            RelationshipState relationship = world.GetOrCreateRelationship(
                new EntityId(1),
                new EntityId(2));
            Assert.That(relationship.Affection, Is.EqualTo(20));
        }
    }
}
