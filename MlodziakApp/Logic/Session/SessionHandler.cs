using Azure.Core;
using Microsoft.IdentityModel.Tokens;
using MlodziakApp.ApiRequests;
using MlodziakApp.Services;
using MlodziakApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Logic.Session
{
    public class SessionHandler : ISessionHandler
    {
        private readonly ISessionDataHandler _sessionDataHandler;
        private readonly IAuthenticationService _authenticationService;
        private readonly IPopUpService _popUpService;

        public SessionHandler(
            ISessionDataHandler sessionDataHandler,
            IAuthenticationService authenticationService,
            IPopUpService popUpService)
        {
            _sessionDataHandler = sessionDataHandler;
            _authenticationService = authenticationService;
            _popUpService = popUpService;
        }

        public async Task<(bool isSuccess, string? sessionId)> InitializeSessionAsync(string accessToken, string refreshToken, string userId)
        {
            var sessionId = CreateSessionId();

            var settingSessioDataResult = await _sessionDataHandler.SetSessionDataAsync(sessionId, userId, accessToken, refreshToken);
            if (!settingSessioDataResult)
            {
                return (false, null);
            }

            return (true, sessionId);
        }

        public async Task HandleInvalidSessionAsync(bool isLoggedIn)
        {          
            if (isLoggedIn)
            {
                await _popUpService.ShowPopUpAsync(Constants.AlertMessages.InvalidSessionMessage, null);
                await _authenticationService.LogoutAsync();
                await _sessionDataHandler.RemoveSessionDataAsync();
                await Shell.Current.GoToAsync($"//{nameof(InvitationPage)}");
                return;
            }

            await _sessionDataHandler.RemoveSessionDataAsync();
            return;
        }

        private string CreateSessionId()
        {
            return Guid.NewGuid().ToString();
        }

        
    }
}
