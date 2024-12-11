using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Messages.MessageItems
{
    public class UserGeolocationMessageItem
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public UserGeolocationMessageItem(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
