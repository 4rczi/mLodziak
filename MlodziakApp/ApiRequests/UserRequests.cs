
using Azure.Core;
using DataAccess.Entities;
using MlodziakApp.Services;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.CodeDom;
using MlodziakApp.Utilities;
using Refit;
using MlodziakApp.ApiCalls;

namespace MlodziakApp.ApiRequests
{
    public class UserRequests : IUserRequests
    {
        private readonly IApplicationLoggingRequests _applicationLoggingRequests;
        private readonly IUserApiCalls _userApiCalls;
        private readonly ISecureStorageService _secureStorageService;

        public UserRequests(IApplicationLoggingRequests applicationLogger, IUserApiCalls userApiCalls, ISecureStorageService secureStorageService)
        {
            _applicationLoggingRequests = applicationLogger;
            _userApiCalls = userApiCalls;
            _secureStorageService = secureStorageService;
        }

        public async Task<User?> GetUserAsync(string userId, string accessToken)
        {
            try
            {
                var user = await _userApiCalls.GetUserAsync($"Bearer {accessToken}", userId);
                return user;
            }

            catch (ApiException apiEx)
            {
                await _applicationLoggingRequests.LogAsync("Warning",
                    "Unsuccessful code returned",
                    "",
                    apiEx.Message,
                    this.GetType().Name,
                    nameof(GetUserAsync),
                    userId,
                    await _secureStorageService.GetSessionIdAsync() ?? "Unknown",
                    apiEx.StatusCode.ToString(),
                    DateTime.UtcNow,
                    DateTime.UtcNow);

                return null;
            }

            catch (Exception ex)
            {
                await _applicationLoggingRequests.LogAsync("Error",
                    "Exception caught",
                    "",
                    ex.Message,
                    this.GetType().Name,
                    nameof(GetUserAsync),
                    userId,
                    await _secureStorageService.GetSessionIdAsync() ?? "Unknown",
                    "",
                    DateTime.UtcNow,
                    DateTime.UtcNow);

                return null;
            }
        }

        public async Task<bool> CreateNewUserAsync(string userId, string accessToken)
        {
            try
            {
                var user = new User { Id = userId };
                var response = await _userApiCalls.CreateNewUserAsync(user, $"Bearer {accessToken}");

                return true;
            }

            catch (ApiException apiEx)
            {
                await _applicationLoggingRequests.LogAsync("Warning",
                    "Unsuccessful code returned",
                    "",
                    apiEx.Message,
                    this.GetType().Name,
                    nameof(CreateNewUserAsync),
                    userId,
                    await _secureStorageService.GetSessionIdAsync() ?? "Unknown",
                    apiEx.StatusCode.ToString(),
                    DateTime.UtcNow,
                    DateTime.UtcNow);

                return false;
            }

            catch (Exception ex)
            {
                await _applicationLoggingRequests.LogAsync("Error",
                    "Exception caught",
                    "",
                    ex.Message,
                    this.GetType().Name,
                    nameof(CreateNewUserAsync),
                    userId,
                    await _secureStorageService.GetSessionIdAsync() ?? "Unknown",
                    "",
                    DateTime.UtcNow,
                    DateTime.UtcNow);

                return false;
            }
        }
    }


}
