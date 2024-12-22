using SharedModels;

namespace SharedServices
{
    public interface ILocationService
    {
        Task<Dictionary<int, List<LocationModel>>> GetAllLocationModelsAsync(string userId);
        Task<List<LocationModel>> GetLocationModelsAsync(string userId, int categoryId);
        Task<LocationModel> GetLocationModelAsync(int physicalLocationId, string userId);
    }
}