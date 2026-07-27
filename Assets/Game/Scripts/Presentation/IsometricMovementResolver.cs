using System.Collections.Generic;
using UnityEngine;

namespace MedievalRising.Presentation
{
    public static class IsometricMovementResolver
    {
        public static Vector2 Resolve(
            Vector2 current,
            Vector2 input,
            float distance,
            Rect worldBounds,
            IReadOnlyList<Rect> blockers)
        {
            if (distance <= 0f || input.sqrMagnitude <= 0f)
            {
                return ClampToBounds(current, worldBounds);
            }

            Vector2 delta = input.normalized * distance;
            Vector2 desired = ClampToBounds(current + delta, worldBounds);
            if (!IsBlocked(desired, blockers))
            {
                return desired;
            }

            Vector2 horizontal = ClampToBounds(current + new Vector2(delta.x, 0f), worldBounds);
            if (!IsBlocked(horizontal, blockers))
            {
                return horizontal;
            }

            Vector2 vertical = ClampToBounds(current + new Vector2(0f, delta.y), worldBounds);
            if (!IsBlocked(vertical, blockers))
            {
                return vertical;
            }

            return ClampToBounds(current, worldBounds);
        }

        public static bool IsBlocked(Vector2 point, IReadOnlyList<Rect> blockers)
        {
            if (blockers == null)
            {
                return false;
            }

            for (int index = 0; index < blockers.Count; index++)
            {
                if (blockers[index].Contains(point))
                {
                    return true;
                }
            }

            return false;
        }

        public static Vector2 ClampToBounds(Vector2 point, Rect worldBounds)
        {
            return new Vector2(
                Mathf.Clamp(point.x, worldBounds.xMin, worldBounds.xMax),
                Mathf.Clamp(point.y, worldBounds.yMin, worldBounds.yMax));
        }
    }
}
