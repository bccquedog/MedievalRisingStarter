using MedievalRising.Domain.World;

namespace MedievalRising.Application.Persistence
{
    public interface ISaveRepository
    {
        void Save(string slotId, WorldState world);

        WorldState Load(string slotId);

        bool Exists(string slotId);
    }
}
