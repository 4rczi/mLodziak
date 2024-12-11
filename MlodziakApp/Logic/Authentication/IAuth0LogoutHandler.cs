
namespace MlodziakApp.Logic.Authentication
{
    public interface IAuth0LogoutHandler
    {
        Task<bool> LogoutAuth0Async();
    }
}