namespace MedievalRising.Domain.Simulation
{
    public sealed class NeedsSystem : ISimulationSystem
    {
        public string SystemId => "needs.hourly";

        public SimulationPhase Phase => SimulationPhase.NeedsAndHealth;

        public SimulationCadence Cadence => SimulationCadence.Hourly;

        public void Tick(SimulationContext context)
        {
            foreach (var character in context.World.Characters)
            {
                character.Needs.AdjustHunger(-2);
                character.Needs.AdjustEnergy(-1);
            }
        }
    }
}
