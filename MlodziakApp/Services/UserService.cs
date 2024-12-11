using DataAccess.Entities;
using MlodziakApp.ApiRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRequests _userRequests;


        public UserService(IUserRequests userRequests)
        {
            _userRequests = userRequests;
        }

        public async Task<bool> SyncUserAsync(string userId, string accessToken)
        {
            //TODO: In the future user syncing should be done via webhooks
            var userEntity = await _userRequests.GetUserAsync(userId, accessToken);
            if (userEntity != null)
            {
                return true;
            }

            var syncingResult = await _userRequests.CreateNewUserAsync(userId, accessToken);

            return syncingResult;
        }

        public async Task<User?> GetUserAsync(string userId, string accessToken)
        {
            var user = await _userRequests.GetUserAsync(userId, accessToken);
            return user;
        }
    }
}
