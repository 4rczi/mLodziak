using Azure.Core;
using DataAccess.Entities;
using DataAccess.Repositories;
using IdentityModel.OidcClient;
using MlodziakApp.ApiRequests;
using MlodziakApp.Logic.Authentication;
using MlodziakApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuth0LoginHandler _loginHandler;
        private readonly IAuth0LogoutHandler _logoutHandler;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly Lazy<ISessionService> _sessionService;
        private readonly IApplicationLoggingRequests _applicationLogger;
        private readonly ISecureStorageService _secureStorageService;


        public AuthenticationService(IAuth0LoginHandler loginHandler,
            IAuth0LogoutHandler logoutHandler,
            IUserService userService,
            ITokenService tokenService,
            Lazy<ISessionService> sessionService,
            IApplicationLoggingRequests applicationLogger,
            ISecureStorageService secureStorageService)
        {
            _loginHandler = loginHandler;
            _logoutHandler = logoutHandler;
            _userService = userService;
            _tokenService = tokenService;
            _sessionService = sessionService;
            _applicationLogger = applicationLogger;
            _secureStorageService = secureStorageService;
        }

        public async Task<bool> LoginAsync()
        {
            var auth0LoginResult = await _loginHandler.LoginAuth0FormAsync();
            if (auth0LoginResult == null)
            {
                return false;
            }

            var userId = await _tokenService.GetUserIdFromAccessTokenAsync(auth0LoginResult.AccessToken);
            if (userId == null)
            {
                return false;
            }

            var userSyncingResult = await _userService.SyncUserAsync(userId, auth0LoginResult.AccessToken);
            if (!userSyncingResult)
            {
                return false;
            }

            var sesionInitializationResult = await _sessionService.Value.InitializeSessionAsync(auth0LoginResult.AccessToken!, auth0LoginResult.RefreshToken!, userId!);
            if (!sesionInitializationResult)
            {
                return false;
            }

            await _applicationLogger.LogAsync("Information", "Logged in", "", "", this.GetType().Name, nameof(LoginAsync), await _secureStorageService.GetUserIdAsync() ?? "Unknown", await _secureStorageService.GetSessionIdAsync() ?? "Unknown", "", DateTime.UtcNow, DateTime.UtcNow);


            return true;
        }

        public async Task<bool> LogoutAsync()
        {        
            await _applicationLogger.LogAsync("Information", "Logging out", "", "", this.GetType().Name, nameof(LogoutAsync), await _secureStorageService.GetUserIdAsync() ?? "Unknown", await _secureStorageService.GetSessionIdAsync() ?? "Unknown", "", DateTime.UtcNow, DateTime.UtcNow);

            var logoutResult = await _logoutHandler.LogoutAuth0Async();
            if (!logoutResult)
            {
                return false;
            }

            await _sessionService.Value.HandleInvalidSessionAsync(isLoggedIn: true, notifyUser: true);

            await Shell.Current.GoToAsync($"//{nameof(InvitationPage)}");

            return true;   
        }
            
    }
}
