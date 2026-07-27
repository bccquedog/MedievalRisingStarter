using MedievalRising.Presentation;
using NUnit.Framework;
using UnityEngine;

namespace MedievalRising.Tests
{
    public sealed class IsometricMovementTests
    {
        [Test]
        public void Resolve_ClampsToVillageBounds()
        {
            var bounds = new Rect(-1f, -1f, 2f, 2f);

            Vector2 resolved = IsometricMovementResolver.Resolve(
                Vector2.zero,
                Vector2.right,
                4f,
                bounds,
                null);

            Assert.That(resolved, Is.EqualTo(new Vector2(1f, 0f)));
        }

        [Test]
        public void Resolve_SlidesAlongOpenAxisWhenDiagonalTargetIsBlocked()
        {
            var bounds = new Rect(-5f, -5f, 10f, 10f);
            var blockers = new[]
            {
                new Rect(0.6f, 0.6f, 1f, 1f)
            };

            Vector2 resolved = IsometricMovementResolver.Resolve(
                Vector2.zero,
                Vector2.one,
                1.5f,
                bounds,
                blockers);

            Assert.That(resolved.x, Is.GreaterThan(0f));
            Assert.That(resolved.y, Is.EqualTo(0f).Within(0.001f));
        }

        [Test]
        public void Resolve_StaysPutWhenAllCandidateMovesAreBlocked()
        {
            var bounds = new Rect(-5f, -5f, 10f, 10f);
            var blockers = new[]
            {
                new Rect(0.1f, -0.2f, 2f, 0.4f),
                new Rect(-0.2f, 0.1f, 0.4f, 2f),
                new Rect(0.6f, 0.6f, 1f, 1f)
            };

            Vector2 resolved = IsometricMovementResolver.Resolve(
                Vector2.zero,
                Vector2.one,
                1.5f,
                bounds,
                blockers);

            Assert.That(resolved, Is.EqualTo(Vector2.zero));
        }
    }
}
