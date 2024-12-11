using Auth0.OidcClient;
using IdentityModel.OidcClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Maui.Controls;
using MlodziakApp.ApiRequests;
using MlodziakApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Logic.Authentication
{
    public class Auth0LoginHandler : IAuth0LoginHandler
    {
        private readonly Auth0Client _auth0Client;
        private readonly IApplicationLoggingRequests _applicationLogger;
        private readonly IConfiguration _configuration;
        private readonly ISecureStorageService _secureStorageService;


        public Auth0LoginHandler(Auth0Client auth0Client,
            IApplicationLoggingRequests applicationLogger,
            IConfiguration configuration)
        {
            _auth0Client = auth0Client;
            _applicationLogger = applicationLogger;
            _configuration = configuration;
        }

        public async Task<LoginResult?> LoginAuth0FormAsync()
        {
            try
            {
                var auth0FormLoginResult = await _auth0Client.LoginAsync(new { audience = _configuration["Auth0:Audience"] });
                if (auth0FormLoginResult.IsError && auth0FormLoginResult.Error != "UserCancel")
                {
                    await _applicationLogger.LogAsync("Warning", "Error occured during Auth0LoginForm ", "", "", this.GetType().Name, nameof(LoginAuth0FormAsync), "Unknown", "Unknown", auth0FormLoginResult.ErrorDescription, DateTime.UtcNow, DateTime.UtcNow);
                    return null;
                }

                return auth0FormLoginResult;
            }
            catch (Exception ex)
            {
                await _applicationLogger.LogAsync("Error", "Auth0 client error occurred", "", ex.Message, this.GetType().Name, nameof(LoginAuth0FormAsync), await _secureStorageService.GetUserIdAsync() ?? "Unknown", await _secureStorageService.GetSessionIdAsync() ?? "Unknown", "", DateTime.UtcNow, DateTime.UtcNow);
                return null;
            }
        }
    }
}
