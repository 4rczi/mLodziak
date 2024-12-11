
using System.Net;

namespace MlodziakApp.Logic.Token
{
    public interface ITokenValidator
    {
        Task<(bool isSuccess, HttpStatusCode statusCode)> ValidateAccessTokenAsync(string accessToken, string userId);
        bool ValidateRefreshToken(string refreshToken);
    }
}