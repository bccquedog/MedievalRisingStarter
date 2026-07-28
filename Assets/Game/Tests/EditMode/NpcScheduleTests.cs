using MedievalRising.Domain.Characters;
using NUnit.Framework;

namespace MedievalRising.Tests
{
    public sealed class NpcScheduleTests
    {
        [Test]
        public void ResolveActivity_UsesLatestStartedEntryForHour()
        {
            Assert.That(StarterNpcRoster.MiraSchedule.ResolveActivity(0), Is.EqualTo("home"));
            Assert.That(StarterNpcRoster.MiraSchedule.ResolveActivity(7), Is.EqualTo("market"));
            Assert.That(StarterNpcRoster.MiraSchedule.ResolveActivity(11), Is.EqualTo("market"));
            Assert.That(StarterNpcRoster.MiraSchedule.ResolveActivity(12), Is.EqualTo("well"));
            Assert.That(StarterNpcRoster.MiraSchedule.ResolveActivity(18), Is.EqualTo("home"));
            Assert.That(StarterNpcRoster.MiraSchedule.ResolveActivity(23), Is.EqualTo("home"));
        }

        [Test]
        public void ResolveActivity_BeforeFirstEntry_WrapsToLastActivity()
        {
            var schedule = new DailySchedule(new[]
            {
                new DailySchedule.Entry(7, "market"),
                new DailySchedule.Entry(18, "home")
            });

            Assert.That(schedule.ResolveActivity(3), Is.EqualTo("home"));
            Assert.That(schedule.ResolveActivity(7), Is.EqualTo("market"));
        }
    }
}
