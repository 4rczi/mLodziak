using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User userEntity);
        Task<User?> GetUserByIdAsync(string userId);
    }
}