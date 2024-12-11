using Auth0.OidcClient;
using DataAccess.Entities;
using IdentityModel.OidcClient.Results;
using MlodziakApp.ApiRequests;
using MlodziakApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Logic.Token
{
    public class TokenRefresher : ITokenRefresher
    {
        private readonly ISecureStorageService _secureStorageService;
        private readonly Auth0Client _auth0Client;
        private readonly IApplicationLoggingRequests _applicationLogger;


        public TokenRefresher(ISecureStorageService secureStorageService, Auth0Client auth0Client, IApplicationLoggingRequests applicationLogger)
        {
            _secureStorageService = secureStorageService;
            _auth0Client = auth0Client;
            _applicationLogger = applicationLogger;
        }

        public async Task<RefreshTokenResult?> RefreshAuth0TokenAsync(string refreshToken, string userId)
        {
            var refreshingTokensResult = await _auth0Client.RefreshTokenAsync(refreshToken);

            if (refreshingTokensResult.IsError)
            {
                await _applicationLogger.LogAsync("Warning", "Error during refreshing tokens", "", "", this.GetType().Name, nameof(RefreshAuth0TokenAsync), userId, await _secureStorageService.GetSessionIdAsync() ?? "Unknown", "", DateTime.UtcNow, DateTime.UtcNow);
                return null;
            }

            return refreshingTokensResult;
        }
    }
}
