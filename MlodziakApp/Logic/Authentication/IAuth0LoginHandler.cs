
using IdentityModel.OidcClient;

namespace MlodziakApp.Logic.Authentication
{
    public interface IAuth0LoginHandler
    {
        Task<LoginResult?> LoginAuth0FormAsync();
    }
}