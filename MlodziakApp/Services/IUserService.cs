using DataAccess.Entities;

namespace MlodziakApp.Services
{
    public interface IUserService
    {
        Task<User?> GetUserAsync(string userId, string accessToken);
        Task<bool> SyncUserAsync(string userId, string accessToken);
    }
}