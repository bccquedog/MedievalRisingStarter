namespace MedievalRising.Domain.Simulation
{
    public interface ISimulationSystem
    {
        string SystemId { get; }

        SimulationPhase Phase { get; }

        SimulationCadence Cadence { get; }

        void Tick(SimulationContext context);
    }
}
