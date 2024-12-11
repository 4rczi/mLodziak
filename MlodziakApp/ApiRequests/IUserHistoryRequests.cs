
namespace MlodziakApp.ApiRequests
{
    public interface IUserHistoryRequests
    {
        Task<bool> CreateUserHistoryAsync(string userId, int physicalLocationId, string accessToken, string sessionId);
    }
}