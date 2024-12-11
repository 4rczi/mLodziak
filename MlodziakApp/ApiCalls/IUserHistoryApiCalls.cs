using DataAccess.Entities;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.ApiCalls
{
    public interface IUserHistoryApiCalls
    {
        [Post("/api/userhistory")]
        Task<bool> CreateUserHistoryAsync(
            [Body] UserHistory userHistory,
            [Header("Authorization")] string accessToken);
    }
}
