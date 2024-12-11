using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public interface IUserHistoryRepository
    {
        Task<List<UserHistory>> GetUserHistoryAsync(string userId);
        Task CreateUserHistoryAsync(UserHistory userHistory);
    }
}