using UnityEngine;

namespace MedievalRising.Presentation
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class IsometricSpriteSorter : MonoBehaviour
    {
        [SerializeField] private int orderOffset;
        [SerializeField] private int unitsPerOrder = 100;

        private SpriteRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            Apply();
        }

        private void LateUpdate()
        {
            Apply();
        }

        private void Apply()
        {
            _renderer ??= GetComponent<SpriteRenderer>();
            _renderer.sortingOrder = CalculateOrder(transform.position.y, unitsPerOrder, orderOffset);
        }

        public static int CalculateOrder(float worldY, int orderScale = 100, int offset = 0)
        {
            return Mathf.RoundToInt(-worldY * orderScale) + offset;
        }
    }
}
