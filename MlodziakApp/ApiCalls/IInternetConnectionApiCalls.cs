using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.ApiCalls
{
    public interface IInternetConnectionApiCalls
    {
        [Get("")]
        Task<ApiResponse<string>> PingAsync();
    }
}
