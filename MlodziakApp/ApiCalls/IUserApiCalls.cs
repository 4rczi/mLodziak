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
        [Get("/api/user")]
        Task<User?> GetUserAsync([Query]string userId, [Header("Authorization")] string accessToken);

        [Post("/api/user")]
        Task<object> CreateNewUserAsync([Body] User user, [Header("Authorization")] string accessToken);
    }
}
