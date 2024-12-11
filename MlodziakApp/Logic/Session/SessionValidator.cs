using MlodziakApp.ApiRequests;
using MlodziakApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Logic.Session
{
    public class SessionValidator : ISessionValidator
    {
        private readonly ISessionDataHandler _sessionDataHandler;
        private readonly ITokenService _tokenService;
        private readonly ISecureStorageService _secureStorageService;
        private readonly IApplicationLoggingRequests _applicationLogger;
        private readonly IUserService _userService;


        public SessionValidator(ISessionDataHandler sessionDataHandler, ITokenService tokenService, ISecureStorageService secureStorageService, IApplicationLoggingRequests applicationLogger, IUserService userService)
        {
            _sessionDataHandler = sessionDataHandler;
            _tokenService = tokenService;
            _secureStorageService = secureStorageService;
            _applicationLogger = applicationLogger;
            _userService = userService;
        }

        public async Task<(bool isSessionValid, string? accessToken, string? refreshToken, string? sessionId, string? userId)> ValidateSessionAsync()
        {
            try
            {
                var (success, accessToken, refreshToken, sessionId, userId) = await _sessionDataHandler.GetSessionDataAsync();

                if (!success)
                {
                    return (false, null, null, null, null);
                }

                var tokenValidationResult = await _tokenService.ValidateTokensAsync(accessToken!, refreshToken!, userId!);
                if (!tokenValidationResult)
                {        
                    return (false, null, null, null, null);                  
                }

                var user = await _userService.GetUserAsync(userId!, accessToken!);
                if (user == null)
                {
                    return (false, null, null, null, null);
                }
             
                return (true, accessToken!, refreshToken!, sessionId!, userId!);
            }

            catch (Exception ex)
            {
                await _applicationLogger.LogAsync("Warning", "Exception caught", "", ex.Message, this.GetType().Name, nameof(ValidateSessionAsync), await _secureStorageService.GetUserIdAsync() ?? "Unknown", await _secureStorageService.GetSessionIdAsync() ?? "Unknown", "", DateTime.UtcNow, DateTime.UtcNow);
                return (false, null, null, null, null);
            }
        }
    }
}
