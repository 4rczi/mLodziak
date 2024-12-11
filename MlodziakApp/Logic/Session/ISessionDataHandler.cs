
namespace MlodziakApp.Logic.Session
{
    public interface ISessionDataHandler
    {
        Task<(bool isSuccess, string? accessToken, string? refreshToken, string? sessionId, string? userId)> GetSessionDataAsync();
        Task<bool> RemoveSessionDataAsync();
        Task<bool> SetSessionDataAsync(string sessionId, string userId, string accessToken, string refreshToken);
    }
}