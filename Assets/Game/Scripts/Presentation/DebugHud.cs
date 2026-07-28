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

            string relationshipLine = "No companion nearby";
            CharacterState mira = null;
            foreach (CharacterState character in bootstrap.Session.World.Characters)
            {
                if (character.Id.Equals(StarterNpcRoster.MiraId))
                {
                    mira = character;
                    break;
                }
            }

            if (mira != null)
            {
                RelationshipState relationship =
                    bootstrap.Session.World.GetOrCreateRelationship(player.Id, mira.Id);
                relationshipLine =
                    $"{mira.DisplayName}: Aff {relationship.Affection} Trust {relationship.Trust} Respect {relationship.Respect}";
            }

            string interactionLine = interactions == null
                ? "Interactions unavailable"
                : $"{interactions.NearestActionHint} | Last: {interactions.LastActionStatus}";

            string text =
                "MEDIEVAL RISING — FOUNDATION BUILD\n" +
                $"{bootstrap.Session.World.Now}\n" +
                $"{player.DisplayName} | Hunger {player.Needs.Hunger} | Energy {player.Needs.Energy} | Coin {player.Money}\n" +
                relationshipLine + "\n" +
                interactionLine + "\n" +
                $"Save: {bootstrap.SaveStatus}  |  F5 Save  F9 Load";

            GUI.Box(new Rect(20, 20, 720, 170), text, _style);
        }
    }
}
