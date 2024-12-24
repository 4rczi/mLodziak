using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Graphics.Text;
using MlodziakApp.ApiRequests;
using MlodziakApp.Messages;
using MlodziakApp.Messages.MessageItems;
using MlodziakApp.Services;
using MlodziakApp.Views;
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
        private readonly ISessionService _sessionService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILocationRequests _locationRequests;
        private readonly IPhysicalLocationRequests _physicalLocationRequests;


        public FCMPushNotificationHandler(IApplicationLoggingRequests applicationLogger,
                                      ISecureStorageService secureStorageService,
                                      NotificationRequests notificationRequests,
                                      IConnectivityService connectivityService,
                                      IPermissionsService permissionsService,
                                      ISessionService sessionService,
                                      IServiceProvider serviceProvider,
                                      ILocationRequests locationRequests,
                                      IPhysicalLocationRequests physicalLocationRequests)
        {
            _applicationLogger = applicationLogger;
            _secureStorageService = secureStorageService;
            _notificationRequests = notificationRequests;
            _connectivityService = connectivityService;
            _permissionsService = permissionsService;
            _sessionService = sessionService;
            _serviceProvider = serviceProvider;
            _locationRequests = locationRequests;

            WeakReferenceMessenger.Default.Register<FCMPushNotificationTappedMessage>(this, OnPushNotificationTapped);
            _physicalLocationRequests = physicalLocationRequests;
        }

        private async void OnPushNotificationTapped(object recipient, FCMPushNotificationTappedMessage message)
        {

            await HandleTappedPushNotificationAsync(message.Value);
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
            var (isSessionValid, accessToken, refreshToken, sessionId, userId) = await _sessionService.ValidateSessionAsync();
            if(!isSessionValid)
            {
                await _sessionService.HandleInvalidSessionAsync(isLoggedIn:true, notifyUser:true);
            }

            if (!await _connectivityService.HasInternetConnectionAsync())
            {
                await _connectivityService.HandleNoInternetConnectionAsync();
            }

            return await _notificationRequests.SendFCMNotificationMessageRequestAsync(accessToken, notificationMessageModel);
        }

        public async Task<NotificationRequestModel> CreateNotificationRequestAsync(PhysicalLocationModel visitedLocation)
        {
            // Both fcm and local must send data that will identtify each intent call in main activity
            const string basicMessage = "Odwiedzono nową lokalizację - ";
            var notificationRequest = new NotificationRequestModel()
            {                       
                CreationDate = DateTime.UtcNow,
                Title = basicMessage + visitedLocation.Name,
                DeviceToken = await GetFCMDeviceTokenAsync(),
                NotificationId = Guid.NewGuid().ToString(),
                PhysicalLocationId = visitedLocation.Id.ToString(),
            };

            return notificationRequest;
        }

        public async Task<bool> CanSendNotificationAsync()
        {
            var isFCMAvailableResult = await IsFCMAvailableAsync();
            var hasInternetConnectionResult = await _connectivityService.HasInternetConnectionAsync();
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                if (!await _permissionsService.CheckRequiredPermissionsAsync())
                {
                    await _permissionsService.HandleDeniedPermissionsAsync();
                }
            });

            return isFCMAvailableResult && hasInternetConnectionResult;
        }

        private async Task HandleTappedPushNotificationAsync(FCMPushNotificationTappedMessageItem physicalLocationInfo)
        {
            var (isSessionValid, accessToken, refreshToken, sessionId, userId) = await _sessionService.ValidateSessionAsync();
            if (!isSessionValid)
            {
                await _sessionService.HandleInvalidSessionAsync(isLoggedIn: true, notifyUser: true);
            }

            var locationModel = await _locationRequests.GetSingleLocationModelAsync(accessToken!, int.Parse(physicalLocationInfo.PhysicalLocationId), userId!, sessionId!);
            var physicalLocationModel = await _physicalLocationRequests.GetSinglePhysicalLocationAsync(accessToken, int.Parse(physicalLocationInfo.PhysicalLocationId), userId, sessionId);

            await Shell.Current.GoToAsync($"//{nameof(ExplorationPage)}/{nameof(MapPage)}");
            WeakReferenceMessenger.Default.Send(new LocationInfoMessage(new LocationInfoMessageItem(locationModel.Id,
                                                                                                    locationModel.CategoryId,
                                                                                                    locationModel.Latitude,
                                                                                                    locationModel.Longitude,
                                                                                                    locationModel.ZoomLevel,
                                                                                                    physicalLocationModel)));
            
            return;
        }
    }
}
