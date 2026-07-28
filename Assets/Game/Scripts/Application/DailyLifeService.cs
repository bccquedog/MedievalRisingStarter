using System;
using MedievalRising.Domain.Characters;
using MedievalRising.Domain.Primitives;

namespace MedievalRising.Application
{
    public sealed class DailyLifeService
    {
        public const int EatMinutes = 20;
        public const int WorkMinutes = 240;
        public const int SleepMinutes = 480;

        public const int EatHungerRestored = 25;
        public const int WorkEnergyCost = 20;
        public const int WorkHungerCost = 15;
        public const long WorkWageMinorUnits = 180;
        public const long MealPriceMinorUnits = 60;
        public const int SleepEnergyRestored = 60;
        public const int SleepHungerCost = 25;

        private readonly GameSession _session;

        public DailyLifeService(GameSession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public DailyLifeActionResult EatMeal()
        {
            CharacterState player = Player();
            if (player.Needs.Hunger >= NeedsState.Maximum)
            {
                return new DailyLifeActionResult(false, DailyLifeActionKind.Eat, "Already full.");
            }

            player.Needs.AdjustHunger(EatHungerRestored);
            _session.AdvanceMinutes(EatMinutes);
            return new DailyLifeActionResult(true, DailyLifeActionKind.Eat, "Ate a meal.");
        }

        public DailyLifeActionResult WorkFarmingShift()
        {
            CharacterState player = Player();
            if (player.Needs.Energy < WorkEnergyCost)
            {
                return new DailyLifeActionResult(false, DailyLifeActionKind.Work, "Too exhausted to work a shift.");
            }

            player.Needs.AdjustEnergy(-WorkEnergyCost);
            player.Needs.AdjustHunger(-WorkHungerCost);
            player.SetMoney(player.Money.Add(new Money(WorkWageMinorUnits)));
            _session.AdvanceMinutes(WorkMinutes);
            return new DailyLifeActionResult(true, DailyLifeActionKind.Work, "Worked a farming shift.");
        }

        public DailyLifeActionResult BuyMeal()
        {
            CharacterState player = Player();
            var price = new Money(MealPriceMinorUnits);
            if (!player.Money.TrySubtract(price, out Money remaining))
            {
                return new DailyLifeActionResult(false, DailyLifeActionKind.BuyMeal, "Not enough coin for a meal.");
            }

            player.SetMoney(remaining);
            DailyLifeActionResult eat = EatMeal();
            if (!eat.Success)
            {
                return new DailyLifeActionResult(true, DailyLifeActionKind.BuyMeal, "Bought food, saved for later.");
            }

            return new DailyLifeActionResult(true, DailyLifeActionKind.BuyMeal, "Bought and ate a meal.");
        }

        public DailyLifeActionResult Sleep()
        {
            CharacterState player = Player();
            if (player.Needs.Energy >= NeedsState.Maximum)
            {
                return new DailyLifeActionResult(false, DailyLifeActionKind.Sleep, "Not tired.");
            }

            player.Needs.AdjustEnergy(SleepEnergyRestored);
            player.Needs.AdjustHunger(-SleepHungerCost);
            _session.AdvanceMinutes(SleepMinutes);
            return new DailyLifeActionResult(true, DailyLifeActionKind.Sleep, "Slept through the night.");
        }

        private CharacterState Player() =>
            _session.World.GetCharacter(_session.World.PlayerCharacterId);
    }
}
