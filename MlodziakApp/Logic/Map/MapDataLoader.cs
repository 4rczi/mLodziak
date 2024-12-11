using Microsoft.IdentityModel.Tokens;
using MlodziakApp.ApiRequests;
using MlodziakApp.Services;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MlodziakApp.Logic.Map
{
    public class MapDataLoader : IMapDataLoader
    {
        private readonly ISessionService _sessionService;
        private readonly IConnectivityService _connectivityService;
        private readonly IPhysicalLocationRequests _physicalLocationRequests;


        public MapDataLoader(ISessionService sessionService, IConnectivityService connectivityService, IPhysicalLocationRequests physicalLocationRequests)
        {
            _sessionService = sessionService;
            _connectivityService = connectivityService;
            _physicalLocationRequests = physicalLocationRequests;
        }

        public async Task<List<PhysicalLocationModel>> LoadPhysicalLocationModelsAsync(int locationId, int categoryId)
        {

            var (isSessionValid, accessToken, refreshToken, sessionId, userId) = await _sessionService.ValidateSessionAsync();
            var hasInternetConnection = await _connectivityService.HasInternetConnectionAsync();

            if (!isSessionValid)
            {
                await _sessionService.HandleInvalidSessionAsync(isLoggedIn: true, notifyUser: true);
                return [];
            }

            if (!hasInternetConnection)
            {
                await _connectivityService.HandleNoInternetConnectionAsync();
                return [];
            }

            var physicalLocationModels = await _physicalLocationRequests.GetPhysicalLocationModelsAsync(accessToken!, userId!, categoryId, locationId, sessionId!);
            return physicalLocationModels;

        }
    }
}
