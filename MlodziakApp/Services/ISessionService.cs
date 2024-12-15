
namespace MlodziakApp.Services
{
    public interface ISessionService
    {
        Task<(bool isSessionValid, string? accessToken, string? refreshToken, string? sessionId, string? userId)> ValidateSessionAsync(bool autoLoginCheck = false);
        Task HandleInvalidSessionAsync(bool isLoggedIn, bool notifyUser);
        Task<bool> InitializeSessionAsync(string accessToken, string refreshToken, string userId);
        Task<(bool isSuccess, string? accessToken, string? refreshToken, string? sessionId, string? userId)> GetSessionDataAsync();
        Task<bool> SetSessionDataAsync(string sessionId, string userId, string accessToken, string refreshToken);
        Task<bool> RemoveSessionDataAsync();
    }
}