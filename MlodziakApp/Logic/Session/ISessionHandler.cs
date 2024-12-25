
namespace MlodziakApp.Logic.Session
{
    public interface ISessionHandler
    {
        Task<(bool isSuccess, string? sessionId)> InitializeSessionAsync(string accessToken, string refreshToken, string userId);
        Task HandleInvalidSessionAsync(bool isLoggedIn);
    }
}