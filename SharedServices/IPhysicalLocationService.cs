using SharedModels;

namespace SharedServices
{
    public interface IPhysicalLocationService
    {
        Task<List<PhysicalLocationModel>> GetPhysicalLocationsAsync(string userId, int categoryId, int locationId);
        Task<List<PhysicalLocationModel>> GetAllPhysicalLocationsAsync(string userId);
        bool CalculateOmittness(DateTime? start, DateTime? end);
        Task<List<PhysicalLocationModel>> GetVisitablePhysicalLocationsAsync(string userId);
    }
}