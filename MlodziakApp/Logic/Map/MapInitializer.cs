using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using MlodziakApp.Messages.MessageItems;
using MlodziakApp.Utilities;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiMap = Microsoft.Maui.Controls.Maps.Map;

namespace MlodziakApp.Logic.Map
{
    public class MapInitializer : IMapInitializer
    {
        private readonly IMapDataLoader _mapDataLoader;


        public MapInitializer(IMapDataLoader mapDataLoader)
        {
            _mapDataLoader = mapDataLoader;
        }

        public async Task<MauiMap> InitializeMapAsync(object bindingContext, LocationInfoMessageItem locationInfoMessageItem)
        {
            var map = new MauiMap();
            InitializeMapProperties(map, bindingContext);
            InitializeMapPosition(map, locationInfoMessageItem.Latitude, locationInfoMessageItem.Longitude, locationInfoMessageItem.ZoomLevel);

            var physicalLocationModels = await _mapDataLoader.LoadPhysicalLocationModelsAsync(locationInfoMessageItem.LocationId, locationInfoMessageItem.CategoryId);
            InitializeCircles(map, physicalLocationModels);

            return map;
        }

        private void InitializeMapProperties(MauiMap map, object bindingContext)
        {
            map.BindingContext = bindingContext;
            map.IsScrollEnabled = false;
            map.IsZoomEnabled = false;
            map.IsShowingUser = false;
        }

        private void InitializeMapPosition(MauiMap map, double locationLatitude, double locationLongitude, float locationZoomLevel)
        {
            var locationPosition = new Location(locationLatitude, locationLongitude);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(locationPosition, Distance.FromKilometers(0.5)));
        }

        private void InitializeCircles(MauiMap map, List<PhysicalLocationModel> physicalLocationModels)
        {
            foreach (var physLocModel in physicalLocationModels)
            {
                var circle = new Circle()
                {
                    Radius = new Distance(physLocModel.Radius),
                    Center = new Location(physLocModel.Latitude, physLocModel.Longitude),
                    FillColor = SetCircleFillColor(physLocModel),
                };

                map.MapElements.Add(circle);
            }
        }

        private Color SetCircleFillColor(PhysicalLocationModel physicalLocationModel)
        {
            if (physicalLocationModel.IsOmmitted)
            {
                return Constants.MapElements.Circles.Ommitted;
            }

            if (physicalLocationModel.IsVisited)
            {
                return Constants.MapElements.Circles.Visited;
            }

            //var isStartingSoonColor = IsEventStartingSoon(physicalLocationModel.DateStart, physicalLocationModel.DateEnd);
            //if (isStartingSoonColor != null)
            //{
            //    return isStartingSoonColor;
            //}

            if (!physicalLocationModel.IsVisited)
            {
                return Constants.MapElements.Circles.NotVisited;
            }

            Debug.WriteLine("Warning! Default color was chosen!");
            return Constants.MapElements.Circles.Default;
        }

        //private Color? IsEventStartingSoon(DateTime? dateStart, DateTime? dateEnd)
        //{
        //    if (dateStart != null && dateEnd != null)
        //    {
        //        var CEDateTimeEventStart = UtcToCentralEuropeDateTimeConverter.LocalToCentralEuropeDateTime((DateTime)dateStart!);
        //        var CEDateTimeEventEnd = UtcToCentralEuropeDateTimeConverter.LocalToCentralEuropeDateTime((DateTime)dateEnd!);
        //        var CEDateTimeNow = UtcToCentralEuropeDateTimeConverter.LocalToCentralEuropeDateTime(DateTime.Now);
        //
        //        if (CEDateTimeNow < CEDateTimeEventEnd)
        //        {
        //            return Constants.MapElements.Circles.StartsSoon;
        //        }
        //    }
        //
        //    return null;
        //}
    }
}
