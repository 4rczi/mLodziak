using IdentityModel.OidcClient.Results;

namespace MlodziakApp.Logic.Token
{
    public interface ITokenRefresher
    {
        Task<RefreshTokenResult?> RefreshAuth0TokenAsync(string refreshToken, string userId);
    }
}