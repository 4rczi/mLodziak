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
        private readonly IPermissionsService _permissionsService;


        public MapDataLoader(ISessionService sessionService, IConnectivityService connectivityService, IPhysicalLocationRequests physicalLocationRequests, IPermissionsService permissionsService)
        {
            _sessionService = sessionService;
            _connectivityService = connectivityService;
            _physicalLocationRequests = physicalLocationRequests;
            _permissionsService = permissionsService;
        }

        public async Task<List<PhysicalLocationModel>> LoadPhysicalLocationModelsAsync(int locationId, int categoryId)
        {         
            var (isSessionValid, accessToken, refreshToken, sessionId, userId) = await _sessionService.ValidateSessionAsync();
            if (!isSessionValid)
            {
                await _sessionService.HandleInvalidSessionAsync(isLoggedIn: true, notifyUser: true);
                return [];
            }

            if (!await _connectivityService.HasInternetConnectionAsync())
            {
                await _connectivityService.HandleNoInternetConnectionAsync();
                return [];
            }

            if (!await _permissionsService.CheckRequiredPermissionsAsync())
            {
                await _permissionsService.HandleDeniedPermissionsAsync();
                return [];
            }

            var physicalLocationModels = await _physicalLocationRequests.GetPhysicalLocationModelsAsync(accessToken!, userId!, categoryId, locationId, sessionId!);
            return physicalLocationModels;

        }
    }
}
