using SharedModels;

namespace MlodziakApp.ApiRequests
{
    public interface IPhysicalLocationRequests
    {
        Task<List<PhysicalLocationModel>> GetPhysicalLocationModelsAsync(string accessToken, string userId, int categoryId, int locationId, string sessionId);
        Task<List<PhysicalLocationModel>> GetVisitablePhysicalLocationModelsAsync(string accessToken, string userId, string sessionId);
        Task<PhysicalLocationModel?> GetSinglePhysicalLocationAsync(string accessToken, int physicalLocationId, string userId, string sessionId);
    }
}