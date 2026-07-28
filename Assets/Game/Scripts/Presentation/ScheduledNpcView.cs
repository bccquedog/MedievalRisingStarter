using System;
using MedievalRising.Domain.Characters;
using UnityEngine;

namespace MedievalRising.Presentation
{
    public sealed class ScheduledNpcView : MonoBehaviour
    {
        [Serializable]
        public sealed class Waypoint
        {
            public string activityId;
            public Vector2 position;
        }

        [SerializeField] private GameBootstrap bootstrap;
        [SerializeField] private float moveSpeed = 1.6f;
        [SerializeField] private Waypoint[] waypoints;

        public string CurrentActivityId { get; private set; } = string.Empty;

        public void Configure(GameBootstrap gameBootstrap, Waypoint[] scheduleWaypoints)
        {
            bootstrap = gameBootstrap;
            waypoints = scheduleWaypoints;
        }

        private void Awake()
        {
            if (bootstrap == null)
            {
                bootstrap = FindFirstObjectByType<GameBootstrap>();
            }
        }

        private void Update()
        {
            if (bootstrap == null || bootstrap.Session == null || waypoints == null || waypoints.Length == 0)
            {
                return;
            }

            string activityId = StarterNpcRoster.MiraSchedule.ResolveActivity(
                bootstrap.Session.World.Now.HourOfDay);
            CurrentActivityId = activityId;
            Vector2 target = ResolvePosition(activityId);
            Vector2 current = transform.position;
            Vector2 next = Vector2.MoveTowards(current, target, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(next.x, next.y, transform.position.z);
        }

        private Vector2 ResolvePosition(string activityId)
        {
            for (int index = 0; index < waypoints.Length; index++)
            {
                Waypoint waypoint = waypoints[index];
                if (waypoint != null && waypoint.activityId == activityId)
                {
                    return waypoint.position;
                }
            }

            return waypoints[0].position;
        }
    }
}
