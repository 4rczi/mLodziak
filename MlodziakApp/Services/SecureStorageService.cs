using Azure;
using Azure.Core;
using MlodziakApp.ApiRequests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Services
{
    public class SecureStorageService : ISecureStorageService
    {
        private readonly ISecureStorageWrapper _secureStorageWrapper;


        public SecureStorageService(ISecureStorageWrapper secureStorageWrapper)
        {
            _secureStorageWrapper = secureStorageWrapper;
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            return await _secureStorageWrapper.GetAsync("accessToken");
        }

        public async Task SetAccessTokenAsync(string accessToken)
        {
            await _secureStorageWrapper.SetAsync("accessToken", accessToken);
        }

        public async Task RemoveAccessTokenAsync()
        {
            await _secureStorageWrapper.RemoveAsync("accessToken");
        }

        public async Task<string?> GetRefreshTokenAsync()
        {
            return await _secureStorageWrapper.GetAsync("refreshToken");
        }

        public async Task SetRefreshTokenAsync(string refreshToken)
        {           
            await _secureStorageWrapper.SetAsync("refreshToken", refreshToken);         
        }

        public async Task RemoveRefreshTokenAsync()
        {
            await _secureStorageWrapper.RemoveAsync("refreshToken");
        }

        public async Task<string?> GetSessionIdAsync()
        {
            return await _secureStorageWrapper.GetAsync("sessionId");
        }

        public async Task SetSessionIdAsync(string sessionId)
        {
            await _secureStorageWrapper.SetAsync("sessionId", sessionId);
        }

        public async Task RemoveSessionIdAsync()
        {
            await _secureStorageWrapper.RemoveAsync("sessionId");
        }

        public async Task<string?> GetUserIdAsync()
        {
            return await _secureStorageWrapper.GetAsync("userId");
        }

        public async Task SetUserIdAsync(string userId)
        {
            await _secureStorageWrapper.SetAsync("userId", userId);
        }

        public async Task RemoveUserIdAsync()
        {
            await _secureStorageWrapper.RemoveAsync("userId");
        }
    }
}
