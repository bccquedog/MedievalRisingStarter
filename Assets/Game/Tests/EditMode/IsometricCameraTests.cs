using MedievalRising.Presentation;
using NUnit.Framework;
using UnityEngine;

namespace MedievalRising.Tests
{
    public sealed class IsometricCameraTests
    {
        [Test]
        public void ClampPosition_KeepsCameraInsideVillageBounds()
        {
            var bounds = new Rect(-3f, -2f, 6f, 4f);

            Vector3 clamped = IsometricCameraFollow2D.ClampPosition(
                new Vector3(10f, -10f, 0f),
                new Vector2(0f, 1f),
                bounds);

            Assert.That(clamped.x, Is.EqualTo(3f));
            Assert.That(clamped.y, Is.EqualTo(-2f));
        }

        [Test]
        public void ClampPosition_AppliesFollowOffsetBeforeClamping()
        {
            var bounds = new Rect(-3f, -2f, 6f, 4f);

            Vector3 clamped = IsometricCameraFollow2D.ClampPosition(
                new Vector3(1f, 0f, 0f),
                new Vector2(0f, 1.25f),
                bounds);

            Assert.That(clamped.x, Is.EqualTo(1f));
            Assert.That(clamped.y, Is.EqualTo(1.25f));
        }
    }
}
