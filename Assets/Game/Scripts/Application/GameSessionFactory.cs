using MedievalRising.Domain.Characters;
using MedievalRising.Domain.Primitives;
using MedievalRising.Domain.Simulation;
using MedievalRising.Domain.Time;
using MedievalRising.Domain.World;

namespace MedievalRising.Application
{
    public static class GameSessionFactory
    {
        public static GameSession CreateNew(string founderName, ulong seed = 1214)
        {
            var world = new WorldState(GameInstant.Epoch, new DeterministicRandom(seed));
            var founder = new CharacterState(
                new EntityId(1),
                founderName,
                new NeedsState(100, 100),
                new Money(250));

            world.AddCharacter(founder, true);

            var mira = new CharacterState(
                StarterNpcRoster.MiraId,
                "Mira",
                new NeedsState(100, 100),
                new Money(400));
            world.AddCharacter(mira);
            world.GetOrCreateRelationship(founder.Id, mira.Id);

            return CreateFromWorld(world);
        }

        public static GameSession CreateFromWorld(WorldState world)
        {
            var simulation = new SimulationEngine(new ISimulationSystem[] { new NeedsSystem() });
            return new GameSession(world, simulation);
        }
    }
}
