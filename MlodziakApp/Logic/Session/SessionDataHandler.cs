using Microsoft.IdentityModel.Tokens;
using MlodziakApp.ApiRequests;
using MlodziakApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Logic.Session
{
    public class SessionDataHandler : ISessionDataHandler
    {
        private readonly ISecureStorageService _secureStorageService;
        private readonly IApplicationLoggingRequests _applicationLogger;


        public SessionDataHandler(ISecureStorageService secureStorageService, IApplicationLoggingRequests applicationLoggingRequests)
        {
            _secureStorageService = secureStorageService;
            _applicationLogger = applicationLoggingRequests;
        }

        public async Task<bool> SetSessionDataAsync(string sessionId, string userId, string accessToken, string refreshToken)
        {
            try
            {

                var setSessionTask = _secureStorageService.SetSessionIdAsync(sessionId);
                var setAccessTokenTask = _secureStorageService.SetAccessTokenAsync(accessToken);
                var setRefreshTokenTask = _secureStorageService.SetRefreshTokenAsync(refreshToken);
                var setUserIdTask = _secureStorageService.SetUserIdAsync(userId);

                await Task.WhenAll(setSessionTask, setAccessTokenTask, setRefreshTokenTask, setUserIdTask);
                
                return true;
            }

            catch (Exception ex)
            {
                await _applicationLogger.LogAsync("Warning", "Exception caught", "", ex.Message, this.GetType().Name, nameof(SetSessionDataAsync), userId, await _secureStorageService.GetSessionIdAsync() ?? "Unknown", "", DateTime.UtcNow, DateTime.UtcNow);
                return false;
            }
        }

        public async Task<bool> RemoveSessionDataAsync()
        {
            try
            {
                var removeAccessTokenTask = _secureStorageService.RemoveAccessTokenAsync();
                var removeRefreshTokenTask = _secureStorageService.RemoveRefreshTokenAsync();
                var removeSessionIdTask = _secureStorageService.RemoveSessionIdAsync();
                var removeUserIdTask = _secureStorageService.RemoveUserIdAsync();

                await Task.WhenAll(removeAccessTokenTask, removeRefreshTokenTask, removeSessionIdTask, removeUserIdTask);

                return true;
            }
            catch (Exception ex)
            {
                await _applicationLogger.LogAsync("Warning", "Exception caught", "", ex.Message, this.GetType().Name, nameof(RemoveSessionDataAsync), await _secureStorageService.GetUserIdAsync() ?? "Unknown", await _secureStorageService.GetSessionIdAsync() ?? "Unknown", "", DateTime.UtcNow, DateTime.UtcNow);
                return false;
            }
        }

        public async Task<(bool isSuccess, string? accessToken, string? refreshToken, string? sessionId, string? userId)> GetSessionDataAsync()
        {
            var getAccessTokenTask = _secureStorageService.GetAccessTokenAsync();
            var getRefreshTokenTask = _secureStorageService.GetRefreshTokenAsync();
            var getSessionIdTask = _secureStorageService.GetSessionIdAsync();
            var getUserIdTask = _secureStorageService.GetUserIdAsync();

            await Task.WhenAll(getAccessTokenTask, getRefreshTokenTask, getSessionIdTask, getUserIdTask);

            if (!getAccessTokenTask.Result.IsNullOrEmpty()
                && !getRefreshTokenTask.Result.IsNullOrEmpty()
                && !getSessionIdTask.Result.IsNullOrEmpty()
                && !getUserIdTask.Result.IsNullOrEmpty())
            {
                return (true, await getAccessTokenTask, await getRefreshTokenTask, await getSessionIdTask, await getUserIdTask);
            }

            return (false, null, null, null, null);
        }
    }
}
