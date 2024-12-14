using DataAccess.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MlodziakApp.Utilities;
using MlodziakApp.Services;
using SharedModels;
using MlodziakApp.Logic.Notification;
using MlodziakApp.ApiCalls;
using Refit;
using Azure;
using Microsoft.IdentityModel.Tokens;

namespace MlodziakApp.ApiRequests
{
    public class NotificationRequests : INotificationRequests
    {
        private readonly IApplicationLoggingRequests _applicationLoggingRequests;
        private readonly INotificationApiCalls _notificationApiCalls;
        private readonly ISecureStorageService _secureStorageService;

        public NotificationRequests(IApplicationLoggingRequests applicationLogger, INotificationApiCalls notificationApiCalls, ISecureStorageService secureStorageService)
        {
            _applicationLoggingRequests = applicationLogger;
            _notificationApiCalls = notificationApiCalls;
            _secureStorageService = secureStorageService;
        }

        public async Task<bool> SendFCMNotificationMessageRequestAsync(string? accessToken, NotificationRequestModel notificationRequest)
        {
            try
            {
                var response = await _notificationApiCalls.SendNotificationAsync(notificationRequest, $"Bearer {accessToken}");
                return response.IsNullOrEmpty();
            }

            catch (ApiException apiEx)
            {
                await _applicationLoggingRequests.LogAsync(
                    "Warning",
                    "Unsuccessful code returned",
                    "",
                    "",
                    this.GetType().Name,
                    nameof(SendFCMNotificationMessageRequestAsync),
                    await _secureStorageService.GetUserIdAsync() ?? "Unknown",
                    await _secureStorageService.GetSessionIdAsync() ?? "Unknown",
                    apiEx.StatusCode.ToString(),
                    DateTime.UtcNow,
                    DateTime.UtcNow);

                return false;
            }
            catch (Exception ex)
            {
                await _applicationLoggingRequests.LogAsync(
                    "Error",
                    "Exception caught",
                    "",
                    ex.Message,
                    this.GetType().Name,
                    nameof(SendFCMNotificationMessageRequestAsync),
                    await _secureStorageService.GetUserIdAsync() ?? "Unknown",
                    await _secureStorageService.GetSessionIdAsync() ?? "Unknown",
                    "",
                    DateTime.UtcNow,
                    DateTime.UtcNow);
                return false;
            }
        }
    }
}
