using SharedModels;

namespace MlodziakApp.Logic.Geolocation
{
    public interface IGeolocationDataLoader
    {
        Task<List<PhysicalLocationModel>> GetVisitablePhysicalLocations();
    }
}