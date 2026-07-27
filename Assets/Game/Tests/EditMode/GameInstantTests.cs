using MedievalRising.Domain.Time;
using NUnit.Framework;

namespace MedievalRising.Tests
{
    public sealed class GameInstantTests
    {
        [Test]
        public void AddMinutes_CrossesDayBoundaryDeterministically()
        {
            GameInstant result = new GameInstant(1439).AddMinutes(2);

            Assert.That(result.DayIndex, Is.EqualTo(1));
            Assert.That(result.HourOfDay, Is.EqualTo(0));
            Assert.That(result.MinuteOfHour, Is.EqualTo(1));
        }
    }
}
