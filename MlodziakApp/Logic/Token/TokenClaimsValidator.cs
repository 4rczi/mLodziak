using MlodziakApp.ApiRequests;
using MlodziakApp.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Logic.Token
{
    public class TokenClaimsValidator : ITokenClaimsValidator
    {
        private readonly IApplicationLoggingRequests _applicationLogger;
        private readonly ISecureStorageService _secureStorageService;


        public TokenClaimsValidator(IApplicationLoggingRequests applicationLogger, ISecureStorageService secureStorageService)
        {
            _applicationLogger = applicationLogger;
            _secureStorageService = secureStorageService;
        }

        public async Task<string?> GetUserIdFromAccessTokenAsync(string accessToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();

                var token = handler.ReadToken(accessToken) as JwtSecurityToken;
                if (token == null)
                {
                    await _applicationLogger.LogAsync("Warning", "Given token is not valid JwtSecurityToken", "", "", this.GetType().Name, nameof(GetUserIdFromAccessTokenAsync), await _secureStorageService.GetUserIdAsync() ?? "Unknown", await _secureStorageService.GetSessionIdAsync() ?? "Unknown", "", DateTime.UtcNow, DateTime.UtcNow);
                    return null;
                }

                var userId = token.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                if (userId == null)
                {
                    await _applicationLogger.LogAsync("Error", "Couldn't retrieve UserId from access token", "", "", this.GetType().Name, nameof(GetUserIdFromAccessTokenAsync), await _secureStorageService.GetUserIdAsync() ?? "Unknown", await _secureStorageService.GetSessionIdAsync() ?? "Unknown", "", DateTime.UtcNow, DateTime.UtcNow);
                    return null;
                }

                return userId;

            }
            catch (Exception ex)
            {
                await _applicationLogger.LogAsync("Error", "Exception caught", "", ex.Message, this.GetType().Name, nameof(GetUserIdFromAccessTokenAsync), await _secureStorageService.GetUserIdAsync() ?? "Unknown", await _secureStorageService.GetSessionIdAsync() ?? "Unknown", "", DateTime.UtcNow, DateTime.UtcNow);
                return null;
            }
        }
    }
}
