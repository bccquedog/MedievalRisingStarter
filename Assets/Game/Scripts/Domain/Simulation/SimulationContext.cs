using System;
using MedievalRising.Domain.Time;
using MedievalRising.Domain.World;

namespace MedievalRising.Domain.Simulation
{
    public sealed class SimulationContext
    {
        public SimulationContext(WorldState world, GameInstant previous, GameInstant current)
        {
            World = world ?? throw new ArgumentNullException(nameof(world));
            Previous = previous;
            Current = current;
        }

        public WorldState World { get; }

        public GameInstant Previous { get; }

        public GameInstant Current { get; }
    }
}
