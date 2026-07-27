using System;
using MedievalRising.Domain.World;

namespace MedievalRising.Application.Persistence
{
    public sealed class SaveService
    {
        private readonly ISaveRepository _repository;

        public SaveService(ISaveRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void Save(string slotId, WorldState world)
        {
            ValidateSlot(slotId);
            _repository.Save(slotId, world ?? throw new ArgumentNullException(nameof(world)));
        }

        public WorldState Load(string slotId)
        {
            ValidateSlot(slotId);
            return _repository.Load(slotId);
        }

        public bool Exists(string slotId)
        {
            ValidateSlot(slotId);
            return _repository.Exists(slotId);
        }

        private static void ValidateSlot(string slotId)
        {
            if (string.IsNullOrWhiteSpace(slotId))
            {
                throw new ArgumentException("Save slot ID is required.", nameof(slotId));
            }
        }
    }
}
