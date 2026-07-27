using MedievalRising.Presentation;
using NUnit.Framework;

namespace MedievalRising.Tests
{
    public sealed class SpriteSortingTests
    {
        [Test]
        public void CalculateOrder_LowerWorldYSortsInFront()
        {
            int lowerObjectOrder = IsometricSpriteSorter.CalculateOrder(-1f);
            int higherObjectOrder = IsometricSpriteSorter.CalculateOrder(1f);

            Assert.That(lowerObjectOrder, Is.GreaterThan(higherObjectOrder));
        }

        [Test]
        public void CalculateOrder_AppliesOffset()
        {
            int order = IsometricSpriteSorter.CalculateOrder(0.5f, 100, 25);

            Assert.That(order, Is.EqualTo(-25));
        }
    }
}
