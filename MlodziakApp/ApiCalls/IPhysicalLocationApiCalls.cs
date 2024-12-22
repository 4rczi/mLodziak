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
        [Get("/api/physicallocation")]
        Task<List<PhysicalLocationModel>> GetPhysicalLocationsAsync(
            [Query] string userId,
            [Query] int categoryId,
            [Query] int locationId,
            [Header("Authorization")] string accessToken);

        [Get("/api/physicallocation/visitable")]
        Task<List<PhysicalLocationModel>> GetVisitablePhysicalLocationsAsync(
            [Query] string userId,
            [Header("Authorization")] string accessToken);

        [Get("/api/physicallocation/single")]
        Task<PhysicalLocationModel> GetSinglePhysicalLocationAsync(
            [Query] int physicalLocationId,
            [Query] string userId,
            [Header("Authorization")] string accessToken);
    }
}
