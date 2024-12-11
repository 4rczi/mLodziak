using Auth0.OidcClient;
using MlodziakApp.ApiRequests;
using MlodziakApp.Services;
using MlodziakApp.Logic.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Logic.Authentication
{
    public class Auth0LogoutHandler : IAuth0LogoutHandler
    {
        private readonly Auth0Client _auth0Client;
        private readonly IApplicationLoggingRequests _applicationLogger;
        private readonly ISecureStorageService _secureStorageService;


        public Auth0LogoutHandler(Auth0Client auth0Client, IApplicationLoggingRequests applicationLogger, ISecureStorageService secureStorageService)
        {
            _auth0Client = auth0Client;
            _applicationLogger = applicationLogger;
            _secureStorageService = secureStorageService;
        }

        public async Task<bool> LogoutAuth0Async()
        {
            try
            {
                var logOutResult = await _auth0Client.LogoutAsync();
                if (logOutResult == IdentityModel.OidcClient.Browser.BrowserResultType.Success)
                {
                    return true;
                }

                return false;
            }

            catch (Exception ex)
            {
                await _applicationLogger.LogAsync("Warning", "Exception caught", "", ex.Message, this.GetType().Name, nameof(LogoutAuth0Async), await _secureStorageService.GetUserIdAsync() ?? "Unknown", await _secureStorageService.GetSessionIdAsync() ?? "Unknown", "", DateTime.UtcNow, DateTime.UtcNow);
                return false;
            }
        }
    }
}

