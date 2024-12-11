using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public interface IPhysicalLocationRepository
    {
        Task<PhysicalLocation?> GetPhysicalLocationAsync(int physicalLocationId);
        Task<List<PhysicalLocation>?> GetPhysicalLocationsAsync();
    }
}