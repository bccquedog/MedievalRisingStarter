using MedievalRising.Application;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MedievalRising.Presentation
{
    public sealed class DailyLifeInteractionController : MonoBehaviour
    {
        [SerializeField] private GameBootstrap bootstrap;
        [SerializeField] private Transform player;

        private DailyLifeInteraction2D[] _dailyInteractions;
        private SocialTalkInteraction2D[] _socialInteractions;

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

            _dailyInteractions = FindObjectsByType<DailyLifeInteraction2D>(FindObjectsSortMode.None);
            _socialInteractions = FindObjectsByType<SocialTalkInteraction2D>(FindObjectsSortMode.None);
        }

        private void Update()
        {
            if (bootstrap == null || bootstrap.Session == null || player == null)
            {
                return;
            }

            Vector2 playerPosition = player.position;
            DailyLifeInteraction2D nearestDaily = FindNearestDaily(playerPosition);
            SocialTalkInteraction2D nearestSocial = FindNearestSocial(playerPosition);

            float dailyDistance = nearestDaily == null
                ? float.MaxValue
                : ((Vector2)nearestDaily.transform.position - playerPosition).magnitude;
            float socialDistance = nearestSocial == null
                ? float.MaxValue
                : ((Vector2)nearestSocial.transform.position - playerPosition).magnitude;

            bool preferSocial = socialDistance < dailyDistance;
            NearestActionHint = preferSocial && nearestSocial != null
                ? "E: Talk"
                : nearestDaily != null
                    ? $"E: {nearestDaily.Action}"
                    : string.Empty;

            Keyboard keyboard = Keyboard.current;
            if (keyboard == null || !keyboard.eKey.wasPressedThisFrame)
            {
                return;
            }

            if (preferSocial && nearestSocial != null)
            {
                SocialTalkResult talk = nearestSocial.Perform(new SocialService(bootstrap.Session));
                LastActionStatus = talk.Message;
                return;
            }

            if (nearestDaily != null)
            {
                DailyLifeActionResult result = nearestDaily.Perform(new DailyLifeService(bootstrap.Session));
                LastActionStatus = result.Message;
            }
        }

        private DailyLifeInteraction2D FindNearestDaily(Vector2 playerPosition)
        {
            if (_dailyInteractions == null)
            {
                return null;
            }

            DailyLifeInteraction2D nearest = null;
            float nearestDistance = float.MaxValue;
            for (int index = 0; index < _dailyInteractions.Length; index++)
            {
                DailyLifeInteraction2D interaction = _dailyInteractions[index];
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

        private SocialTalkInteraction2D FindNearestSocial(Vector2 playerPosition)
        {
            if (_socialInteractions == null)
            {
                return null;
            }

            SocialTalkInteraction2D nearest = null;
            float nearestDistance = float.MaxValue;
            for (int index = 0; index < _socialInteractions.Length; index++)
            {
                SocialTalkInteraction2D interaction = _socialInteractions[index];
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
