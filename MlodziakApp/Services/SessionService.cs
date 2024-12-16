using Auth0.OidcClient;
using Azure.Core;
using Azure.Identity;
using CommunityToolkit.Mvvm.Messaging;
using DataAccess.Entities;
using IdentityModel.OidcClient.Results;
using Microsoft.IdentityModel.Tokens;
using MlodziakApp.ApiRequests;
using MlodziakApp.Logic.Session;
using MlodziakApp.Messages;
using MlodziakApp.Views;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionHandler _sessionHandler;
        private readonly ISessionDataHandler _sessionDataHandler;
        private readonly ISessionValidator _sessionValidator;
        private readonly IPopUpService _popUpService;
        private readonly IApplicationLoggingRequests _applicationLogger;


        public SessionService(
            ISessionHandler sessionHandler,
            ISessionDataHandler sessionDataHandler,
            ISessionValidator sessionValidator,
            IPopUpService popUpService,
            IApplicationLoggingRequests applicationLogger)
        {
            _sessionHandler = sessionHandler;
            _sessionDataHandler = sessionDataHandler;
            _sessionValidator = sessionValidator;
            _popUpService = popUpService;
            _applicationLogger = applicationLogger;
        }

        public async Task<(bool isSessionValid, string? accessToken, string? refreshToken, string? sessionId, string? userId)> ValidateSessionAsync(bool autoLoginCheck)
        {
            var (isSessionValid, accessToken, refreshToken, sessionId, userId) = await _sessionValidator.ValidateSessionAsync();

            if (isSessionValid && autoLoginCheck)
            {
                // We dont await handlers' results, cause geolocation service run in an infite loop
                WeakReferenceMessenger.Default.Send(new SessionInitializedMessage(true));

                await _applicationLogger.LogAsync("Information", "Sucessfully auto logged in", "", "", this.GetType().Name, nameof(ValidateSessionAsync), userId!, sessionId!, "", DateTime.UtcNow, DateTime.UtcNow);
            }

            return (isSessionValid, accessToken, refreshToken, sessionId, userId );
        }

        public async Task HandleInvalidSessionAsync(bool isLoggedIn, bool notifyUser)
        {
            if (isLoggedIn)
            {
                var sessionExpiredMessage = new SessionExpiredMessage(false);             
                WeakReferenceMessenger.Default.Send(sessionExpiredMessage);

                // Asynchronously wait for listeners' handlers to complete
                await sessionExpiredMessage.CompletionSource.Task;
            }

            await _sessionHandler.HandleInvalidSessionAsync();

            if (notifyUser)
            {
                await _popUpService.ShowPopUpAsync(Constants.AlertMessages.InvalidSessionMessage, null);
            }
            
            return;
        }

        public async Task<bool> InitializeSessionAsync(string accessToken, string refreshToken, string userId)
        {
            var (isSuccess, sessionId) = await _sessionHandler.InitializeSessionAsync(accessToken, refreshToken, userId);
            if (!isSuccess)
            {
                return false;              
            }

            await _applicationLogger.LogAsync("Information", "Initialized session", "", "", this.GetType().Name, nameof(InitializeSessionAsync), userId, sessionId!, "", DateTime.UtcNow, DateTime.UtcNow);

            // We dont await handlers' results, cause geolocation service run in an infite loop
            WeakReferenceMessenger.Default.Send(new SessionInitializedMessage(false));

            return true;
        }

        public async Task<(bool isSuccess, string? accessToken, string? refreshToken, string? sessionId, string? userId)> GetSessionDataAsync()
        {
            var (isSuccess, accessToken, refreshToken, sessionId, userId) = await _sessionDataHandler.GetSessionDataAsync();
            if (!isSuccess)
            {
                return (false, null, null, null, null);
            }

            return (true,  accessToken, refreshToken, sessionId, userId);
        }

        public async Task<bool> SetSessionDataAsync(string sessionId, string userId, string accessToken, string refreshToken)
        {
            var successResult = await _sessionDataHandler.SetSessionDataAsync(sessionId, userId, accessToken, refreshToken);
            if (!successResult)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> RemoveSessionDataAsync()
        {
            var successResult = await _sessionDataHandler.RemoveSessionDataAsync();
            if (!successResult)
            {
                return false;
            }

            return true;
        }
    }
}
