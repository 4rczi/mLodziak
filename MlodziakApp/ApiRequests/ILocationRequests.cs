using SharedModels;

namespace MlodziakApp.ApiRequests
{
    public interface ILocationRequests
    {
        Task<Dictionary<int, List<LocationModel>>> GetAllLocationModelsAsync(string accessToken, string userId, string sessionId);
        Task<List<LocationModel>> GetLocationModelsAsync(string accessToken, int categoryId, string userId, string sessionId);
        Task<LocationModel> GetSingleLocationModelAsync(string accessToken, int physicalLocationId, string userId, string sessionId);
    }
}