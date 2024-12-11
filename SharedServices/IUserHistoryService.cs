using DataAccess.Entities;

namespace SharedServices
{
    public interface IUserHistoryService
    {
        Task CreateUserHistoryAsync(UserHistory userHistory);
    }
}