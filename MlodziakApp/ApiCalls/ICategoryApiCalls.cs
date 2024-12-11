using Refit;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.ApiCalls
{
    public interface ICategoryApiCalls
    {
        [Get("/api/category/{userId}")]
        Task<List<CategoryModel>> GetCategoriesAsync(
                                      string userId,
            [Header("Authorization")] string accessToken);
    }
}
