﻿using MlodziakApp.ApiRequests;
using MlodziakApp.Services;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Logic.Geolocation
{
    public class GeolocationVisitHandler : IGeolocationVisitHandler
    {
        private readonly ISessionService _sessionService;
        private readonly IUserHistoryRequests _userHistoryRequests;
        private readonly IConnectivityService _connectivityService;


        public GeolocationVisitHandler(ISessionService sessionService, IUserHistoryRequests userHistoryRequests, IConnectivityService connectivityService)
        {
            _sessionService = sessionService;
            _userHistoryRequests = userHistoryRequests;
            _connectivityService = connectivityService;
        }

        public int? CanVisitLocation(Location userGeolocation, List<PhysicalLocationModel> visitablePhysicalLocationModels)
        {
            foreach (var physicalLocationModel in visitablePhysicalLocationModels)
            {
                var physicalLocationPoint = new Location(physicalLocationModel.Latitude, physicalLocationModel.Longitude);
                var distanceInMeters = Location.CalculateDistance(userGeolocation, physicalLocationPoint, DistanceUnits.Kilometers) * 1000;

                if (distanceInMeters < physicalLocationModel.Radius)
                {

                    return physicalLocationModel.Id;
                }
            }

            return null;
        }

        public async Task<bool> VisitPhysicalLocationAsync(int physicalLocationId)
        {
            var (isSessionValid, accessToken, refreshToken, sessionId, userId) = await _sessionService.ValidateSessionAsync();
            var hasInternetAccess = await _connectivityService.HasInternetConnectionAsync();

            if (isSessionValid && hasInternetAccess)
            {
                var success = await _userHistoryRequests.CreateUserHistoryAsync(userId!, physicalLocationId, accessToken!, sessionId!);
                if (success)
                {
                    return true;
                }
            }

            return false;
        }
    }
}