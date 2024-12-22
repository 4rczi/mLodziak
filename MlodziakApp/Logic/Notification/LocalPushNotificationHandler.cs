using CommunityToolkit.Mvvm.Messaging;
using MlodziakApp.ApiRequests;
using MlodziakApp.Messages;
using MlodziakApp.Services;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Logic.Notification
{
    public class LocalPushNotificationHandler : INotificationHandler
    {
        private readonly IApplicationLoggingRequests _applicationLogger;
        private readonly ISecureStorageService _secureStorageService;


        public LocalPushNotificationHandler(IApplicationLoggingRequests applicationLogger,
                                            ISecureStorageService secureStorageService)
        {
            _applicationLogger = applicationLogger;
            _secureStorageService = secureStorageService;
        }

        private void OnLocalPushNotificationTapped(NotificationActionEventArgs e)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SendNotificationAsync(NotificationRequestModel notificationRequest)
        {
            try
            {
                var convertedLocalNotification = notificationRequest.ToLocalNotification();
                await LocalNotificationCenter.Current.Show(convertedLocalNotification);
                return true;
            }
            catch (Exception ex )
            {
                await _applicationLogger.LogAsync("Warning", "LocalNotification message sending failed", "", ex.Message, this.GetType().Name, nameof(SendNotificationAsync), await _secureStorageService.GetUserIdAsync() ?? "Unknown", await _secureStorageService.GetUserIdAsync() ?? "Unknown", "", DateTime.UtcNow, DateTime.UtcNow);
                return false;
            }
        }

        public Task<NotificationRequestModel> CreateNotificationRequestAsync(PhysicalLocationModel visitedLocation)
        {
            const string basicTitle = "Odwiedzono nową lokację - ";

            var notificationRequest = new NotificationRequestModel()
            {
                Title = basicTitle + visitedLocation.Name,
            };

            return Task.FromResult(notificationRequest);
        }

        public async Task<bool> CanSendNotificationAsync()
        {
            // has appropriate permissions
            return true;
        }
    }
}
