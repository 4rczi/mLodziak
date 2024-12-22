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

        [Get("/api/locations")]
        Task<List<LocationModel>> GetLocationModelsAsync(
            [Header("Authorization")] string accessToken,
            [Query] int categoryId,
            [Query] string userId);

        [Get("/api/location/all")]
        Task<Dictionary<int, List<LocationModel>>> GetAllLocationModelsAsync(
            [Header("Authorization")] string accessToken,
            [Query] string userId);

        [Get("/api/location/single")]
        Task<LocationModel> GetSingleLocationModelAsync(
            [Header("Authorization")] string accessToken,
            [Query] int physicalLocationId,
            [Query] string userId);


    }
}
