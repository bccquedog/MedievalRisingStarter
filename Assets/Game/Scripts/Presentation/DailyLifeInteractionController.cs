using MedievalRising.Application;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MedievalRising.Presentation
{
    public sealed class DailyLifeInteractionController : MonoBehaviour
    {
        [SerializeField] private GameBootstrap bootstrap;
        [SerializeField] private Transform player;

        private DailyLifeInteraction2D[] _interactions;

        public string LastActionStatus { get; private set; } = "No actions yet";

        public string NearestActionHint { get; private set; } = string.Empty;

        public void Configure(GameBootstrap gameBootstrap, Transform playerTransform)
        {
            bootstrap = gameBootstrap;
            player = playerTransform;
        }

        private void Awake()
        {
            if (bootstrap == null)
            {
                bootstrap = FindFirstObjectByType<GameBootstrap>();
            }

            _interactions = FindObjectsByType<DailyLifeInteraction2D>(FindObjectsSortMode.None);
        }

        private void Update()
        {
            if (bootstrap == null || bootstrap.Session == null || player == null)
            {
                return;
            }

            DailyLifeInteraction2D nearest = FindNearestInRange();
            NearestActionHint = nearest != null
                ? $"E: {nearest.Action}"
                : string.Empty;

            Keyboard keyboard = Keyboard.current;
            if (keyboard == null || nearest == null || !keyboard.eKey.wasPressedThisFrame)
            {
                return;
            }

            DailyLifeActionResult result = nearest.Perform(new DailyLifeService(bootstrap.Session));
            LastActionStatus = result.Message;
        }

        private DailyLifeInteraction2D FindNearestInRange()
        {
            if (_interactions == null)
            {
                return null;
            }

            DailyLifeInteraction2D nearest = null;
            float nearestDistance = float.MaxValue;
            Vector2 playerPosition = player.position;

            for (int index = 0; index < _interactions.Length; index++)
            {
                DailyLifeInteraction2D interaction = _interactions[index];
                if (interaction == null || !interaction.isActiveAndEnabled || !interaction.IsPlayerInRange(playerPosition))
                {
                    continue;
                }

                float distance = ((Vector2)interaction.transform.position - playerPosition).magnitude;
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = interaction;
                }
            }

            return nearest;
        }
    }
}
