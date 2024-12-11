using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public interface ILocationRepository
    {
        Task<Location?> GetLocationAsync(int locationId);
        Task<List<Location>> GetLocationsAsync();
    }
}