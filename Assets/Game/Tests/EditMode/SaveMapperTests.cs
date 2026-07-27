using MedievalRising.Application;
using MedievalRising.Infrastructure.Persistence;
using NUnit.Framework;

namespace MedievalRising.Tests
{
    public sealed class SaveMapperTests
    {
        [Test]
        public void RoundTrip_PreservesAuthoritativeFoundationState()
        {
            GameSession original = GameSessionFactory.CreateNew("Ysabeau", 42);
            original.AdvanceMinutes(180);

            var restored = SaveMapper.FromDto(SaveMapper.ToDto(original.World));
            var restoredPlayer = restored.GetCharacter(restored.PlayerCharacterId);

            Assert.That(restored.Now.TotalMinutes, Is.EqualTo(original.World.Now.TotalMinutes));
            Assert.That(restored.Random.State, Is.EqualTo(original.World.Random.State));
            Assert.That(restoredPlayer.DisplayName, Is.EqualTo("Ysabeau"));
            Assert.That(restoredPlayer.Needs.Hunger, Is.EqualTo(94));
            Assert.That(restoredPlayer.Money.MinorUnits, Is.EqualTo(250));
        }
    }
}
