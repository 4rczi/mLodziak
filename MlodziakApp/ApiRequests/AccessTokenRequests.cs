using DataAccess.Entities;
using MlodziakApp.ApiCalls;
using MlodziakApp.Services;
using MlodziakApp.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.ApiRequests
{
    public class AccessTokenRequests : IAccessTokenRequests
    {
        private readonly IApplicationLoggingRequests _applicationLoggingRequests;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISecureStorageService _secureStorageService;
        private readonly IAccessTokenApiCalls _accessTokenApiCalls;


        public AccessTokenRequests(IApplicationLoggingRequests applicationLogger,
                                   IHttpClientFactory httpClientFactory,
                                   ISecureStorageService secureStorageService,
                                   IAccessTokenApiCalls accessTokenApiCalls)
        {
            _applicationLoggingRequests = applicationLogger;
            _httpClientFactory = httpClientFactory;
            _secureStorageService = secureStorageService;
            _accessTokenApiCalls = accessTokenApiCalls;
        }

        public async Task<(bool isSuccess, HttpStatusCode statusCode)> ValidateAccessTokenAsync(string accessToken)
        {
            try
            {
                var response = await _accessTokenApiCalls.GetAccessTokenAsync($"Bearer {accessToken}");

                if (!response.IsSuccessStatusCode)
                {
                    return (false, response.StatusCode);                 
                }

                return (true, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                await _applicationLoggingRequests.LogAsync("Error", "Exception caught", "", ex.Message,
                    this.GetType().Name, nameof(ValidateAccessTokenAsync),
                    await _secureStorageService.GetUserIdAsync() ?? "Unknown",
                    await _secureStorageService.GetSessionIdAsync() ?? "Unknown",
                    "", DateTime.UtcNow, DateTime.UtcNow);
                return (false, HttpStatusCode.InternalServerError);
            }
        }
    }
}
