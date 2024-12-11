using Auth0.OidcClient;
using Microsoft.IdentityModel.Tokens;
using MlodziakApp.ApiRequests;
using MlodziakApp.Logic.Token;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Services
{
    public class TokenService : ITokenService
    {
        private readonly IApplicationLoggingRequests _applicationLogger;
        private readonly ISecureStorageService _secureStorageService;
        private readonly ITokenClaimsValidator _accessTokenClaimsValidator;
        private readonly ITokenValidator _tokenValidator;
        private readonly ITokenRefresher _tokenRefresher;


        public TokenService(IApplicationLoggingRequests applicationLogger,
            ISecureStorageService secureStorageService,
            ITokenClaimsValidator accessTokenClaimsValidator,
            ITokenValidator tokenValidator,
            ITokenRefresher tokenRefresher)
        {
            _applicationLogger = applicationLogger;
            _secureStorageService = secureStorageService;
            _accessTokenClaimsValidator = accessTokenClaimsValidator;
            _tokenValidator = tokenValidator;
            _tokenRefresher = tokenRefresher;
        }

        public async Task<bool> ValidateTokensAsync(string accessToken, string refreshToken, string userId)
        {
            var accessTokenValidationResult = await ValidateAccessTokenAsync(accessToken, userId);
            var refreshTokenValidationResult = ValidateRefreshToken(refreshToken);

            return accessTokenValidationResult && refreshTokenValidationResult;
        }

        private async Task<bool> ValidateAccessTokenAsync(string accessToken, string userId)
        {
            var (isSuccess, statusCode) = await _tokenValidator.ValidateAccessTokenAsync(accessToken, userId);

            if (!isSuccess && statusCode == HttpStatusCode.Unauthorized)
            {
                var tokenRefreshResult = await RefreshTokensAsync(userId!);
                if (!tokenRefreshResult)
                {
                    return false;
                }

                return true;
            }

            return isSuccess;
        }

        private bool ValidateRefreshToken(string refreshToken)
        {
            var refreshTokenValidationResult = _tokenValidator.ValidateRefreshToken(refreshToken);
            return refreshTokenValidationResult;
        }

        private async Task<bool> RefreshTokensAsync(string userId)
        {
            try
            {
                var currentRefreshToken = await _secureStorageService.GetRefreshTokenAsync();
                if (currentRefreshToken == null)
                {
                    return false;
                }

                var refreshingAuth0TokensResult = await _tokenRefresher.RefreshAuth0TokenAsync(currentRefreshToken, userId);
                if (refreshingAuth0TokensResult == null)
                {
                    return false;
                }

                var replacingTokensResult = await ReplaceTokensAsync(refreshingAuth0TokensResult.AccessToken, refreshingAuth0TokensResult.RefreshToken, userId);
                if (!replacingTokensResult)
                {
                    return false;
                }

                await _applicationLogger.LogAsync("Information", "Successfully refreshed tokens", "", "", this.GetType().Name, nameof(RefreshTokensAsync), userId, await _secureStorageService.GetSessionIdAsync() ?? "Unknown", "", DateTime.UtcNow, DateTime.UtcNow);

                return true;
            }
            catch (Exception ex)
            {
                await _applicationLogger.LogAsync("Warning", "Exception caught", "", ex.Message, this.GetType().Name, nameof(RefreshTokensAsync), userId, await _secureStorageService.GetSessionIdAsync() ?? "Unknown", "", DateTime.UtcNow, DateTime.UtcNow);
                return false;
            }
        }

        public async Task<string?> GetUserIdFromAccessTokenAsync(string accessToken)
        {
            return await _accessTokenClaimsValidator.GetUserIdFromAccessTokenAsync(accessToken);
        }

        private async Task<bool> ReplaceTokensAsync(string accessToken, string refreshToken, string userId)
        {
            try
            {
                var settingAccessTokenTask = _secureStorageService.SetAccessTokenAsync(accessToken);
                var settingRefreshTokenTask = _secureStorageService.SetRefreshTokenAsync(refreshToken);

                await Task.WhenAll(settingAccessTokenTask, settingRefreshTokenTask);

                return true;
            }
            catch (Exception ex)
            {
                await _applicationLogger.LogAsync("Warning", "Exception caught", "", ex.Message, this.GetType().Name, nameof(ReplaceTokensAsync), userId, await _secureStorageService.GetSessionIdAsync() ?? "Unknown", "", DateTime.UtcNow, DateTime.UtcNow);
                return false;
            }
        }
    }
}
