
namespace MlodziakApp.Services
{
    public interface ITokenService
    {
        Task<bool> ValidateTokensAsync(string accessToken, string refreshToken, string userId);
        Task<string?> GetUserIdFromAccessTokenAsync(string accessToken);
    }
}