using SharedModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Logic.Map
{
    public class MapHandler : IMapHandler
    {
        public PhysicalLocationModel? HandleMapClicked(List<PhysicalLocationModel> physicalLocationModels, Location touchPosition)
        {
            foreach (var physicalLocationModel in physicalLocationModels)
            {
                if (IsCircleClicked(physicalLocationModel, touchPosition))
                {
                    return physicalLocationModel;
                }
            }

            return null;
        }

        private bool IsCircleClicked(PhysicalLocationModel physicalLocationModel, Location touchPosition)
        {
            //TODO: If circles overlap, select one, whose center is closer to the touch point
            var physLocationPoint = new Location(physicalLocationModel.Latitude, physicalLocationModel.Longitude);

            double distanceInMeters = Location.CalculateDistance(touchPosition, physLocationPoint, DistanceUnits.Kilometers) * 1000;

            if (distanceInMeters < physicalLocationModel.Radius)
            {
                return true;
            }

            return false;
        }
    }
}
