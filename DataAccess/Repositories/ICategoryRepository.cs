using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public interface ICategoryRepository
    {
        Task<bool> DeleteCategoryAsync(int categoryId);
        Task<List<Category>> GetCategoriesAsync();
        Task<Category?> GetCategoryAsync(int categoryId);
    }
}