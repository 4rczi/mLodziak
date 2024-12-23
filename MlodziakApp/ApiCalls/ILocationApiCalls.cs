using Refit;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.ApiCalls
{
    public interface ILocationApiCalls
    {
        [Get("/api/location/{userId}/{categoryId}")]
        Task<List<LocationModel>> GetLocationModelsAsync(
            [Header("Authorization")] string accessToken,
            string userId,
            int categoryId);

        [Get("/api/location/{userId}/all")]
        Task<Dictionary<int, List<LocationModel>>> GetAllLocationModelsAsync(
            [Header("Authorization")] string accessToken,
            string userId);

        [Get("/api/location/{userId}/single/{physicalLocationId}")]
        Task<LocationModel> GetSingleLocationModelAsync(
            [Header("Authorization")] string accessToken,
            string userId,
            int physicalLocationId);


    }
}
