using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Constants
{
    public class LongRunningTasks
    {
        public static int ConnectivityCheckRequestIntervalInSeconds { get; private set; } = 10;

        public static int GeolocationCheckRequestIntervalInSeconds { get; private set; } = 10;
    }
}
