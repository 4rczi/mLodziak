using SharedModels;

namespace MlodziakApp.Logic.Map
{
    public interface IMapDataLoader
    {
        Task<List<PhysicalLocationModel>> LoadPhysicalLocationModelsAsync(int locationId, int categoryId);
    }
}