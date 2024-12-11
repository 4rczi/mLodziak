using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.ApiCalls
{
    public interface IAccessTokenApiCalls
    {
        [Get("/api/accesstoken/validate")]
        Task<ApiResponse<object>> GetAccessTokenAsync([Header("Authorization")] string accessToken);
    }
}
