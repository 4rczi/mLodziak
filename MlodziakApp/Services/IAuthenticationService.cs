
namespace MlodziakApp.Services
{
    public interface IAuthenticationService
    {
        Task<bool> LoginAsync();
        Task<bool> LogoutAsync();
    }
}