using MedievalRising.Application;
using UnityEngine;

namespace MedievalRising.Presentation
{
    public sealed class DailyLifeInteraction2D : MonoBehaviour
    {
        public enum ActionKind
        {
            Eat,
            Work,
            BuyMeal,
            Sleep
        }

        [SerializeField] private ActionKind action = ActionKind.Eat;
        [SerializeField] private float interactRadius = 1.1f;

        public ActionKind Action => action;

        public void Configure(ActionKind kind, float radius)
        {
            action = kind;
            interactRadius = radius;
        }

        public bool IsPlayerInRange(Vector2 playerPosition) =>
            ((Vector2)transform.position - playerPosition).magnitude <= interactRadius;

        public DailyLifeActionResult Perform(DailyLifeService service)
        {
            switch (action)
            {
                case ActionKind.Eat:
                    return service.EatMeal();
                case ActionKind.Work:
                    return service.WorkFarmingShift();
                case ActionKind.BuyMeal:
                    return service.BuyMeal();
                case ActionKind.Sleep:
                    return service.Sleep();
                default:
                    return new DailyLifeActionResult(false, DailyLifeActionKind.Eat, "Unknown interaction.");
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.3f, 0.7f, 1f, 0.35f);
            Gizmos.DrawSphere(transform.position, interactRadius);
        }
    }
}
