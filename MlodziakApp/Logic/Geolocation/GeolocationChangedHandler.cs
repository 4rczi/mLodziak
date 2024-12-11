using CommunityToolkit.Mvvm.Messaging;
using MlodziakApp.ApiRequests;
using MlodziakApp.Messages;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Logic.Geolocation
{
    public class GeolocationChangedHandler : IGeolocationChangedHandler
    {
        private readonly IGeolocationVisitHandler _geolocationVisitHandler;
        private readonly IGeolocationDataLoader _geolocationDataLoader;
        private readonly IPhysicalLocationRequests _physicalLocationRequests;

        private bool _shouldLoadData = true;

        private List<PhysicalLocationModel> _visitablePhysicalLocationModels;

        public GeolocationChangedHandler(IGeolocationVisitHandler geolocationVisitHandler, IPhysicalLocationRequests physicalLocationRequests, IGeolocationDataLoader geolocationDataLoader)
        {
            _geolocationVisitHandler = geolocationVisitHandler;
            _physicalLocationRequests = physicalLocationRequests;
            _geolocationDataLoader = geolocationDataLoader;
        }

        public async Task<PhysicalLocationModel?> HandleUserGeolocationChangeAsync(Location? userGeolocation)
        {
            if (userGeolocation == null)
            {
                return null;
            }

            if (_shouldLoadData)
            {
                _visitablePhysicalLocationModels = await _geolocationDataLoader.GetVisitablePhysicalLocations();
                _shouldLoadData = false;
            }

            var visitablePhysicalLocationId = _geolocationVisitHandler.CanVisitLocation(userGeolocation, _visitablePhysicalLocationModels);

            if (visitablePhysicalLocationId != null)
            {
                var visitationResult = await _geolocationVisitHandler.VisitPhysicalLocationAsync((int)visitablePhysicalLocationId);
                if (visitationResult)
                {
                    var visitedPhysicalLocationModel = _visitablePhysicalLocationModels.Where(physLoc => physLoc.Id == visitablePhysicalLocationId).First();
                    _shouldLoadData = true;
                    return visitedPhysicalLocationModel;
                }            
            }

            return null;

        }
    }
}
