using MedievalRising.Application;
using UnityEngine;
using DomainEntityId = MedievalRising.Domain.Primitives.EntityId;

namespace MedievalRising.Presentation
{
    public sealed class SocialTalkInteraction2D : MonoBehaviour
    {
        [SerializeField] private ulong targetCharacterId = 2;
        [SerializeField] private float interactRadius = 1.2f;

        public DomainEntityId TargetCharacterId => new DomainEntityId(targetCharacterId);

        public void Configure(ulong characterId, float radius)
        {
            targetCharacterId = characterId;
            interactRadius = radius;
        }

        public bool IsPlayerInRange(Vector2 playerPosition) =>
            ((Vector2)transform.position - playerPosition).magnitude <= interactRadius;

        public SocialTalkResult Perform(SocialService service) =>
            service.TalkTo(TargetCharacterId);

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.7f, 0.4f, 0.9f, 0.35f);
            Gizmos.DrawSphere(transform.position, interactRadius);
        }
    }
}
