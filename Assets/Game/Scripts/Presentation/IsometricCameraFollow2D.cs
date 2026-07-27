using UnityEngine;

namespace MedievalRising.Presentation
{
    [RequireComponent(typeof(Camera))]
    public sealed class IsometricCameraFollow2D : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Rect worldBounds = new Rect(-8f, -5f, 16f, 10f);
        [SerializeField] private Vector2 offset = new Vector2(0f, 1.25f);

        public void Configure(Transform followTarget, Rect bounds)
        {
            target = followTarget;
            worldBounds = bounds;
        }

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            Vector3 next = ClampPosition(target.position, offset, worldBounds);
            next.z = transform.position.z;
            transform.position = next;
        }

        public static Vector3 ClampPosition(Vector3 targetPosition, Vector2 followOffset, Rect bounds)
        {
            Vector2 desired = (Vector2)targetPosition + followOffset;
            Vector2 clamped = IsometricMovementResolver.ClampToBounds(desired, bounds);
            return new Vector3(clamped.x, clamped.y, targetPosition.z);
        }
    }
}
