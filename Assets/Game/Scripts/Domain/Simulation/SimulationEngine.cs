using System;
using System.Collections.Generic;
using MedievalRising.Domain.Time;
using MedievalRising.Domain.World;

namespace MedievalRising.Domain.Simulation
{
    public sealed class SimulationEngine
    {
        private readonly List<ISimulationSystem> _systems;

        public SimulationEngine(IEnumerable<ISimulationSystem> systems)
        {
            if (systems == null)
            {
                throw new ArgumentNullException(nameof(systems));
            }

            _systems = new List<ISimulationSystem>(systems);
            _systems.Sort(CompareSystems);
            RejectDuplicateIds(_systems);
        }

        public IReadOnlyList<ISimulationSystem> Systems => _systems;

        public void Advance(WorldState world, int minutes)
        {
            if (world == null)
            {
                throw new ArgumentNullException(nameof(world));
            }

            if (minutes <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minutes));
            }

            for (int i = 0; i < minutes; i++)
            {
                GameInstant previous = world.Now;
                GameInstant current = previous.AddMinutes(1);
                world.SetNow(current);

                var context = new SimulationContext(world, previous, current);
                foreach (ISimulationSystem system in _systems)
                {
                    int interval = (int)system.Cadence;
                    if (current.IsDivisibleByMinutes(interval))
                    {
                        system.Tick(context);
                    }
                }
            }
        }

        private static int CompareSystems(ISimulationSystem left, ISimulationSystem right)
        {
            int phase = left.Phase.CompareTo(right.Phase);
            return phase != 0
                ? phase
                : string.CompareOrdinal(left.SystemId, right.SystemId);
        }

        private static void RejectDuplicateIds(IReadOnlyList<ISimulationSystem> systems)
        {
            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (ISimulationSystem system in systems)
            {
                if (system == null)
                {
                    throw new ArgumentException("Simulation systems cannot contain null.");
                }

                if (string.IsNullOrWhiteSpace(system.SystemId) || !ids.Add(system.SystemId))
                {
                    throw new ArgumentException($"Duplicate or empty system ID: {system.SystemId}");
                }
            }
        }
    }
}
