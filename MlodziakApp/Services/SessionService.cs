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
        private readonly ISessionValidator _sessionValidator;
        private readonly IApplicationLoggingRequests _applicationLogger;


        public SessionService(
            ISessionHandler sessionHandler,
            ISessionValidator sessionValidator,
            IApplicationLoggingRequests applicationLogger)
        {
            _sessionHandler = sessionHandler;
            _sessionValidator = sessionValidator;
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

        public async Task HandleInvalidSessionAsync(bool isLoggedIn)
        {
            if (isLoggedIn)
            {
                var sessionExpiredMessage = new SessionExpiredMessage(false);             
                WeakReferenceMessenger.Default.Send(sessionExpiredMessage);     
                await sessionExpiredMessage.CompletionSource.Task; // Asynchronously wait for listeners' handlers to complete          
            }

            await _sessionHandler.HandleInvalidSessionAsync(isLoggedIn);

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
    }
}
