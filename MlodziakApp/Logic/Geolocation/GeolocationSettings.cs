using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Logic.Geolocation
{
    public static class GeolocationSettings
    {
        public static GeolocationAccuracy GeolocationAccuracy { get; } = GeolocationAccuracy.Best;

        public static int GeolocationRequestIntervalInSeconds { get; } = 8;
    }
}
