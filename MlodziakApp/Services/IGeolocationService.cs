
using SharedModels;

namespace MlodziakApp.Services
{
    public interface IGeolocationService
    {
        Task StartTrackingUserLocationAsync(GeolocationAccuracy accuracy, int requestIntervalInSeconds);
        Task StopTrackingUserLocationAsync();
    }
}