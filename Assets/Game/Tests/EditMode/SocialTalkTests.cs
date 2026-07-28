using MedievalRising.Application;
using MedievalRising.Domain.Characters;
using NUnit.Framework;

namespace MedievalRising.Tests
{
    public sealed class SocialTalkTests
    {
        [Test]
        public void TalkTo_RaisesRelationshipAndAdvancesTime()
        {
            GameSession session = GameSessionFactory.CreateNew("Aldric");
            var service = new SocialService(session);

            SocialTalkResult result = service.TalkTo(StarterNpcRoster.MiraId);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Affection, Is.EqualTo(20 + SocialService.TalkAffectionDelta));
            Assert.That(result.Trust, Is.EqualTo(20 + SocialService.TalkTrustDelta));
            Assert.That(result.Respect, Is.EqualTo(20 + SocialService.TalkRespectDelta));
            Assert.That(session.World.Now.TotalMinutes, Is.EqualTo(SocialService.TalkMinutes));
        }

        [Test]
        public void TalkTo_FailsForSelf()
        {
            GameSession session = GameSessionFactory.CreateNew("Aldric");
            var service = new SocialService(session);

            SocialTalkResult result = service.TalkTo(session.World.PlayerCharacterId);

            Assert.That(result.Success, Is.False);
            Assert.That(session.World.Now.TotalMinutes, Is.EqualTo(0));
        }
    }
}
