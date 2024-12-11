
namespace MlodziakApp.Logic.Session
{
    public interface ISessionValidator
    {
        Task<(bool isSessionValid, string? accessToken, string? refreshToken, string? sessionId, string? userId)> ValidateSessionAsync();
    }
}