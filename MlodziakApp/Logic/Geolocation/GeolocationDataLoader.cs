using Microsoft.Identity.Client.Extensibility;
using MlodziakApp.ApiRequests;
using MlodziakApp.Services;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Logic.Geolocation
{
    public class GeolocationDataLoader : IGeolocationDataLoader
    {
        private readonly ISessionService _sessionService;
        private readonly IPhysicalLocationRequests _physicalLocationRequests;


        public GeolocationDataLoader(ISessionService sessionService, IPhysicalLocationRequests physicalLocationRequests)
        {
            _sessionService = sessionService;
            _physicalLocationRequests = physicalLocationRequests;
        }

        public async Task<List<PhysicalLocationModel>> GetVisitablePhysicalLocations()
        {
            var visitablePhysicalLocations = new List<PhysicalLocationModel>();

            var (isSessionValid, accessToken, refreshToken, sessionId, userId) = await _sessionService.ValidateSessionAsync();
            if (isSessionValid)
            {
                visitablePhysicalLocations = await _physicalLocationRequests.GetVisitablePhysicalLocationModelsAsync(accessToken!, userId!, sessionId!);
            }

            return visitablePhysicalLocations;
        }
    }

}
