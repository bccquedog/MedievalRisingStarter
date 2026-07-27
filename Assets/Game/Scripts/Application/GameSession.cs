using System;
using MedievalRising.Domain.Simulation;
using MedievalRising.Domain.World;

namespace MedievalRising.Application
{
    public sealed class GameSession
    {
        private readonly SimulationEngine _simulation;

        public GameSession(WorldState world, SimulationEngine simulation)
        {
            World = world ?? throw new ArgumentNullException(nameof(world));
            _simulation = simulation ?? throw new ArgumentNullException(nameof(simulation));
        }

        public WorldState World { get; }

        public void AdvanceMinutes(int minutes)
        {
            _simulation.Advance(World, minutes);
        }
    }
}
