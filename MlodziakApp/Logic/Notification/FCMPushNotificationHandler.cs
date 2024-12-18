using CommunityToolkit.Maui.Converters;
using MlodziakApp.ApiRequests;
using MlodziakApp.Services;
using Plugin.Firebase.CloudMessaging;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Logic.Notification
{
    public class FCMPushNotificationHandler : INotificationHandler
    {
        private readonly IApplicationLoggingRequests _applicationLogger;
        private readonly ISecureStorageService _secureStorageService;
        private readonly NotificationRequests _notificationRequests;
        private readonly IConnectivityService _connectivityService;
        private readonly IPermissionsService _permissionsService;


        public FCMPushNotificationHandler(IApplicationLoggingRequests applicationLogger,
                                      ISecureStorageService secureStorageService,
                                      NotificationRequests notificationRequests,
                                      IConnectivityService connectivityService,
                                      IPermissionsService permissionsService)
        {
            _applicationLogger = applicationLogger;
            _secureStorageService = secureStorageService;
            _notificationRequests = notificationRequests;
            _connectivityService = connectivityService;
            _permissionsService = permissionsService;
        }

        private async Task<bool> CanReceiveFCMMessagesAsync()
        {
            try
            {
                await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
                return true;
            }

            catch (Exception ex)
            {
                await _applicationLogger.LogAsync("Warning", "FCM is not available", "", ex.Message, this.GetType().Name, nameof(CanReceiveFCMMessagesAsync), await _secureStorageService.GetUserIdAsync() ?? "Unknown", await _secureStorageService.GetUserIdAsync() ?? "Unknown", "", DateTime.UtcNow, DateTime.UtcNow);
                return false;               
            }
        }

        private async Task<bool> IsFCMAvailableAsync()
        {
            return (await CanReceiveFCMMessagesAsync() && (!string.IsNullOrEmpty(await GetFCMDeviceTokenAsync())));
        }

        private async Task<string?> GetFCMDeviceTokenAsync()
        {
            return await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
        }

        public async Task<bool> SendNotificationAsync(NotificationRequestModel notificationMessageModel)
        {
            var accessToken = await _secureStorageService.GetAccessTokenAsync();
            if (accessToken == null)
            {
                return false;
            }

            return await _notificationRequests.SendFCMNotificationMessageRequestAsync(accessToken, notificationMessageModel);
        }

        public async Task<NotificationRequestModel> CreateNotificationRequestAsync(PhysicalLocationModel visitedLocation)
        {
            const string basicMessage = "Odwiedzono nową lokalizację - ";
            var notificationRequest = new NotificationRequestModel()
            {
                Title = basicMessage + visitedLocation.Name,
                DeviceToken = await GetFCMDeviceTokenAsync()
            };

            return notificationRequest;
        }

        public async Task<bool> CanSendNotificationAsync()
        {
            var isFCMAvailableResult = await IsFCMAvailableAsync();
            var hasInternetConnectionResult = await _connectivityService.HasInternetConnectionAsync();
            var hasRequiredPermissions = await _permissionsService.CheckRequiredPermissionsAsync();

            return isFCMAvailableResult && hasInternetConnectionResult && hasRequiredPermissions;
        }
    }
}
