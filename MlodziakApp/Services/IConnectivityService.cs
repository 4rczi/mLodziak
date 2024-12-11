
namespace MlodziakApp.Services
{
    public interface IConnectivityService
    {
        Task<bool> HasInternetConnectionAsync();
        Task HandleNoInternetConnectionAsync(); 
    }
}