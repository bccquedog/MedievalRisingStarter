using MedievalRising.Domain.Characters;
using UnityEngine;

namespace MedievalRising.Presentation
{
    public sealed class DebugHud : MonoBehaviour
    {
        [SerializeField] private GameBootstrap bootstrap;

        private GUIStyle _style;

        private void Awake()
        {
            if (bootstrap == null)
            {
                bootstrap = FindFirstObjectByType<GameBootstrap>();
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

            string text =
                "MEDIEVAL RISING — FOUNDATION BUILD\n" +
                $"{bootstrap.Session.World.Now}\n" +
                $"{player.DisplayName} | Hunger {player.Needs.Hunger} | Energy {player.Needs.Energy}\n" +
                $"Save: {bootstrap.SaveStatus}  |  F5 Save  F9 Load\n" +
                "Workflow: ticket → architecture gate → implementation → tests → visual QA";

            GUI.Box(new Rect(20, 20, 650, 130), text, _style);
        }
    }
}
