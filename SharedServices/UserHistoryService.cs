using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedServices
{
    public class UserHistoryService : IUserHistoryService
    {
        private readonly IUserHistoryRepository _userHistoryRepository;

        public UserHistoryService(IUserHistoryRepository userHistoryRepository)
        {
            _userHistoryRepository = userHistoryRepository;
        }

        public async Task CreateUserHistoryAsync(UserHistory userHistory)
        {
            await _userHistoryRepository.CreateUserHistoryAsync(userHistory);
        }
    }
}
