using DataAccess.Entities;

namespace SharedServices
{
    public interface IUserService
    {
        Task CreateUserAsync(User userEntity);
        Task<User?> GetUserByIdAsync(string userId);
    }
}