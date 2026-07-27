using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace MedievalRising.Presentation
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class PlayerMover2D : MonoBehaviour
    {
        [SerializeField] private float speed = 3f;
        [SerializeField] private Rect worldBounds = new Rect(-8f, -5f, 16f, 10f);

        private readonly List<Rect> _blockerRects = new List<Rect>();
        private IsometricBlocker2D[] _blockers;

        private void Awake()
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (renderer.sprite == null)
            {
                renderer.sprite = CreatePlaceholderSprite();
                renderer.color = new Color(0.73f, 0.19f, 0.12f);
            }

            _blockers = FindObjectsByType<IsometricBlocker2D>(FindObjectsSortMode.None);
        }

        public void ConfigureBounds(Rect bounds)
        {
            worldBounds = bounds;
        }

        private void Update()
        {
            Vector2 input = Vector2.zero;
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null)
            {
                input.x = ReadAxis(keyboard.aKey, keyboard.leftArrowKey, keyboard.dKey, keyboard.rightArrowKey);
                input.y = ReadAxis(keyboard.sKey, keyboard.downArrowKey, keyboard.wKey, keyboard.upArrowKey);
            }

            if (Gamepad.current != null && Gamepad.current.leftStick.ReadValue().sqrMagnitude > input.sqrMagnitude)
            {
                input = Gamepad.current.leftStick.ReadValue();
            }

            Vector2 next = IsometricMovementResolver.Resolve(
                transform.position,
                input,
                speed * Time.deltaTime,
                worldBounds,
                CurrentBlockerRects());

            transform.position = new Vector3(next.x, next.y, transform.position.z);
        }

        private static float ReadAxis(KeyControl negativeA, KeyControl negativeB, KeyControl positiveA, KeyControl positiveB)
        {
            float negative = negativeA.isPressed || negativeB.isPressed ? 1f : 0f;
            float positive = positiveA.isPressed || positiveB.isPressed ? 1f : 0f;
            return positive - negative;
        }

        private IReadOnlyList<Rect> CurrentBlockerRects()
        {
            _blockerRects.Clear();
            if (_blockers == null)
            {
                return _blockerRects;
            }

            for (int index = 0; index < _blockers.Length; index++)
            {
                IsometricBlocker2D blocker = _blockers[index];
                if (blocker != null && blocker.isActiveAndEnabled)
                {
                    _blockerRects.Add(blocker.WorldRect);
                }
            }

            return _blockerRects;
        }

        private static Sprite CreatePlaceholderSprite()
        {
            var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture.name = "Runtime Player Placeholder";
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
        }
    }
}
