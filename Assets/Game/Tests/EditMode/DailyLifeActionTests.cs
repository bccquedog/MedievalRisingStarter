using MedievalRising.Application;
using MedievalRising.Domain.Characters;
using MedievalRising.Domain.Primitives;
using NUnit.Framework;

namespace MedievalRising.Tests
{
    public sealed class DailyLifeActionTests
    {
        [Test]
        public void EatMeal_RestoresHungerAndAdvancesTime()
        {
            GameSession session = GameSessionFactory.CreateNew("Aldric");
            CharacterState player = session.World.GetCharacter(session.World.PlayerCharacterId);
            player.Needs.AdjustHunger(-40);

            var service = new DailyLifeService(session);
            DailyLifeActionResult result = service.EatMeal();

            Assert.That(result.Success, Is.True);
            Assert.That(session.World.Now.TotalMinutes, Is.EqualTo(DailyLifeService.EatMinutes));
            Assert.That(player.Needs.Hunger, Is.GreaterThan(60));
        }

        [Test]
        public void EatMeal_FailsWhenAlreadyFull()
        {
            GameSession session = GameSessionFactory.CreateNew("Aldric");
            var service = new DailyLifeService(session);

            DailyLifeActionResult result = service.EatMeal();

            Assert.That(result.Success, Is.False);
            Assert.That(session.World.Now.TotalMinutes, Is.EqualTo(0));
        }

        [Test]
        public void WorkFarmingShift_PaysWageAndCostsNeeds()
        {
            GameSession session = GameSessionFactory.CreateNew("Aldric");
            CharacterState player = session.World.GetCharacter(session.World.PlayerCharacterId);
            long startingMoney = player.Money.MinorUnits;

            var service = new DailyLifeService(session);
            DailyLifeActionResult result = service.WorkFarmingShift();

            Assert.That(result.Success, Is.True);
            Assert.That(player.Money.MinorUnits, Is.EqualTo(startingMoney + DailyLifeService.WorkWageMinorUnits));
            Assert.That(session.World.Now.TotalMinutes, Is.EqualTo(DailyLifeService.WorkMinutes));
        }

        [Test]
        public void WorkFarmingShift_FailsWhenExhausted()
        {
            GameSession session = GameSessionFactory.CreateNew("Aldric");
            CharacterState player = session.World.GetCharacter(session.World.PlayerCharacterId);
            player.Needs.AdjustEnergy(-(NeedsState.Maximum - 5));
            long startingMoney = player.Money.MinorUnits;

            var service = new DailyLifeService(session);
            DailyLifeActionResult result = service.WorkFarmingShift();

            Assert.That(result.Success, Is.False);
            Assert.That(player.Money.MinorUnits, Is.EqualTo(startingMoney));
            Assert.That(session.World.Now.TotalMinutes, Is.EqualTo(0));
        }

        [Test]
        public void BuyMeal_SubtractsPriceAndEats()
        {
            GameSession session = GameSessionFactory.CreateNew("Aldric");
            CharacterState player = session.World.GetCharacter(session.World.PlayerCharacterId);
            player.Needs.AdjustHunger(-50);
            long startingMoney = player.Money.MinorUnits;

            var service = new DailyLifeService(session);
            DailyLifeActionResult result = service.BuyMeal();

            Assert.That(result.Success, Is.True);
            Assert.That(player.Money.MinorUnits, Is.EqualTo(startingMoney - DailyLifeService.MealPriceMinorUnits));
        }

        [Test]
        public void BuyMeal_FailsWithoutEnoughCoin()
        {
            GameSession session = GameSessionFactory.CreateNew("Aldric");
            CharacterState player = session.World.GetCharacter(session.World.PlayerCharacterId);
            player.SetMoney(new Money(10));
            player.Needs.AdjustHunger(-50);

            var service = new DailyLifeService(session);
            DailyLifeActionResult result = service.BuyMeal();

            Assert.That(result.Success, Is.False);
            Assert.That(player.Money.MinorUnits, Is.EqualTo(10));
        }

        [Test]
        public void Sleep_RestoresEnergyAndAdvancesEightHours()
        {
            GameSession session = GameSessionFactory.CreateNew("Aldric");
            CharacterState player = session.World.GetCharacter(session.World.PlayerCharacterId);
            player.Needs.AdjustEnergy(-40);

            var service = new DailyLifeService(session);
            DailyLifeActionResult result = service.Sleep();

            Assert.That(result.Success, Is.True);
            Assert.That(session.World.Now.TotalMinutes, Is.EqualTo(DailyLifeService.SleepMinutes));
            Assert.That(player.Needs.Energy, Is.GreaterThan(60));
        }

        [Test]
        public void Sleep_FailsWhenNotTired()
        {
            GameSession session = GameSessionFactory.CreateNew("Aldric");
            var service = new DailyLifeService(session);

            DailyLifeActionResult result = service.Sleep();

            Assert.That(result.Success, Is.False);
            Assert.That(session.World.Now.TotalMinutes, Is.EqualTo(0));
        }
    }
}
