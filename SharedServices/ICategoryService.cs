using SharedModels;

namespace SharedServices
{
    public interface ICategoryService
    {
        Task<List<CategoryModel>> GetCategoryModelsAsync(string userId);
    }
}