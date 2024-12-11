using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Messages.MessageItems
{
    public class LocationInfoMessageItem
    {
        public int LocationId { get; set; }
        public int CategoryId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public float ZoomLevel { get; set; }

        public LocationInfoMessageItem(int locationId, int categoryId, double latitude, double longitude, float zoomLevel)
        {
            LocationId = locationId;
            CategoryId = categoryId;
            Latitude = latitude;
            Longitude = longitude; 
            ZoomLevel = zoomLevel;
        }
    }
}
