using System.Collections.Generic;
using MedievalRising.Application;
using MedievalRising.Domain.Simulation;
using NUnit.Framework;

namespace MedievalRising.Tests
{
    public sealed class SimulationEngineTests
    {
        [Test]
        public void Advance_OneHour_DecaysNeedsOnce()
        {
            GameSession session = GameSessionFactory.CreateNew("Test Founder");
            var player = session.World.GetCharacter(session.World.PlayerCharacterId);

            session.AdvanceMinutes(60);

            Assert.That(player.Needs.Hunger, Is.EqualTo(98));
            Assert.That(player.Needs.Energy, Is.EqualTo(99));
        }

        [Test]
        public void Systems_RunByPhaseThenStableId()
        {
            var order = new List<string>();
            var engine = new SimulationEngine(new ISimulationSystem[]
            {
                new Recorder("b", SimulationPhase.Events, order),
                new Recorder("z", SimulationPhase.NeedsAndHealth, order),
                new Recorder("a", SimulationPhase.Events, order)
            });
            GameSession session = GameSessionFactory.CreateNew("Test");

            engine.Advance(session.World, 1);

            Assert.That(order, Is.EqualTo(new[] { "z", "a", "b" }));
        }

        private sealed class Recorder : ISimulationSystem
        {
            private readonly List<string> _order;

            public Recorder(string id, SimulationPhase phase, List<string> order)
            {
                SystemId = id;
                Phase = phase;
                _order = order;
            }

            public string SystemId { get; }

            public SimulationPhase Phase { get; }

            public SimulationCadence Cadence => SimulationCadence.EveryMinute;

            public void Tick(SimulationContext context) => _order.Add(SystemId);
        }
    }
}
