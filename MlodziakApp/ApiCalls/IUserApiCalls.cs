using DataAccess.Entities;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.ApiCalls
{
    public interface IUserApiCalls
    {
        [Get("/api/user/{userId}")]
        Task<User?> GetUserAsync([Header("Authorization")] string accessToken, string userId);

        [Post("/api/user")]
        Task<object> CreateNewUserAsync([Body] User user, [Header("Authorization")] string accessToken);
    }
}
