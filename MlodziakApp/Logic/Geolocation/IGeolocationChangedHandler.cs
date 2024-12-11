using SharedModels;

namespace MlodziakApp.Logic.Geolocation
{
    public interface IGeolocationChangedHandler
    {
        Task<PhysicalLocationModel?> HandleUserGeolocationChangeAsync(Location? userGeolocation);
    }
}