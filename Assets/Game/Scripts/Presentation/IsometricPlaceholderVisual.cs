using UnityEngine;

namespace MedievalRising.Presentation
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class IsometricPlaceholderVisual : MonoBehaviour
    {
        [SerializeField] private Color color = Color.white;
        [SerializeField] private bool diamond = true;

        private void Awake()
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (renderer.sprite == null)
            {
                renderer.sprite = diamond ? CreateDiamondSprite() : CreateSquareSprite();
            }

            renderer.color = color;
        }

        public void Configure(Color value, bool useDiamond)
        {
            color = value;
            diamond = useDiamond;
        }

        private static Sprite CreateDiamondSprite()
        {
            const int width = 32;
            const int height = 16;
            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.name = "Runtime Isometric Diamond";

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float normalizedX = Mathf.Abs((x + 0.5f - width * 0.5f) / (width * 0.5f));
                    float normalizedY = Mathf.Abs((y + 0.5f - height * 0.5f) / (height * 0.5f));
                    texture.SetPixel(x, y, normalizedX + normalizedY <= 1f ? Color.white : Color.clear);
                }
            }

            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 16f);
        }

        private static Sprite CreateSquareSprite()
        {
            var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture.name = "Runtime Placeholder Square";
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
        }
    }
}
