
namespace MlodziakApp.Logic.Token
{
    public interface ITokenClaimsValidator
    {
        Task<string?> GetUserIdFromAccessTokenAsync(string accessToken);
    }
}