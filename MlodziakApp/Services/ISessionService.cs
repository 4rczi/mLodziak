
namespace MlodziakApp.Services
{
    public interface ISessionService
    {
        Task<(bool isSessionValid, string? accessToken, string? refreshToken, string? sessionId, string? userId)> ValidateSessionAsync(bool autoLoginCheck = false);
        Task HandleInvalidSessionAsync(bool isLoggedIn);
        Task<bool> InitializeSessionAsync(string accessToken, string refreshToken, string userId);
    }
}