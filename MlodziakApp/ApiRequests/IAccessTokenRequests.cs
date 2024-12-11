using System.Net;

namespace MlodziakApp.ApiRequests
{
    public interface IAccessTokenRequests
    {
        Task<(bool isSuccess, HttpStatusCode statusCode)> ValidateAccessTokenAsync(string accessToken);
    }
}