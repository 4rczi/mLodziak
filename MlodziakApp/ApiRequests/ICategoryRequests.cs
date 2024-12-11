using SharedModels;

namespace MlodziakApp.ApiRequests
{
    public interface ICategoryRequests
    {
        Task<List<CategoryModel>> GetCategoryModelsAsync(string accessToken, string userId, string sessionId);
    }
}