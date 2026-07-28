using MedievalRising.Domain.Characters;
using UnityEngine;

namespace MedievalRising.Presentation
{
    public sealed class DebugHud : MonoBehaviour
    {
        [SerializeField] private GameBootstrap bootstrap;
        [SerializeField] private DailyLifeInteractionController interactions;

        private GUIStyle _style;

        private void Awake()
        {
            if (bootstrap == null)
            {
                bootstrap = FindFirstObjectByType<GameBootstrap>();
            }

            if (interactions == null)
            {
                interactions = FindFirstObjectByType<DailyLifeInteractionController>();
            }
        }

        private void OnGUI()
        {
            if (bootstrap == null || bootstrap.Session == null)
            {
                return;
            }

            _style ??= new GUIStyle(GUI.skin.box)
            {
                alignment = TextAnchor.UpperLeft,
                fontSize = 18,
                padding = new RectOffset(16, 16, 12, 12)
            };

            CharacterState player = bootstrap.Session.World.GetCharacter(
                bootstrap.Session.World.PlayerCharacterId);

            string interactionLine = interactions == null
                ? "Interactions unavailable"
                : $"{interactions.NearestActionHint} | Last: {interactions.LastActionStatus}";

            string text =
                "MEDIEVAL RISING — FOUNDATION BUILD\n" +
                $"{bootstrap.Session.World.Now}\n" +
                $"{player.DisplayName} | Hunger {player.Needs.Hunger} | Energy {player.Needs.Energy} | Coin {player.Money}\n" +
                interactionLine + "\n" +
                $"Save: {bootstrap.SaveStatus}  |  F5 Save  F9 Load";

            GUI.Box(new Rect(20, 20, 700, 150), text, _style);
        }
    }
}
