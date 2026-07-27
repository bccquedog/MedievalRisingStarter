using System;
using System.IO;
using MedievalRising.Application;
using MedievalRising.Application.Persistence;
using MedievalRising.Infrastructure.Persistence;
using NUnit.Framework;

namespace MedievalRising.Tests
{
    public sealed class SaveServiceTests
    {
        [Test]
        public void SaveLoad_RoundTripsWorldThroughLocalRepository()
        {
            string root = Path.Combine(Path.GetTempPath(), "MedievalRisingTests", Guid.NewGuid().ToString("N"));
            try
            {
                var saves = new SaveService(new LocalJsonSaveRepository(root));
                GameSession original = GameSessionFactory.CreateNew("Aldric", 99);
                original.AdvanceMinutes(65);

                saves.Save("starter", original.World);

                Assert.That(saves.Exists("starter"), Is.True);

                var restored = saves.Load("starter");
                var restoredPlayer = restored.GetCharacter(restored.PlayerCharacterId);

                Assert.That(restored.Now.TotalMinutes, Is.EqualTo(original.World.Now.TotalMinutes));
                Assert.That(restored.Random.State, Is.EqualTo(original.World.Random.State));
                Assert.That(restoredPlayer.DisplayName, Is.EqualTo("Aldric"));
                Assert.That(restoredPlayer.Needs.Hunger, Is.EqualTo(98));
                Assert.That(restoredPlayer.Needs.Energy, Is.EqualTo(99));
            }
            finally
            {
                if (Directory.Exists(root))
                {
                    Directory.Delete(root, true);
                }
            }
        }
    }
}
