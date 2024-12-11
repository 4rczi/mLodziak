using SharedModels;

namespace MlodziakApp.Logic.Geolocation
{
    public interface IGeolocationVisitHandler
    {
        int? CanVisitLocation(Location userGeolocation, List<PhysicalLocationModel> visitablePhysicalLocationModels);
        Task<bool> VisitPhysicalLocationAsync(int physicalLocationId);
    }
}