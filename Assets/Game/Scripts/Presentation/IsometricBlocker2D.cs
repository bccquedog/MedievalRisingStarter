using UnityEngine;

namespace MedievalRising.Presentation
{
    public sealed class IsometricBlocker2D : MonoBehaviour
    {
        [SerializeField] private Vector2 size = Vector2.one;

        public Rect WorldRect => new Rect(
            (Vector2)transform.position - (size * 0.5f),
            size);

        public void Configure(Vector2 value)
        {
            size = value;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.9f, 0.2f, 0.1f, 0.35f);
            Gizmos.DrawCube(transform.position, size);
        }
    }
}
