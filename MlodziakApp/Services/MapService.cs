using Microsoft.IdentityModel.Tokens;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Maps;
using MlodziakApp.Logic.Map;
using MlodziakApp.Messages.MessageItems;
using MlodziakApp.Utilities;
using MlodziakApp.ViewModels;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace MlodziakApp.Services
{
    public class MapService : IMapService
    {
        private readonly IMapDataLoader _mapDataLoader;
        private readonly IMapInitializer _mapInitializer;
        private readonly IMapHandler _mapHandler;
        private readonly IPopUpService _pupUpService;

        private List<PhysicalLocationModel> _physicalLocationModels;

        public MapService(IMapDataLoader mapDataLoader, IMapInitializer mapInitializer, IMapHandler mapHandler, IPopUpService popUpService)
        {
            _mapDataLoader = mapDataLoader;
            _mapInitializer = mapInitializer;
            _mapHandler = mapHandler;
            _pupUpService = popUpService;
        }

        public async Task<Map> InitalizeMapAsync(object bindingContext, LocationInfoMessageItem locationInfoMessageItem)
        {
            var map = await _mapInitializer.InitializeMapAsync(bindingContext, locationInfoMessageItem);

            _physicalLocationModels = await _mapDataLoader.LoadPhysicalLocationModelsAsync(locationInfoMessageItem.LocationId, locationInfoMessageItem.CategoryId);
            if (_physicalLocationModels.IsNullOrEmpty())
            {
                await _pupUpService.ShowPopUpAsync(Constants.AlertMessages.FailedToLoadDataMessage, null);
            }            

            return map;
        }

        public PhysicalLocationModel? HandleMapClicked(Location touchPosition)
        {
            var locationClicked = _mapHandler.HandleMapClicked(_physicalLocationModels, touchPosition);
            if (locationClicked != null)
            {
                return locationClicked;
            }

            return null;    
        }
    }


}
