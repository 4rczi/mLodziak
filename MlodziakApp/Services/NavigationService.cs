using CommunityToolkit.Mvvm.Messaging;
using MlodziakApp.ApiRequests;
using MlodziakApp.Messages;
using MlodziakApp.Messages.MessageItems;
using MlodziakApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Services
{
    public class NavigationService
    {
        private readonly ISessionService _sessionService;
        private readonly ILocationRequests _locationRequests;
        private readonly IPhysicalLocationRequests _physicalLocationRequests;


        public NavigationService(ISessionService sessionService, ILocationRequests locationRequests, IPhysicalLocationRequests physicalLocationRequests)
        {
            _sessionService = sessionService;
            _locationRequests = locationRequests;
            _physicalLocationRequests = physicalLocationRequests;
        }

        public async Task NavigateToPhysicalLocationAsync(FCMPushNotificationPendingMessage message)
        {
            var (isSessionValid, accessToken, refreshToken, sessionId, userId) = await _sessionService.ValidateSessionAsync();
            if (!isSessionValid)
            {
                await _sessionService.HandleInvalidSessionAsync(isLoggedIn: true);
            }

            var physicalLocationInfo = message.Value._FCMPushNotificationTappedMessageItem;

            var locationModel = await _locationRequests.GetSingleLocationModelAsync(accessToken!, int.Parse(physicalLocationInfo.PhysicalLocationId), userId!, sessionId!);
            var physicalLocationModel = await _physicalLocationRequests.GetSinglePhysicalLocationAsync(accessToken, int.Parse(physicalLocationInfo.PhysicalLocationId), userId, sessionId);

            await Shell.Current.GoToAsync($"//{nameof(ExplorationPage)}/{nameof(MapPage)}");
            WeakReferenceMessenger.Default.Send(new LocationInfoMessage(new LocationInfoMessageItem(locationModel.Id,
                                                                                                    locationModel.CategoryId,
                                                                                                    locationModel.Latitude,
                                                                                                    locationModel.Longitude,
                                                                                                    locationModel.ZoomLevel,
                                                                                                    physicalLocationModel)));
        }
    }
}
