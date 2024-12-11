using DataAccess.Entities;

namespace MlodziakApp.ApiRequests
{
    public interface IUserRequests
    {
        Task<bool> CreateNewUserAsync(string userId, string accessToken);
        Task<User?> GetUserAsync(string userId, string accessToken);
    }
}