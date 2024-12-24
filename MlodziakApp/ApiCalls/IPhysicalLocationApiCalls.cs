using Refit;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.ApiCalls
{
    public interface IPhysicalLocationApiCalls
    {
        [Get("/api/physicallocation/{userId}/{categoryId}/{locationId}")]
        Task<List<PhysicalLocationModel>> GetPhysicalLocationsAsync(
            [Header("Authorization")] string accessToken,
            string userId,
            int categoryId,
            int locationId);

        [Get("/api/physicallocation/{userId}/visitable")]
        Task<List<PhysicalLocationModel>> GetVisitablePhysicalLocationsAsync(
            [Header("Authorization")] string accessToken,
            string userId);

        [Get("/api/physicallocation/{userId}/single/{physicalLocationId}")]
        Task<PhysicalLocationModel> GetSinglePhysicalLocationAsync(
            [Header("Authorization")] string accessToken,
            string userId,
            int physicalLocationId);
    }
}
