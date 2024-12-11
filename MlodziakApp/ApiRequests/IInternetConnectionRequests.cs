
namespace MlodziakApp.ApiRequests
{
    public interface IInternetConnectionRequests
    {
        Task<bool> IsInternetAccessibleAsync();
    }
}