using CommunityToolkit.Mvvm.Messaging;
using MlodziakApp.ApiRequests;
using MlodziakApp.Logic.Notification;
using MlodziakApp.Messages;
using Plugin.Firebase.CloudMessaging;
using Plugin.LocalNotification;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Services
{
    public class PushNotificationService
    {
        private readonly IGeolocationService _geolocationService;
        private readonly ISecureStorageService _secureStorageService;
        private readonly IApplicationLoggingRequests _applicationLoggingRequests;
        private readonly IEnumerable<INotificationHandler> _notificationHandlers;

        public PushNotificationService(IGeolocationService geolocationService,
                                       ISecureStorageService secureStorageService,
                                       IApplicationLoggingRequests applicationLoggingRequests,
                                       IEnumerable<INotificationHandler> notificationHandlers
                                      )
        {
            _geolocationService = geolocationService;
            _secureStorageService = secureStorageService;
            _applicationLoggingRequests = applicationLoggingRequests;
            _notificationHandlers = notificationHandlers;

            WeakReferenceMessenger.Default.Register<VisitedPhysicalLocationMessage>(this, VisitedPhysicalLocationMessageReceived);
        }

        private async void VisitedPhysicalLocationMessageReceived(object recipient, VisitedPhysicalLocationMessage message)
        {
            await HandleLocationVisitedAsync(message.Value);
        }

        private async Task HandleLocationVisitedAsync(PhysicalLocationModel visitedLocation)
        {
            try
            {
                foreach (var handler in _notificationHandlers)
                {
                    if (handler != null && await handler.CanSendNotificationAsync())
                    {
                        var pushMessageRequest = await handler.CreateNotificationRequestAsync(visitedLocation);
                        await handler.SendNotificationAsync(pushMessageRequest);
                        return;
                    }
                }

                // Should cache unsent notification. Once a handler is ready it should send them again(if they are not too old),
                // or if many unsent notifications - perform commutation

                await _applicationLoggingRequests.LogAsync("Warning", "No notification handler is initialized", "", "", this.GetType().Name, nameof(HandleLocationVisitedAsync), await _secureStorageService.GetUserIdAsync() ?? "Unknown", await _secureStorageService.GetSessionIdAsync() ?? "Unknown", "", DateTime.UtcNow, DateTime.UtcNow);
                return;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }
}
