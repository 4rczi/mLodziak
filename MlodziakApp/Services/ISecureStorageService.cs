
namespace MlodziakApp.Services
{
    public interface ISecureStorageService
    {
        Task<string?> GetAccessTokenAsync();
        Task RemoveAccessTokenAsync();
        Task SetAccessTokenAsync(string accessToken);

        Task<string?> GetRefreshTokenAsync();
        Task RemoveRefreshTokenAsync();
        Task SetRefreshTokenAsync(string refreshToken);

        Task<string?> GetSessionIdAsync();
        Task RemoveSessionIdAsync();
        Task SetSessionIdAsync(string sessionId);

        Task<string?> GetUserIdAsync();
        Task RemoveUserIdAsync();
        Task SetUserIdAsync(string userId);
    }
}